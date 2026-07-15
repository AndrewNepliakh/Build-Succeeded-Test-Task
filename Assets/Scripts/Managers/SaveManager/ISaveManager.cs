namespace Managers
{
    public interface ISaveManager
    {
        void Save<T>(T saveData) where T : SaveData;
        T Load<T>() where T : SaveData;
    }
}