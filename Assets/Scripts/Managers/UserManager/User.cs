using System;
using Newtonsoft.Json;

namespace Managers
{
    [Serializable]
    public class User
    {
        private UserData _userData;
        public string UserId => _userData.UserId;

        public UserData UserData => _userData;
        
        public User(UserData userData)
        {
            _userData = userData;
        }
    }
}