using Colossal.Serialization.Entities;
using Game;
using Game.Net;
using Game.Prefabs;
using Unity.Entities;

namespace RemoveAbandonedCars.Systems
{
    public partial class IncreaseParkingCapacitySystem : GameSystemBase
    {
        private bool m_HasModifiedPrefabs = false;

        public struct OriginalParkingSlotInterval : IComponentData
        {
            public float m_OriginalInterval;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            Enabled = false;
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);

            if (mode.IsGame() && !m_HasModifiedPrefabs)
            {
                Enabled = true;
            }
        }

        protected override void OnUpdate()
        {
            Mod.log.Info($"Increasing car parking capacity (one-time execution)...");

            float divisor = Mod.m_Setting.SlotDivisorSlider;

            var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

            Entities
                .WithNone<OriginalParkingSlotInterval>()
                .ForEach((Entity entity, in ParkingLaneData laneData) =>
                {
                    ecb.AddComponent(entity, new OriginalParkingSlotInterval
                    {
                        m_OriginalInterval = laneData.m_SlotInterval
                    });
                })
                .WithName("BackupOriginalParkingIntervals")
                .Run();

            ecb.Playback(EntityManager);
            ecb.Dispose();

            Entities
             .ForEach((ref ParkingLaneData laneData, in OriginalParkingSlotInterval original) =>
             {
                 if (laneData.m_RoadTypes != RoadTypes.Car)
                 {
                     return;
                 }

                 if (laneData.m_SlotInterval > 0f)
                 {
                     laneData.m_SlotInterval = original.m_OriginalInterval / divisor;
                 }
             })
             .WithName("UpdateParkingLanePrefabs")
             .Schedule();

            this.CompleteDependency();

            m_HasModifiedPrefabs = true;
            Enabled = false;
        }
    }
}