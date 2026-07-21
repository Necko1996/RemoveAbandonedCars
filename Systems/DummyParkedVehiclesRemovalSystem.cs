using Game;
using Game.Common;
using Game.SceneFlow;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;

namespace RemoveAbandonedCars.Systems
{
    public partial class DummyParkedVehiclesRemovalSystem : GameSystemBase
    {
        private EntityQuery m_ConnectionQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_ConnectionQuery = SystemAPI.QueryBuilder()
                .WithAll<Game.Objects.OutsideConnection>()
                .WithAny<OwnedVehicle>()
                .WithNone<Deleted>()
                .Build();
        }

        protected override void OnUpdate()
        {
            if(!Mod.m_Setting.IsDeletionProcessActive)
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

            int globalDummyTrafficCount = 0;
            int otherCount = 0;
            int skippedConnectionCount = 0;

            var ownedVehicleHandle = SystemAPI.GetBufferTypeHandle<OwnedVehicle>(true);
            var personalCarLookup = SystemAPI.GetComponentLookup<Game.Vehicles.PersonalCar>(true);

            EntityCommandBuffer ecb = modificationBarrier.CreateCommandBuffer();

            using (var chunks = m_ConnectionQuery.ToArchetypeChunkArray(Allocator.TempJob))
            {
                for (int i = 0; i < chunks.Length; i++)
                {
                    var chunk = chunks[i];
                    BufferAccessor<OwnedVehicle> bufferAccessor = chunk.GetBufferAccessor(ref ownedVehicleHandle);

                    for (int j = 0; j < chunk.Count; j++)
                    {
                        DynamicBuffer<OwnedVehicle> vehiclesBuffer = bufferAccessor[j];

                        if (vehiclesBuffer.Length > 5)
                        {
                            for (int k = 0; k < vehiclesBuffer.Length; k++)
                            {
                                Entity vehicleEntity = vehiclesBuffer[k].m_Vehicle;

                                if (personalCarLookup.HasComponent(vehicleEntity))
                                {
                                    Game.Vehicles.PersonalCar personalCarData = personalCarLookup[vehicleEntity];

                                    if ((personalCarData.m_State & PersonalCarFlags.DummyTraffic) != 0)
                                    {
                                        globalDummyTrafficCount++;

                                        ecb.AddComponent<Deleted>(vehicleEntity);
                                    }
                                    else
                                    {
                                        otherCount++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            skippedConnectionCount++;
                        }
                    }
                }
            }

            Mod.log.Info($"Dummy traffic: {globalDummyTrafficCount}");
            Mod.log.Info($"Other traffic: {otherCount}");
            Mod.log.Info($"Connections with 0 vehicles: {skippedConnectionCount}");
            Mod.log.Info($"-------------------------------------");

            Mod.m_Setting.DeletedDummyVehiclesCount = globalDummyTrafficCount;
            Mod.m_Setting.IsDeletionProcessActive = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();            
        }
    }
}