using System;

namespace Managers
{
    [Serializable]
    public class UserData
    {
        public int CurrentLevel = 1;

        public bool IsFirstStart = true;

        public bool IsFirstPlay = true;

        public string UserId { get; private set; }

        public UserData()
        {
            UserId = Guid.NewGuid().ToString();
        }
    }
}