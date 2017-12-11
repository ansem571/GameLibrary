namespace GameLibrary.Interfaces
{
    public interface IStats
    {
        int Level { get; set; }
        int CurrentHealth { get; set; }
        int MaxHealth { get; set; }
        int CurrentMana { get; set; }
        int MaxMana { get; set; }
        int ManaCost { get; set; }
        int P_Atk { get; set; }
        int P_Def { get; set; }
        int M_Atk { get; set; }
        int M_Def { get; set; }
        int AttackSpeed { get; set; }
        int Gold { get; set; }

        void DisplayStats(bool displayCombatStats = false);
    }
}
