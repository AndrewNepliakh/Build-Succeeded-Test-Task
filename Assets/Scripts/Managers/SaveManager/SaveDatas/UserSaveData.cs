using System;

namespace Managers
{
    [Serializable]
    public class UserSaveData : SaveData
    {
        public UserData UserData = new();
    }
}