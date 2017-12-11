using GameLibrary.Interfaces;
using System;

namespace GameLibrary.Models.Player
{
    public class PlayerStats : IStats
    {
        public int Level { get; set; } = 1;
        public int CurrentHealth { get; set; } = 100;
        public int MaxHealth { get; set; } = 100;
        public int CurrentMana { get; set; } = 60;
        public int MaxMana { get; set; } = 60;
        public int ManaCost { get; set; } = 10;
        public int P_Atk { get; set; } = 7;
        public int P_Def { get; set; } = 4;
        public int M_Atk { get; set; } = 6;
        public int M_Def { get; set; } = 5;
        public int AttackSpeed { get; set; } = 10;
        public int CurrentExp { get; set; } = 0;
        public int MaxExp { get; set; } = 100;
        public int Deaths { get; set; } = 0;
        public int Gold { get; set; } = 0;

        public void LevelUp()
        {
            if (CurrentExp < MaxExp)
                return;
            Console.WriteLine($"You have leveled up from {Level} to {(Level + 1)}");
            CurrentExp -= MaxExp;
            MaxExp *= (int)Math.Ceiling(1.1d);
            Level++;
            var formula = 1 + (((Level - 1) / (double)Level) / 100);
            P_Atk = (int)Math.Ceiling((P_Atk * formula));
            P_Def = (int)Math.Ceiling((P_Def * formula));
            M_Atk = (int)Math.Ceiling(M_Atk * formula);
            M_Def = (int)Math.Ceiling(M_Def * formula);
            MaxMana = (int)Math.Ceiling(MaxMana * formula);
            CurrentMana = MaxMana;
            MaxHealth = (int)Math.Ceiling(MaxHealth * formula);
            CurrentHealth = MaxHealth;
            AttackSpeed = Level % 10 == 0 ? AttackSpeed += 10 : AttackSpeed++;
        }

        public void DisplayStats(bool displayCombatStats = false)
        {
            Console.WriteLine($"Level: {Level}");
            if (displayCombatStats)
            {
                Console.WriteLine($"HP: {CurrentHealth}/{MaxHealth}");
                Console.WriteLine($"MP: {CurrentMana}/{MaxMana}");
                Console.WriteLine($"Exp: {CurrentExp}/{MaxExp}");
                Console.WriteLine($"P.Atk: {P_Atk}\tP.Def: {P_Def}");
                Console.WriteLine($"M.Atk: {M_Atk}\tM.Def: {M_Def}");
                Console.WriteLine($"Attack speed: {AttackSpeed}");
            }
            Console.WriteLine($"Deaths: {Deaths}\tGold: {Gold}");
        }
    }
}
