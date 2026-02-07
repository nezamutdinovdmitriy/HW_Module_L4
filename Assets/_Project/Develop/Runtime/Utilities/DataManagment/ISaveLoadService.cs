using System;
using System.Collections;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagment
{
    public interface ISaveLoadService
    {
        public IEnumerator Load<TData>(Action<TData> onLoad) where TData : ISaveData;
        public IEnumerator Save<TData>(TData data) where TData : ISaveData;
        public IEnumerator Remove<TData>() where TData : ISaveData;
        public IEnumerator Exists<TData>(Action<bool> onExistsResult) where TData : ISaveData;
    }
}