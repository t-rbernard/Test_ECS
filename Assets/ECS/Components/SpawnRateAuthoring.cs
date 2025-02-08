using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public class SpawnRateAuthoring : MonoBehaviour
    {
		public float spawnRatePerSecond = 1f;
        private class SpawnRateAuthoringBaker : Baker<SpawnRateAuthoring>
        {
            public override void Bake(SpawnRateAuthoring authoring)
            {
	            Entity entity = GetEntity(TransformUsageFlags.None);
	            AddComponent(entity, new SpawnRateComponent { 
		            spawnRatePerSecond = authoring.spawnRatePerSecond, 
		            timeToNextSpawn = authoring.spawnRatePerSecond
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