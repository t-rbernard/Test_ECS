using ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Random = UnityEngine.Random;

namespace ECS.System
{
    /**
     * Affects a new random colour to colour components with a spawner marker.
     * Right now it randomizes on update but I'd like to make it randomize colour on entity creation only (onCreate happens before entity creation)
     */
    partial struct RandomizeColourSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //On System update, replace every spawner's colour with a random colour that'll be inherited by spawned cars
            foreach ((RefRW<ColourComponent> colourComponent, RefRW<URPMaterialPropertyBaseColor> materialPropertyBaseColor) in
                     //WithAny here to eventually have other colour randomization markers (randomize every X frames or smth)
                     SystemAPI.Query<RefRW<ColourComponent>, RefRW<URPMaterialPropertyBaseColor>>().WithAny<ShouldRandomizeColourOnceMarker>())
            {
                colourComponent.ValueRW.colour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);;
                materialPropertyBaseColor.ValueRW.Value = new float4(colourComponent.ValueRO.colour.r, colourComponent.ValueRO.colour.g, colourComponent.ValueRO.colour.b, colourComponent.ValueRO.colour.a);
            }
            
            //Disable the "randomize once" marker to avoid randomizing if not necessary
            var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<ShouldRandomizeColourOnceMarker>();
            var entityQuery = state.EntityManager.CreateEntityQuery(queryBuilder);
            state.EntityManager.SetComponentEnabled<ShouldRandomizeColourOnceMarker>(entityQuery, false);
            queryBuilder.Dispose();
            entityQuery.Dispose();
        }
    }
}
