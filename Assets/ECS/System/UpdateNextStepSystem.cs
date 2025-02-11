using ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace ECS.System
{
    public partial struct UpdateNextStepSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CurrentMovementGoalComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //TODO : try and find a better way to access the buffer than use entity access ?
            var bufferLookup = SystemAPI.GetBufferLookup<Float2WaypointsBufferData>();
            foreach ((var spawnerEntity, var transform, var currentStep, var currentGoal) 
            in SystemAPI.Query<RefRO<SpawnerReference>, RefRO<LocalTransform>, RefRW<CurrentPathStep>, RefRW<CurrentMovementGoalComponent>>())
            {
                var goalPositionBuffer = bufferLookup[spawnerEntity.ValueRO.Spawner];
                //If we have valid buffer data
                if (goalPositionBuffer is { IsCreated: true, IsEmpty: false })
                {
                    //If goal has been attained
                    if (transform.ValueRO.Position.Equals(currentGoal.ValueRO.getAsFloat3(transform.ValueRO.Position.y)))
                    {
                        //Increment current step to either set a new goal
                        currentStep.ValueRW.CurrentStepIndex = currentStep.ValueRO.CurrentStepIndex + 1;   
                        //If we're not at the ultimate step yet, set next goal
                        if(currentStep.ValueRO.CurrentStepIndex < goalPositionBuffer.Length)
                        {
                            currentGoal.ValueRW.position = goalPositionBuffer[currentStep.ValueRO.CurrentStepIndex].Value;
                        }
                    }
                }
            }
        }
    }
}