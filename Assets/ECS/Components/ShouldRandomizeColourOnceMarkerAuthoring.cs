using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    class ShouldRandomizeColourOnceMarkerAuthoring : MonoBehaviour
    {

    }

    class ShouldBeRandomizedTagAuthoringBaker : Baker<ShouldRandomizeColourOnceMarkerAuthoring>
    {
        public override void Bake(ShouldRandomizeColourOnceMarkerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ShouldRandomizeColourOnceMarker());
        }
    }

    public struct ShouldRandomizeColourOnceMarker : IComponentData, IEnableableComponent
    {
    
    }
}