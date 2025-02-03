using ECS.Components;
using ECS.System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

    [UpdateAfter(typeof(RandomizeColourSystem))]
partial struct UpdateMaterialColour : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<ColourComponent> colourComponent, RefRW<URPMaterialPropertyBaseColor> materialPropertyBaseColor) in
                 SystemAPI.Query<RefRW<ColourComponent>, RefRW<URPMaterialPropertyBaseColor>>().WithAll<SpawnerMarker>())
        {
            float4 colourFromComponent = new float4(colourComponent.ValueRO.colour.r, colourComponent.ValueRO.colour.g,
                colourComponent.ValueRO.colour.b, colourComponent.ValueRO.colour.a);
            //Just in case Unity affects and rerenders stuff even if a colour is changed to itself
            if(materialPropertyBaseColor.ValueRO.Value.Equals(colourFromComponent))
            {
                materialPropertyBaseColor.ValueRW.Value = colourFromComponent;
            }
        }

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
