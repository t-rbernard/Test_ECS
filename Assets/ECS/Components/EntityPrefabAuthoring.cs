using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    class EntityPrefabAuthoring : MonoBehaviour
    {
        // Feels slightly wrong to have GameObject here and not Entity
        // But Entity isn't serializable and it's what I found here :
        // https://dev.to/rk042/a-comprehensive-guide-to-generating-entity-prefabs-at-runtime-in-unity-ecs-16o4
        // Some tutorials I found did use Entity, but were much older and didn't work due to being out of date
        public GameObject prefab;
        
        private class EntityPrefabAuthoringBaker : Baker<EntityPrefabAuthoring>
        {
            public override void Bake(EntityPrefabAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EntityPrefab { prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic) });
            }
        }
    }

    /**
     * Component containing an Entity prefab to be used as reference by Systems (namely the Spawner System)
     */
    public struct EntityPrefab : IComponentData
    {
        public Entity prefab;
    }
}