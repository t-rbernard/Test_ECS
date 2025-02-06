using ECS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.System
{
    /**
     * System making entities follow a given path and destroys them once the path is completed
     * TODO: Split this in two systems ? With a second system destroying all entities with a DestroyMarker or smth ?
     */
    public partial struct FollowPathAndDestroySystem : ISystem
    {
        private EntityQuery _query;

        public void OnCreate(ref SystemState state)
        {
            Debug.LogWarning("Init FollowPathAndDestroySystem");
            _query = state.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<LocalTransform>(), 
                                                            ComponentType.ReadOnly<FollowPathParameters>(), 
                                                            ComponentType.ReadOnly<PathingComponent>());
            
            state.RequireForUpdate(_query);
        }
        
        public void OnUpdate(ref SystemState state)
        {
            foreach (Entity entity in _query.ToEntityArray(Allocator.Temp))
            {
                var entityPathingComponent = state.EntityManager.GetComponentObject<PathingComponent>(entity);
                
                //Don't continue pathing update if the path is done 
                if(entityPathingComponent.currentGoalStep < entityPathingComponent.pathStruct.path.Length)
                {
                    
                    var entityTransform = state.EntityManager.GetComponentData<LocalTransform>(entity);
                    var entityPathParameters = state.EntityManager.GetComponentData<FollowPathParameters>(entity);

                    float2 currentGoal = entityPathingComponent.pathStruct.path[entityPathingComponent.currentGoalStep];
                    float3 currentFullGoal = new float3(currentGoal.x, entityTransform.Position.y, currentGoal.y); //float2 -> float3
                    float3 distanceToGoal = currentFullGoal - entityTransform.Position;
                    
                    //If total distance to goal < movement speed normalized by delta, we'd go over our goal step in this update
                    float absoluteDistance = math.csum(math.abs(distanceToGoal));
                    if (absoluteDistance < entityPathParameters.moveSpeed * Time.deltaTime)
                    {
                        Debug.Log("Closer to goal than expected movement, snap to goal");
                        state.EntityManager.SetComponentData(entity, entityTransform.WithPosition(currentFullGoal));
                    }
                    else //Normal movement
                    {
                        float3 normalizedMovementVector = math.normalize(distanceToGoal) * entityPathParameters.GetFloat3MoveSpeed() * Time.deltaTime;
                        state.EntityManager.SetComponentData(entity, entityTransform.Translate(normalizedMovementVector));
                    }
                    
                    //If we've achieved the goal step, set next step as goal
                    if (entityTransform.Position.Equals(currentFullGoal))
                    {
                        Debug.Log("Arrived at next goal, incrementing goal step");
                        entityPathingComponent.currentGoalStep++;
                    }
                }
                else //Else destroy the moving entity
                {
                    Debug.Log("Path finished : Destroy entity");
                    state.EntityManager.DestroyEntity(entity);
                }
            }
        }

        public void OnDestroy(ref SystemState state)
        {
            _query.Dispose();
        }
    }
}