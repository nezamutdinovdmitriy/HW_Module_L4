using System;
using System.Collections;

namespace Assets._Project.Develop.Runtime.Utilities.DataManagment.DataRepository
{
    public interface IDataRepository
    {
        public IEnumerator Read(string key, Action<string> onRead);
        public IEnumerator Write(string key, string serializedData);
        public IEnumerator Remove(string key);
        public IEnumerator Exists(string key, Action<bool> onExistsResult);
    }
}