using ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.System
{
    [UpdateAfter(typeof(RandomizeColourSystem))] //Update after to have the right colouring
    partial struct SpawnEntitySystem : ISystem
    {
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
            //Handle car spawn on startup
            foreach ((var entityPrefab, var spawnerTransform, var spawnerColour) in
                     SystemAPI.Query<RefRW<EntityPrefab>, RefRO<LocalTransform>, RefRO<ColourComponent>>()
                              .WithAll<SpawnerMarker, ShouldSpawnOnceMarker>())
            {
                CreateAndUpdateEntity(ref state, entityPrefab, spawnerTransform, spawnerColour);
            }
            
            //Disable the "spawn once" marker to prevent spawning on every subsequent frame
            // Debug.LogWarning("Disable spawn once marker");
            EntityQueryBuilder queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<SpawnerMarker, ShouldSpawnOnceMarker>();
            EntityQuery entityQuery = queryBuilder.Build(state.EntityManager);
            if(!entityQuery.IsEmpty)
                state.EntityManager.SetComponentEnabled<ShouldSpawnOnceMarker>(entityQuery, false);
            entityQuery.Dispose();
            queryBuilder.Dispose();
            // TODO : check optimization of instantiating Builder and Query here. Builder is a ref so it seems fine ?
            
            // TODO: Handle car spawn on button hit
            // foreach ((RefRW<SpawnerReference> spawnerReference, RefRO<SpawnCarShortcut> spawnCarShortcut) in
            //          SystemAPI.Query<RefRW<SpawnerReference>, RefRO<SpawnCarShortcut>>().WithAny<SpawnCarShortcut>())
            // {
            // }
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        /**
         * Creates an entity according to the given prefab then update its components
         */
        private void CreateAndUpdateEntity(ref SystemState state, 
                                           RefRW<EntityPrefab> entityPrefab, 
                                           RefRO<LocalTransform> spawnerTransform,
                                           RefRO<ColourComponent> spawnerColour)
        {
            //TODO: create entity appropriately
            Entity spawnedEntity = state.EntityManager.Instantiate(entityPrefab.ValueRO.prefab);
            state.EntityManager.SetComponentData(spawnedEntity, new LocalTransform
            {
                Position = new float3(spawnerTransform.ValueRO.Position.x, 0.5f, spawnerTransform.ValueRO.Position.z),
                Rotation = spawnerTransform.ValueRO.Rotation,
                Scale = 1
            });
            //Set colour data from the spawner to the entity
            state.EntityManager.SetComponentData<ColourComponent>(spawnedEntity, new ColourComponent { colour = spawnerColour.ValueRO.colour });
            //TODO: do the same for path
            //TODO: check if possible to just reference the spawner and access its components (would allow to link spawner colour/path to the car and prevent duplication)

        }
    }
}
