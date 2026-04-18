using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature
{
    public class DropLootSystem : IInitializableSystem, IUpdatableSystem
    {
        private readonly DropLootService _dropLootService;
        private ReactiveVariable<bool> _isDropped;

        private Entity _entity;
        private ICompositeCondition _dropLootCondition;

        public DropLootSystem(DropLootService dropLootService)
            => _dropLootService = dropLootService;

        public void OnInit(Entity entity)
        {
            _isDropped = entity.LootIsDropped;
            _entity = entity;
            _dropLootCondition = entity.CanDropLoot;
        }

        public void OnUpdate(float deltaTime)
        {
            if(_dropLootCondition.Evaluate())
            {
                DropLoot();
                _isDropped.Value = true;
            }
        }

        private void DropLoot() => _dropLootService.DropLootFor(_entity);
    }
}