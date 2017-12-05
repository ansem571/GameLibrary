namespace GameLibrary.Interfaces
{
    public interface ICharacterMovement
    {
        float MaxWidth { get; }
        float MaxHeight { get; }

        void ResetPlayerToRespawn(IPoint loc, IPoint respawn);
        void PerformMovement(IPoint loc);
    }
}
