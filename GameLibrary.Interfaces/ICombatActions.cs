namespace GameLibrary.Interfaces
{
    public interface ICombatActions
    {
        void Attack(params object[] args);
        void ChargeMana(params object[] args);

        bool IsDefeated(IStats stats);
    }
}
