using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    class ShouldRandomizeColourOnceMarkerAuthoring : MonoBehaviour
    {

    }

    class ShouldRandomizeColourOnceMarkerAuthoringBaker : Baker<ShouldRandomizeColourOnceMarkerAuthoring>
    {
        public override void Bake(ShouldRandomizeColourOnceMarkerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ShouldRandomizeColourOnceMarker());
        }
    }

    /**
     * Simple marker used to randomize colour once then be disabled
     */
    public struct ShouldRandomizeColourOnceMarker : IComponentData, IEnableableComponent
    {
    
    }
}