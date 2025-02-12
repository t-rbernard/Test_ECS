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

    /**
     * A reference to a Spawner entity, used to refer to the spawner's path and colour
     * (could be split to make these systems more generic if needed)
     */
    public struct SpawnerReference : IComponentData
    {
        public Entity Spawner;
    }
}