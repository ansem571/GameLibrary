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
        int CurrentExp { get; set; }
        int MaxExp { get; set; }
        int Deaths { get; set; }
        int Gold { get; set; }

        void LevelUp();
        void DisplayStats(bool displayCombatStats = false);
    }
}
