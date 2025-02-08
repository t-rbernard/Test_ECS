using Unity.Burst;
using Unity.Entities;

namespace ECS.System
{
    public partial struct DestroyPathCompleteEntities : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //TODO: if currentStep > currentRoute.Length - 1 destroy entity, maybe pool into entity command buffer
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}