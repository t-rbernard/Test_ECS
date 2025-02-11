using ECS.Components;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace ECS.System
{
    public partial struct EnableCurrentGoalIfNeeded : ISystem
    {
        
        public void OnUpdate(ref SystemState state)
        {
            var bufferLookup = SystemAPI.GetBufferLookup<Float2WaypointsBufferData>();
            foreach ((var currentStep, var spawnerRef, var currentGoal, var entity) 
                     in SystemAPI.Query<RefRW<CurrentPathStep>, RefRO<SpawnerReference>, RefRW<CurrentMovementGoalComponent>>()
                                 .WithDisabled<CurrentMovementGoalComponent>().WithEntityAccess())
            {
                Debug.LogWarning("Try access goal buffer");
                var goalPositionBuffer = bufferLookup[spawnerRef.ValueRO.Spawner];
                //If we have valid buffer data
                if (goalPositionBuffer is { IsCreated: true, IsEmpty: false }
                    && currentStep.ValueRO.CurrentStepIndex < goalPositionBuffer.Length)
                {
                    Debug.LogWarning("Try enable movement goal");
                    state.EntityManager.SetComponentEnabled<CurrentMovementGoalComponent>(entity, true);
                    currentGoal.ValueRW.position = goalPositionBuffer[currentStep.ValueRO.CurrentStepIndex].Value;
                }
            }
        }
    }
}