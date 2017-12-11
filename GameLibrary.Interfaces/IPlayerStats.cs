namespace GameLibrary.Interfaces
{
    public interface IPlayerStats : IStats
    {
        int Deaths { get; set; }
        int CurrentExp { get; set; }
        int MaxExp { get; set; }

        void LevelUp();
    }
}
