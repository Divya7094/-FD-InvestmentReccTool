using WisVestAPI.Models;
using System.Text.Json;
 
namespace WisVestAPI.Services
{
    public class UserService
    {
        private readonly string _filePath = "users.json";
 
        public List<User> GetAllUsers()
        {
            if (!File.Exists(_filePath))
            {
                return new List<User>();
            }
 
            var jsonData = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(jsonData) ?? new List<User>();
        }
 
        public void SaveAllUsers(List<User> users)
        {
            var jsonData = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
 
        public void AddUser(User user)
        {
            var users = GetAllUsers();
            users.Add(user);
            SaveAllUsers(users);
        }
 
        public bool UserExists(string email)
        {
            var users = GetAllUsers();
return users.Any(u => u.Email == email);
        }
    }
}