using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using System.Linq.Expressions;

namespace ChillToeic.Service
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return  _userRepository.GetAll();
        }

        public User GetUserById(int id)
        {
            return _userRepository.Find(m =>m.Id == id).FirstOrDefault();
        }
        public User FindUserByEmail(string email)
        {
            return _userRepository.Find(m => m.Email == email).FirstOrDefault();
        }
        public User FindUserByUserName(string username)
        {
            return _userRepository.Find(m => m.UserName == username).FirstOrDefault();
        }

        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

        public void UpdateUser(User user)
        { 
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }
    }
}
