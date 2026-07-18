namespace Managers
{
    public interface ILevelManager
    {
        public Level CurrentLevel { get; }
        void Init();
    }
}