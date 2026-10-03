namespace E_CommerceOrderManagementAPI.Services.Interfaces
{
    public interface ICacheRepository
    {
        T? Get<T>(string key);
        void Set<T>(string key, T value, int minutes);
        void Remove(string key);
    }
}
