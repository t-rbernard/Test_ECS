using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    class SpawnerMarkerAuthoring : MonoBehaviour
    {

    }

    class SpawnerMarkerAuthoringBaker : Baker<SpawnerMarkerAuthoring>
    {
        public override void Bake(SpawnerMarkerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SpawnerMarker());
        }
    }

    /**
     * Simple marker differentiating spawners from other entities with common components
     */
    public struct SpawnerMarker : IComponentData
    {

    }
}
