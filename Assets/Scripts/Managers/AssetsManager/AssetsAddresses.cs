using System;
using System.Collections.Generic;

namespace Managers
{
    public static class AssetsAddresses
    {
        private static readonly Dictionary<Type, string> Addresses = new()
        {
            // { typeof(MainMenu), "UI/MainMenu" },
            // { typeof(SettingsPopup), "UI/SettingsPopup" },
            // { typeof(GameHUD), "UI/GameHUD" },
        };

        public static string Get<T>()
        {
            if (Addresses.TryGetValue(typeof(T), out var address))
                return address;

            throw new Exception($"Address for {typeof(T).Name} is not registered.");
        }
    }
}