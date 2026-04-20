using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class InstantShootingDirectionArgs
    {
        private readonly List<InstantShotDirectionArgs> _args;

        public InstantShootingDirectionArgs(params InstantShotDirectionArgs[] args)
        {
            _args = new List<InstantShotDirectionArgs>(args);
        }

        public IReadOnlyList<InstantShotDirectionArgs> Args => _args;

        public void Add(InstantShotDirectionArgs shotInDirectionArgs)
        {
            InstantShotDirectionArgs arg = _args.FirstOrDefault(arg => arg.Angel == shotInDirectionArgs.Angel);

            if(arg != null)
            {
                arg.ProjectileCounts += shotInDirectionArgs.ProjectileCounts;
                return;
            }

            _args.Add(shotInDirectionArgs);
        }

        public void Remove(InstantShotDirectionArgs shotInDirectionArgs)
        {
            InstantShotDirectionArgs arg = _args.FirstOrDefault(arg => arg.Angel == shotInDirectionArgs.Angel);

            if (arg != null)
            {
                arg.ProjectileCounts -= shotInDirectionArgs.ProjectileCounts;
               
                if(arg.ProjectileCounts <= 0)
                    _args.Remove(shotInDirectionArgs);
            }
        }
    }
}