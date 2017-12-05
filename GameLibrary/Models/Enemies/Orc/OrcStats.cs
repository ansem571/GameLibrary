using GameLibrary.Interfaces;
using System;

namespace GameLibrary.Models.Enemies.Orc
{
    public class OrcStats : IStats
    {
        public int Level { get; set; } = 1;
        public int CurrentHealth { get; set; } = 20;
        public int MaxHealth { get; set; } = 20;
        public int CurrentMana { get; set; } = 20;
        public int MaxMana { get; set; } = 20;
        public int ManaCost { get; set; } = 10;
        public int P_Atk { get; set; } = 3;
        public int P_Def { get; set; } = 1;
        public int M_Atk { get; set; } = 1;
        public int M_Def { get; set; } = 1;
        public int AttackSpeed { get; set; } = 3;
        public int Gold { get; set; } = 20;


        //Do not need these for enemies
        public int Deaths { get; set; }
        public int CurrentExp { get; set; }
        public int MaxExp { get; set; }

        public OrcStats()
        {
            if (Level != 1)
            {
                MaxHealth *= 1 + (Level / 10);
                CurrentHealth = MaxHealth;
                MaxMana *= 1 + (Level / 10);
                CurrentMana = MaxMana;
                P_Atk *= 1 + (Level / 10);
                P_Def *= 1 + (Level / 10);
                M_Atk *= 1 + (Level / 10);
                M_Def *= 1 + (Level / 10);

                AttackSpeed *= 1 + (Level / 3);
                Gold *= Level;
            }
        }

        public void LevelUp()
        {
            throw new NotImplementedException();
        }

        public void DisplayStats(bool displayCombatStats)
        {
            Console.WriteLine($"Level: {Level}");
            if (displayCombatStats)
            {
                Console.WriteLine($"HP: {CurrentHealth}/{MaxHealth}");
                Console.WriteLine($"MP: {CurrentMana}/{MaxMana}");
            }
        }
    }
}
