using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

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
		            spawnRatePerSecond = math.abs(authoring.SpawnTimeoutInSeconds), 
		            timeToNextSpawn = math.abs(authoring.SpawnTimeoutInSeconds)
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