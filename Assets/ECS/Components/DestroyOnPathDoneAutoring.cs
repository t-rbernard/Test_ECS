using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public class DestroyOnPathDoneAutoring : MonoBehaviour
    {
        private class DestroyOnPathDoneAutoringBaker : Baker<DestroyOnPathDoneAutoring>
        {
            public override void Bake(DestroyOnPathDoneAutoring authoring)
            {
                Entity entity = GetEntity(authoring, TransformUsageFlags.None);
                AddComponent(entity, new DestroyOnPathDoneMarker());
            }
        }
    }

    /**
     * A simple marker used to signal the destruction system this entity should get destroyed after its path is done
     */
    public struct DestroyOnPathDoneMarker : IComponentData
    {
    }
}