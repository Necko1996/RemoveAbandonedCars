using Game;
using Game.Common;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;

namespace RemoveAbandonedCars.Systems
{
    public partial class TrailerRemovalSystem : GameSystemBase
    {
        protected override void OnCreate()
        {
            base.OnCreate();
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 512;
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

            Entities
                .WithAll<Controller>()
                .WithAny<CarTrailerLane>()
                .WithNone<Deleted>()
                .ForEach((Entity entity, in Controller controller) =>
                {
                    if (controller.m_Controller == Entity.Null)
                    {
                        ecb.AddComponent<Deleted>(entity);
                    }
                })
                .WithName("DeleteAbandonedTrailers")
                .Schedule();

            this.CompleteDependency();

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
