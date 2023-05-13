namespace BookStoreApi.Service
{
    public interface IService
    {
        public Task<T> GetAsync<T>(string command, object parms);
        public Task<List<T>> GetAll<T>(string command, object parms);
        public Task<int> EditData(string command, object parms);
    }
}
