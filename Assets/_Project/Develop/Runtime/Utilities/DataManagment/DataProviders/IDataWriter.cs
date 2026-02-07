namespace Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders
{
    public interface IDataWriter<TData> where TData : ISaveData
    {
        public void WriteTo(TData data);
    }
}