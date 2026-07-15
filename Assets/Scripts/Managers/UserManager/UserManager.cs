using System.Collections.Generic;

namespace Managers
{
    public class UserManager : IUserManager
    {
        private User _currentUser;
        private List<User> _users = new List<User>();
        
        public List<User> Users => _users;
        public User CurrentUser => _currentUser;
        
        public void Init(UserData userData)
        {
            _currentUser = new User(userData);
        }
    }
}