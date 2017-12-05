namespace GameLibrary.Interfaces
{
    public interface IGameManager
    {
        IMap Map { get; }
        IPlayer Player { get; }
        IBattleManager BattleManager { get; }
        void Play();
        void Save();
        void Quit();
    }
}
