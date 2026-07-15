using System;
using Zenject;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;

namespace Managers
{
    public class SaveManager : ISaveManager
    {
        [Inject] private DiContainer _container;

        private readonly Dictionary<Type, SaveData> _cached = new();

        private static readonly JsonSerializerSettings Settings = new()
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() }
        };

        private string GetPathFor<T>() where T : SaveData
        {
            var name = typeof(T).Name + ".json";
            return Path.Combine(Application.persistentDataPath, name);
        }

        public void Save<T>(T saveData) where T : SaveData
        {
            var path = GetPathFor<T>();
            var json = JsonConvert.SerializeObject(saveData, Settings);
            File.WriteAllText(path, json);
        }

        public T Load<T>() where T : SaveData
        {
            if (_cached.TryGetValue(typeof(T), out var cached))
                return (T)cached;

            var path = GetPathFor<T>();

            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);

                var data = JsonConvert.DeserializeObject<T>(json, Settings);
                _container.Inject(data);
                _cached[typeof(T)] = data;
                return data;
            }
            
            return CreateDefault<T>();
        }

        public void SaveDefaultData<T>() where T : SaveData, new()
        {
            var data = CreateDefault<T>();
            Save(data);
        }

        private T CreateDefault<T>() where T : SaveData
        {
            var data = (T)Activator.CreateInstance(typeof(T));
            _container.Inject(data);
            _cached[typeof(T)] = data;
            return data;
        }
    }
}
