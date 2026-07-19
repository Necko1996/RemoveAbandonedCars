using Game;
using Game.Common;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;

namespace RemoveAbandonedCars.Systems
{
    public partial class TrailerRemovalSystem : GameSystemBase
    {
        private EntityQuery m_TrailerQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_TrailerQuery = SystemAPI.QueryBuilder()
                .WithAll<Controller>()
                .WithAny<CarTrailerLane>()
                .WithNone<Deleted>()
                .Build();
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 512;
        }

        protected override void OnUpdate()
        {
            //int countTrailers = 0;

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            using (var chunks = m_TrailerQuery.ToArchetypeChunkArray(Allocator.TempJob))
            {
                var entityType = GetEntityTypeHandle();
                var controllerType = GetComponentTypeHandle<Controller>(true);

                for (int i = 0; i < chunks.Length; i++)
                {
                    var chunk = chunks[i];
                    var chunkEntities = chunk.GetNativeArray(entityType);
                    var chunkControllers = chunk.GetNativeArray(ref controllerType);

                    for (int j = 0; j < chunk.Count; j++)
                    {
                        if (chunkControllers[j].m_Controller == Entity.Null)
                        {
                            //countTrailers++;
                            Entity trailerEntity = chunkEntities[j];

                            ecb.AddComponent<Deleted>(trailerEntity);

                            //Mod.log.Info($"Found matching Entity ID number: {trailerEntity.Index}:{trailerEntity.Version}");
                        }
                    }
                }
            }

            //Mod.log.Info($"Number of orphan trailers: {countTrailers}");
            //Mod.log.Info($"-------------------------------------");

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
