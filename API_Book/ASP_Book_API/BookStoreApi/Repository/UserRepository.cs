using BookStoreApi.Model;
using BookStoreApi.Interface;
using BookStoreApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Repository
{
    public class UserRepository : IUser
    {
        private readonly IService _service;
        public UserRepository(IService service)
        {
            _service = service;
        }
        public async Task<bool> Verify(int id_user, string token)
        {
            string username = await _service.GetAsync<string>("SELECT USERNAME FROM [USER] WHERE ID = @ID_USER AND TOKEN = @TOKEN",
                new { id_user = id_user, token = token });
            if (username == null) return false;
            return true;
        }
        public async Task<User> FindUser(string username)
        {
            return await _service.GetAsync<User>("SELECT * FROM [USER] WHERE USERNAME = @USERNAME", new { username = username });
        }
        public async Task<int> CountUser()
        {
            return await _service.GetAsync<int>("SELECT COUNT(*) FROM [USER]", new { });
        }
        public async Task<bool> CreateUser(InfoRegister userInfo, int id, string salt, string passHash, DateTime created_time)
        {
            int n = await _service.EditData("INSERT INTO [USER] VALUES(@ID,@USERNAME,@PASSWORD,@FULLNAME," +
                "@EMAIL,@PHONE,@BIRTHDATE,@CREATED_TIME,NULL,@SALT)",
                new
                {
                    id = id,
                    username = userInfo.username,
                    password = passHash,
                    fullname = userInfo.fullname,
                    email = userInfo.email,
                    phone = userInfo.phone,
                    birthDate = userInfo.birthDate,
                    created_time = created_time,
                    salt = salt
                });
            if (n == 1)
            {
                int m = await _service.EditData("INSERT INTO ADDRESS_USER VALUES(@ID_USER,1,@ID_PROVINCE,@ID_DISTRICT,@ID_WARD,@HOUSENUMBER,1)",
                    new
                    {
                        id_user = id,
                        id_province = userInfo.id_province,
                        id_district = userInfo.id_district,
                        id_ward = userInfo.id_ward,
                        housenumber = userInfo.housenumber
                    });
                if (m == 1) return true;
            }
            return false;
        }

        public async Task<int> InsertAddressUser(int id_user, int id_province, int id_district, string id_ward, string housenumber)
        {
            int serial = await _service.GetAsync<int>("SELECT COUNT(*) FROM ADDRESS_USER WHERE ID_USER = @ID_USER", new { id_user = id_user });
            serial += 1;
            int m = await _service.EditData("INSERT INTO ADDRESS_USER VALUES(@ID_USER,@SERIAL,@ID_PROVINCE,@ID_DISTRICT,@ID_WARD,@HOUSENUMBER,0)",
                    new
                    {
                        id_user = id_user,
                        serial = serial,
                        id_province = id_province,
                        id_district = id_district,
                        id_ward = id_ward,
                        housenumber =  housenumber
                    });
            return serial;
        }


        public async Task<bool> SetToken(string username, string token)
        {
            int n = await _service.EditData("UPDATE [USER] SET TOKEN = @TOKEN WHERE USERNAME = @USERNAME",
                new { username = username, token = token });
            if (n == 1) return true;
            return false;
        }

        public async Task<List<Province>> GetProvinceList()
        {
            return await _service.GetAll<Province>("SELECT * FROM PROVINCE", new { });
        }
        public async Task<List<District>> GetDistrictList(int id_province)
        {
            return await _service.GetAll<District>("SELECT * FROM DISTRICT WHERE ID_PROVINCE = @ID_PROVINCE", new { id_province = id_province });
        }
        public async Task<List<Ward>> GetWardList(string id_district)
        {
            return await _service.GetAll<Ward>("SELECT * FROM WARD WHERE ID_DISTRICT = @ID_DISTRICT", new { id_district = id_district });
        }
        public async Task<List<Address>> GetAddressList(int id_user)
        {

            List<Address> list = await _service.GetAll<Address>("SELECT * FROM ADDRESS_USER WHERE ID_USER = @ID_USER",
                new { id_user = id_user });
            foreach (Address address in list)
            {
                address.fullInfo = await GetFullInfoAddress(address);
            }
            return list;
        }

        public async Task<string> GetFullInfoAddress(Address address)
        {
            string provinceName = await _service.GetAsync<string>("SELECT NAME FROM PROVINCE WHERE ID_PROVINCE = @ID_PROVINCE",
                new { id_province = address.id_province });
            string districtName = await _service.GetAsync<string>("SELECT NAME FROM DISTRICT WHERE ID_DISTRICT = @ID_DISTRICT",
            new { id_district = address.id_district });
            string wardName = await _service.GetAsync<string>("SELECT NAME FROM WARD WHERE ID_WARD = @ID_WARD",
            new { id_ward = address.id_ward });
            string fullInfo = $"{address.house_number}, {wardName}, {districtName}, {provinceName}";
            return fullInfo;
        }
    }
}
