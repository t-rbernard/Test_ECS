using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace ECS.Components
{
    public class SpawnRateAuthoring : MonoBehaviour
    {
		[Tooltip("Seconds before a new entity spawns")] 
		public float SpawnTimeoutInSeconds = 1f;
        private class SpawnRateAuthoringBaker : Baker<SpawnRateAuthoring>
        {
            public override void Bake(SpawnRateAuthoring authoring)
            {
	            Entity entity = GetEntity(TransformUsageFlags.None);
	            AddComponent(entity, new SpawnRateComponent { 
		            spawnRatePerSecond = authoring.SpawnTimeoutInSeconds, 
		            timeToNextSpawn = authoring.SpawnTimeoutInSeconds
	            });
            }
        }
    }

	public struct SpawnRateComponent : IComponentData {
		public float spawnRatePerSecond;
		public float timeToNextSpawn;
		public void resetTimeToNextSpawn() { timeToNextSpawn = spawnRatePerSecond; }
	}
}