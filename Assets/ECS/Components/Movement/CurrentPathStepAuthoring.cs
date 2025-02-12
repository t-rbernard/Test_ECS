using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public class CurrentPathStepAuthoring : MonoBehaviour
    {
        private class CurrentPathStepBaker : Baker<CurrentPathStepAuthoring>
        {
            public override void Bake(CurrentPathStepAuthoring authoring)
            {
                Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new CurrentPathStep { CurrentStepIndex = 0 });
            }
        }
    }
    
    /**
     * Current step in the path the entity is following, from 0 to Path.Length - 1
     */
    public struct CurrentPathStep : IComponentData
    {
        public int CurrentStepIndex;
    }
        
}