namespace GameLibrary.Interfaces
{
    public interface IEnemy
    {
        bool IsBoss { get; }
        string Name { get; }
        IStats Stats { get; }
        ICombatActions CombatActions { get; }

        void Attack(bool useMagic, IPlayer player);
        void ChargeMana(IPlayer player);

        bool IsDefeated(IPlayer player);
    }
}
