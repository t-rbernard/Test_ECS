using ECS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECS.System
{
    [BurstCompile]
    partial struct SpawnEntitySystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnRateComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((var spawnRate, var prefab, var transform, var entity) 
                     in SystemAPI.Query<RefRO<SpawnRateComponent>, RefRO<EntityPrefab>, RefRO<LocalTransform>>().WithEntityAccess())
            {
                if(spawnRate.ValueRO.timeToNextSpawn == 0)
                {
                    CreateAndUpdateEntity(ref state, entity, prefab, transform);
                }
            }
        }

        /**
         * Creates an entity according to the given prefab then update its components
         */
        [BurstCompile]
        public void CreateAndUpdateEntity(ref SystemState state,
                                          Entity spawnerEntity,
                                          RefRO<EntityPrefab> entityPrefab, 
                                          RefRO<LocalTransform> spawnerTransform)
        {
            Entity spawnedEntity = state.EntityManager.Instantiate(entityPrefab.ValueRO.prefab);
            state.EntityManager.SetComponentData(spawnedEntity, new LocalTransform
            {
                Position = new float3(spawnerTransform.ValueRO.Position.x, 0.5f, spawnerTransform.ValueRO.Position.z),
                Rotation = spawnerTransform.ValueRO.Rotation,
                Scale = 1
            });

            Color randomColor;
            var colourChoicesBuffer = state.EntityManager.GetBuffer<ColourBufferData>(spawnerEntity);
            if (colourChoicesBuffer is { IsCreated: true, Length: > 0 }) // Get random colour from buffer if possible
            {
                randomColor = colourChoicesBuffer[Random.Range(0, colourChoicesBuffer.Length)].colour;
            }
            else // Otherwise get a random visible colour
            {
                randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }

            state.EntityManager.SetComponentData(spawnedEntity, new URPMaterialPropertyBaseColor { Value = new float4(randomColor.r, randomColor.g, randomColor.b, randomColor.a) });
            state.EntityManager.SetComponentData(spawnedEntity, new SpawnerReference { Spawner = spawnerEntity });
        }
    }
}
