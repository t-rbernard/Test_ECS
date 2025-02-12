using Unity.Entities;
using UnityEngine;

class ColourSetAuthoring : MonoBehaviour
{
    public Color[] colours;
    class ColourSetAuthoringBaker : Baker<ColourSetAuthoring>
    {
        public override void Bake(ColourSetAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.None);
            var buffer = AddBuffer<ColourBufferData>(entity);
            foreach (Color col in authoring.colours)
            {
                buffer.Add(new ColourBufferData(col));
            }
        }
    }
}

public struct ColourBufferData : IBufferElementData
{
    public Color colour;
    public ColourBufferData(Color col) { colour = col; }
}
