using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagment
{
    public interface IConfigsLoader
    {
        public IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded);
    }
}