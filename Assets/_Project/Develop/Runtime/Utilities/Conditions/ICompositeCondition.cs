using System;

namespace Assets._Project.Develop.Runtime.Utilities.Conditions
{
    public interface ICompositeCondition : ICondition
    {
        public ICompositeCondition Add(ICondition condition, int order = 0, Func<bool, bool, bool> logicOperation = null);
        public ICompositeCondition Remove(ICondition condition);
    }
}