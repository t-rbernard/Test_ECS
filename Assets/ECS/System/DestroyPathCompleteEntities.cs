using ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.System
{
    public partial struct DestroyPathCompleteEntities : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var bufferLookup = SystemAPI.GetBufferLookup<Float2WaypointsBufferData>();
            foreach ((var spawnerEntity, var currentStep, var entity)
                     in SystemAPI.Query<RefRO<SpawnerReference>, RefRW<CurrentPathStep>>()
                                 .WithAll<DestroyOnPathDoneMarker>().WithEntityAccess())
            {
                var goalPositionBuffer = bufferLookup[spawnerEntity.ValueRO.Spawner];
                //If we have valid buffer data and are past the end of the goal buffer
                if (goalPositionBuffer is { IsCreated: true, IsEmpty: false } 
                    && currentStep.ValueRO.CurrentStepIndex >= goalPositionBuffer.Length)
                {
                    ecb.DestroyEntity(entity);
                }
            }
            ecb.Playback(state.EntityManager);
        }
    }
}