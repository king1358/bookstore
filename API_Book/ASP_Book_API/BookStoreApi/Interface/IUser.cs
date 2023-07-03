using BookStoreApi.Model;

namespace BookStoreApi.Interface
{
    public interface IUser
    {
        Task<bool> Verify(int id_user, string token);
        Task<User> FindUser(string username);
        Task<int> CountUser();
        Task<bool> CreateUser(InfoRegister userInfo, int id, string salt, string passHash,DateTime created_time);
        Task<int> InsertAddressUser(int id_user, int id_province, int id_district, string id_ward, string housenumber);
        Task<bool> SetToken(string username,string token);
        Task<List<Province>> GetProvinceList();
        Task<List<District>> GetDistrictList(int id_province);
        Task<List<Ward>> GetWardList(string id_district);
        Task<List<Address>> GetAddressList(int id_user);
        Task<string> GetFullInfoAddress(Address address);
    }
}
