using System.Collections.Generic;

namespace Managers
{
    public interface IUserManager
    {
        List<User> Users { get; }
        User CurrentUser { get; }
        void Init(UserData userData);
    }
}