using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components
{
    public class FollowPathParametersAuthoring : MonoBehaviour
    {
        [Tooltip("Move speed per second")] public float moveSpeed;

        private class FollowPathParametersBaker : Baker<FollowPathParametersAuthoring>
        {
            public override void Bake(FollowPathParametersAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new FollowPathParameters { moveSpeed = authoring.moveSpeed });
            }
        }
    }

    public struct FollowPathParameters : IComponentData
    {
        public float moveSpeed;

        /**
         * Produces a float3 from the movement speed allowing only x,z movement
         */
        public float3 GetFloat3MoveSpeed()
        {
            return new float3(moveSpeed, 0, moveSpeed);
        }
    }
}