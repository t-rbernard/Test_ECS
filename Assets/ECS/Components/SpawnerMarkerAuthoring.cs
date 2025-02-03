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

    struct SpawnerMarker : IComponentData
    {

    }
}
