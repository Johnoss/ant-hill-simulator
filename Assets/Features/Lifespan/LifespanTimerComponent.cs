using Leopotam.EcsLite;

namespace Features.Lifespan
{
    public struct LifespanTimerComponent
    {
        public int LifespanEntity;
        public EcsWorld World;
    }
}