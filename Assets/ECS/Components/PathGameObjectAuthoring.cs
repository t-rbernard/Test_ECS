using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components
{
    public class PathGameObjectAuthoring : MonoBehaviour
    {
        //Don't add goal step to authoring as it's always init to 0
        public GameObject[] waypoints;
        private class PathGameObjectAuthoringBaker : Baker<PathGameObjectAuthoring>
        {
            public override void Bake(PathGameObjectAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                var buffer = AddBuffer<Float2WaypointsBufferData>(entity);
                if(authoring.waypoints != null && authoring.waypoints.Length > 0)
                {
                    foreach (var obj in authoring.waypoints)
                    {
                        if (obj != null)
                            buffer.Add(new Float2WaypointsBufferData(obj.transform));
                    }
                }
                // AddComponentObject(entity, new PathingComponent { currentGoalStep = 0, pathStruct = authoring.path });
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            if(waypoints != null && waypoints.Length > 0)
            {
                Gizmos.DrawLine(new Vector3(transform.position.x, 0f, transform.position.z), waypoints[0].transform.position);
                for (int i = 0; i < waypoints.Length - 1; i++)
                {
                    Gizmos.DrawWireSphere(waypoints[i].transform.position, 0.2f);
                    Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
                }
                Gizmos.DrawWireSphere(waypoints[waypoints.Length - 1].transform.position, 0.2f);
            }
        }
    }

    /**
     * Component used to link entities to a Path GameObject that is editable from the scene and track current step
     */
    // public struct PathingComponent : IComponentData
    // {
    //     DynamicBuffer<float2> _pathBuffer;
    // }

    public struct Float2WaypointsBufferData : IBufferElementData
    {
        public float2 Value;
        public Float2WaypointsBufferData(float2 value) { Value = value; }
        public Float2WaypointsBufferData(Transform transform)
        {
            Value = new float2(transform.position.x, transform.position.z);
        }
    }
}