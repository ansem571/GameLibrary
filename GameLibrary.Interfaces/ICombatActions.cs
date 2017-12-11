namespace GameLibrary.Interfaces
{
    public interface ICombatActions
    {
        void Attack(IStats attacker, IStats defender, bool useMagic);
        void ChargeMana(IStats actorStats);

        bool IsDefeated(IStats stats);
    }
}
