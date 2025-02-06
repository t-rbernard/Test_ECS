using System;
using ECS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.System
{
    [UpdateAfter(typeof(RandomizeColourSystem))] //Update after to have the right colouring
    partial struct SpawnEntitySystem : ISystem
    {
        private EntityQuery _spawnerQuery;
        // private EntityQuery _spawnOnceSpawnerQuery;
        
        public void OnCreate(ref SystemState state)
        {
            //Query used to look for spawners containing all the data we require to create a coloured entity moving over a given path
            _spawnerQuery = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<EntityPrefab>(), 
                                                                  ComponentType.ReadOnly<LocalTransform>(),
                                                                  ComponentType.ReadOnly<ColourComponent>(), 
                                                                  ComponentType.ReadOnly<PathingComponent>(), 
                                                                  ComponentType.ReadOnly<SpawnerMarker>(), 
                                                                  ComponentType.ReadWrite<ShouldSpawnOnceMarker>());

            state.RequireForUpdate(_spawnerQuery);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            _spawnerQuery.Dispose();
        }


        public void OnUpdate(ref SystemState state)
        {
            //Handle car spawn on startup
            foreach (var spawnerEntity in _spawnerQuery.ToEntityArray(Allocator.Temp))
            {
                var entityPrefab = state.EntityManager.GetComponentData<EntityPrefab>(spawnerEntity);
                var spawnerTransform = state.EntityManager.GetComponentData<LocalTransform>(spawnerEntity);
                var spawnerColour = state.EntityManager.GetComponentData<ColourComponent>(spawnerEntity);
                var spawnerPath = state.EntityManager.GetComponentData<PathingComponent>(spawnerEntity);
            
                CreateAndUpdateEntity(ref state, entityPrefab, spawnerPath, spawnerTransform, spawnerColour);
            }
            
            if(!_spawnerQuery.IsEmpty)
                state.EntityManager.SetComponentEnabled<ShouldSpawnOnceMarker>(_spawnerQuery, false);
            
        }

        /**
         * Creates an entity according to the given prefab then update its components
         */
        private void CreateAndUpdateEntity(ref SystemState state, 
                                           EntityPrefab entityPrefab, 
                                           PathingComponent spawnerPath,
                                           LocalTransform spawnerTransform,
                                           ColourComponent spawnerColour)
        {
            //TODO: create entity appropriately
            Entity spawnedEntity = state.EntityManager.Instantiate(entityPrefab.prefab);
            state.EntityManager.SetComponentData(spawnedEntity, new LocalTransform
            {
                Position = new float3(spawnerTransform.Position.x, 0.5f, spawnerTransform.Position.z),
                Rotation = spawnerTransform.Rotation,
                Scale = 1
            });
            //Set colour data from the spawner to the entity
            state.EntityManager.SetComponentData<ColourComponent>(spawnedEntity, new ColourComponent { colour = spawnerColour.colour });
            state.EntityManager.SetComponentData<PathingComponent>(spawnedEntity, spawnerPath);
            //TODO: Try to just reference the spawner and access its components
            //(would allow to link spawner colour/path to the car and prevent duplication)
        }
    }
}
