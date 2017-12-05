namespace GameLibrary.Interfaces
{
    public interface ITile
    {
        string RegionId { get; }
        IPoint Location { get; }
        bool Visited { get; set; }

        void EnteredTile(IPlayer player, params object[] args);
        object[] GetAppropriateParams(params object[] args);
    }
}
