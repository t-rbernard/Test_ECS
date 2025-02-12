using ECS.Components;
using Unity.Burst;
using Unity.Entities;

namespace ECS.System
{
    /**
     * Currently only lowers spawner timers, but with a bit of component refactors can be a generic timer update
     */
    public partial struct UpdateTimers : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var timer in SystemAPI.Query<RefRW<SpawnRateComponent>>())
            {
                if(timer.ValueRO.timeToNextSpawn == 0f)
                    timer.ValueRW.resetTimeToNextSpawn();
                if(timer.ValueRO.timeToNextSpawn > 0f)
                    timer.ValueRW.timeToNextSpawn -= SystemAPI.Time.DeltaTime;
                if (timer.ValueRO.timeToNextSpawn < 0f)
                    timer.ValueRW.timeToNextSpawn = 0f;
            }
        }
    }
}