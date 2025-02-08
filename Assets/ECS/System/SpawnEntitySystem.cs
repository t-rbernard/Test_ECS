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
    [UpdateAfter(typeof(UpdateTimers))]
    partial struct SpawnEntitySystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnRateComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var waypointsBufferLookup = SystemAPI.GetBufferLookup<Float2WaypointsBufferData>();
            foreach ((var spawnRate, var prefab, var transform, var entity) 
                     in SystemAPI.Query<RefRO<SpawnRateComponent>, RefRO<EntityPrefab>, RefRO<LocalTransform>>().WithEntityAccess())
            {
                if(spawnRate.ValueRO.timeToNextSpawn == 0)
                {
                    CreateAndUpdateEntity(ref state, waypointsBufferLookup[entity], prefab, transform);
                }
            }
        }

        /**
         * Creates an entity according to the given prefab then update its components
         */
        [BurstCompile]
        public void CreateAndUpdateEntity(ref SystemState state,
                                          DynamicBuffer<Float2WaypointsBufferData> spawnerBuffer,
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
            Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            //TODO : Use spawner's random colour set
            state.EntityManager.SetComponentData(spawnedEntity, new URPMaterialPropertyBaseColor { Value = new float4(randomColor.r, randomColor.g, randomColor.b, randomColor.a) });
            if(spawnerBuffer.IsCreated && !spawnerBuffer.IsEmpty)
            {
                var buffer = state.EntityManager.AddBuffer<Float2WaypointsBufferData>(spawnedEntity);
                foreach (var bufferData in spawnerBuffer)
                {
                    buffer.Add(new Float2WaypointsBufferData(bufferData.Value)); 
                    //TODO: Maybe do that via a Baker ? cf PathGameObjectAuthoring
                }
            }
            //TODO: Try to just reference the spawner and access its components ?
        }
    }
}
