using ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.System
{
    public partial struct MoveToGoalSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CurrentMovementGoalComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((var parameters, var currentGoal, var transform) 
                     in SystemAPI.Query<RefRO<FollowPathParameters>, RefRO<CurrentMovementGoalComponent>, RefRW<LocalTransform>>())
            {
                //Do the thing
                float3 currentGoalPosition = currentGoal.ValueRO.getAsFloat3(transform.ValueRO.Position.y);
                float3 distanceToGoal = currentGoalPosition - transform.ValueRO.Position;
                float yAngle = math.atan(distanceToGoal.z/distanceToGoal.x);
                // Quaternion.FromToRotation(transform.ValueRO)

                //If total distance to goal < movement speed normalized by delta, we'd go over our goal step in this update
                float absoluteDistance = math.csum(math.abs(distanceToGoal));
                if (absoluteDistance < parameters.ValueRO.moveSpeed * Time.deltaTime)
                {
                    Debug.Log("Closer to goal than expected movement, snap to goal");
                    transform.ValueRW = transform.ValueRO.WithPosition(currentGoalPosition).WithRotation(Quaternion.Euler(0, yAngle, 0));
                }
                else //Normal movement
                {
                    float3 normalizedMovementVector = math.normalize(distanceToGoal) * parameters.ValueRO.GetFloat3MoveSpeed() * SystemAPI.Time.DeltaTime;
                    transform.ValueRW = transform.ValueRO.Translate(normalizedMovementVector)/*.WithRotation(angle)*/;
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
    
    // readonly partial struct MoveTowardsGoalAspect : IAspect
    // {
    //     public readonly RefRO<FollowPathParameters> followPathParameters;
    //     public readonly RefRO<CurrentMovementGoalComponent> currentMovementGoal;
    //     public readonly RefRW<LocalTransform> localTransform;
    // }
}