using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public class SpawnerReferenceAuthoring : MonoBehaviour
    {
        private class SpawnerReferenceBaker : Baker<SpawnerReferenceAuthoring>
        {
            public override void Bake(SpawnerReferenceAuthoring authoring)
            {
                Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new SpawnerReference());

            }
        }
    }

    public struct SpawnerReference : IComponentData
    {
        public Entity Spawner;
    }
}