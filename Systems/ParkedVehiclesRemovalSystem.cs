using Game;
using Game.Common;
using Game.Objects;
using Game.SceneFlow;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;

namespace RemoveAbandonedCars.Systems
{
    public partial class ParkedVehiclesRemovalSystem : GameSystemBase
    {
        private EntityQuery m_ParkedVehiclesQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_ParkedVehiclesQuery = SystemAPI.QueryBuilder()
                .WithAll<PersonalCar, ParkedCar>()
                .WithNone<Deleted, Controller, SubObject>()
                .Build();
        }

        protected override void OnUpdate()
        {
            if(!Mod.m_Setting.IsDeletionOfParkedVehiclesProcessActive)
            {
                return;
            }

            if (GameManager.instance.gameMode != GameMode.Game)
            {
                return;
            }

            var modificationBarrier = this.World.GetExistingSystemManaged<Game.Common.ModificationBarrier3>();
            if (modificationBarrier == null)
            {
                return;
            }

            int notSpawnedVehicle = 0;
            int noOwner = 0;
            int noOwnerComponenet = 0;

            var parkedCarType = SystemAPI.GetComponentTypeHandle<Game.Vehicles.ParkedCar>(true);
            var ownerLookup = SystemAPI.GetComponentLookup<Game.Common.Owner>(true);
            var entityType = SystemAPI.GetEntityTypeHandle();

            EntityCommandBuffer ecb = modificationBarrier.CreateCommandBuffer();

            using (var chunks = m_ParkedVehiclesQuery.ToArchetypeChunkArray(Allocator.TempJob))
            {
                for (int i = 0; i < chunks.Length; i++)
                {
                    var chunk = chunks[i];
                    var parkedCars = chunk.GetNativeArray(ref parkedCarType);
                    var entities = chunk.GetNativeArray(entityType);

                    for (int j = 0; j < chunk.Count; j++)
                    {
                        Entity vehicleEntity = entities[j];

                        Game.Vehicles.ParkedCar parkedCar = parkedCars[j];

                        if (parkedCar.m_Lane == Entity.Null)
                        {
                            notSpawnedVehicle++;

                            ecb.AddComponent<Deleted>(vehicleEntity);

                            //Mod.log.Info($"Missing Owner component! Entity: {vehicleEntity.Index}:{vehicleEntity.Version}");

                            continue;
                        }

                        if (ownerLookup.TryGetComponent(vehicleEntity, out Game.Common.Owner owner))
                        {
                            if (owner.m_Owner == Entity.Null)
                            {
                                noOwner++;
                            }
                        }
                        else
                        {

                            noOwnerComponenet++;
                            //Mod.log.Info($"Missing Owner component! Entity: {vehicleEntity.Index}:{vehicleEntity.Version}");
                        }
                    }
                }
            }

            Mod.log.Info($"Parked cars, not spawned: {notSpawnedVehicle}");
            Mod.log.Info($"Parked cars, no owner: {noOwner}");
            Mod.log.Info($"Parked cars, no owner componenet: {noOwnerComponenet}");
            Mod.log.Info($"-------------------------------------");

            Mod.m_Setting.DeletedParkedVehiclesCount = notSpawnedVehicle;
            Mod.m_Setting.IsDeletionOfParkedVehiclesProcessActive = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();            
        }
    }
}