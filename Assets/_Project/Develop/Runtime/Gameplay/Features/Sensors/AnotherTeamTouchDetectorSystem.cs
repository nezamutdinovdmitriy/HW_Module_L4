using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Optimization;
using Assets._Project.Develop.Runtime.Utilities.Reactive;


namespace Assets._Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class AnotherTeamTouchDetectorSystem : IInitializableSystem, IUpdatableSystem
    {
        private Buffer<Entity> _contacts;
        private ReactiveVariable<bool> _isTouchAnotherTeam;
        private ReactiveVariable<TeamType> _sourceTeam;


        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactsEntitiesBuffer;
            _isTouchAnotherTeam = entity.IsTouchAnotherTeam;
            _sourceTeam = entity.Team;
        }

        void IUpdatableSystem.OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _contacts.Count; i++)
            {
                Entity contact = _contacts.Items[i];

                if(contact.TryGetTeam(out ReactiveVariable<TeamType> anotherTeam))
                {
                    if(_sourceTeam.Value != anotherTeam.Value)
                    {
                        _isTouchAnotherTeam.Value = true;
                        return;
                    }
                }
            }

            _isTouchAnotherTeam.Value= false;
        }
    }
}