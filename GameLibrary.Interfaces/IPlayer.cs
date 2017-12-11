namespace GameLibrary.Interfaces
{
    public interface IPlayer
    {
        string GetName();
        IPoint GetCurrentLocation();
        void PerformMovement();
        void UpdateLocation(IPoint point);
        void UpdateRespawn(IPoint point);

        void Attack(bool useMagic, IEnemy enemy);
        void ChargeMana(IEnemy enemy);
        bool IsDefeated(IEnemy enemy);
        //TODO: remove null once I add different types of damage
        int GetCostOfSpell(string spellName = null);

        IStats GetCurrentStats();
        IPlayerStats GetAllCurrentStats();

        void OpenMonsterCollection();
        void PrintStats(bool displayCombatStats = false);
        void ResetHealthMana();
    }
}
