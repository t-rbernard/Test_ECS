using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public class PathingComponentAuthoring : MonoBehaviour
    {
        //Don't add goal step to authoring as it's always init to 0
        public PathStruct path;
        private class PathingComponentAuthoringBaker : Baker<PathingComponentAuthoring>
        {
            public override void Bake(PathingComponentAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponentObject(entity, new PathingComponent { currentGoalStep = 0, pathStruct = authoring.path });
            }
        }
    }

    /**
     * Component used to link entities to a Path GameObject that is editable from the scene and track current step
     */
    public class PathingComponent : IComponentData
    {
        public int currentGoalStep;
        public PathStruct pathStruct;
    }
}