using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagment
{
    public class ConfigsProviderService
    {
        private readonly Dictionary<Type, object> _configs = new();

        private readonly IConfigsLoader[] _configsLoaders;

        public ConfigsProviderService(params IConfigsLoader[] configsLoaders)
        {
            _configsLoaders = configsLoaders;
        }

        public T GetConfig<T>() where T : class
        {
            if (_configs.ContainsKey(typeof(T)) == false)
                throw new InvalidOperationException($"Not found config by {typeof(T)}");

            return (T)_configs[typeof(T)];
        }

        public IEnumerator LoadAsync()
        {
            _configs.Clear();

            foreach (IConfigsLoader configLoader in _configsLoaders)
                yield return configLoader.LoadAsync(loadedConfigs => _configs.AddRange(loadedConfigs));
        }
    }
}