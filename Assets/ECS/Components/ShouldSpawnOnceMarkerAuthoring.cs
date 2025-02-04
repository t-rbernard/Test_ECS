using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    class ShouldSpawnOnceMarkerAuthoring : MonoBehaviour
    {

    }

    class ShouldSpawnOnceMarkerAuthoringBaker : Baker<ShouldSpawnOnceMarkerAuthoring>
    {
        public override void Bake(ShouldSpawnOnceMarkerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ShouldSpawnOnceMarker());
        }
    }

    /**
     * A simple marker used to spawn a car on first Entity update then disable the marker
     */
    public struct ShouldSpawnOnceMarker : IComponentData, IEnableableComponent
    {
    
    }
}