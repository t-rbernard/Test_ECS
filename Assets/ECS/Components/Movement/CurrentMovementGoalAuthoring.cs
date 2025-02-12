using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components
{
    public class CurrentMovementGoalAuthoring : MonoBehaviour
    {
        public float2 position;
        private class CurrentMovementGoalBaker : Baker<CurrentMovementGoalAuthoring>
        {
            public override void Bake(CurrentMovementGoalAuthoring authoring)
            {
                Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new CurrentMovementGoalComponent(authoring.position));
                SetComponentEnabled<CurrentMovementGoalComponent>(entity, false);
            }
        }
    }

    /**
     * Current float2 goal for this entity 
     */
    public struct CurrentMovementGoalComponent : IComponentData, IEnableableComponent
    {
        public float2 position;

        public CurrentMovementGoalComponent(float2 position) { this.position = position; }
        [Pure] public float3 getAsFloat3() => new (position.x, 0, position.y);
        [Pure] public float3 getAsFloat3(float yValue) => new (position.x, yValue, position.y);
    }
}