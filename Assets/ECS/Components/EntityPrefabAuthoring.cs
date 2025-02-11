using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    class EntityPrefabAuthoring : MonoBehaviour
    {
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