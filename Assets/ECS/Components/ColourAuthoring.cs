using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace ECS.Components
{
    class ColourAuthoring : MonoBehaviour
    {
        public Color colour;
        private class ColourAuthoringBaker : Baker<ColourAuthoring>
        {
            public override void Bake(ColourAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ColourComponent { colour = authoring.colour });
                
                AddComponent(entity, new URPMaterialPropertyBaseColor { Value = new float4(authoring.colour.r, authoring.colour.g, authoring.colour.b, authoring.colour.a) });
            }
        }
    
    }
    
    public struct ColourComponent : IComponentData
    {
        public Color colour;
    }
}

