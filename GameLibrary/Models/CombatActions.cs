using GameLibrary.Enums;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GameLibrary.Models
{
    public class CombatActions : ICombatActions
    {
        public void Attack(IStats attacker, IStats opp, bool useMagic)
        {
            float formula = useMagic ? attacker.M_Atk - opp.M_Def : attacker.P_Atk - opp.P_Def;
            if (useMagic)
            {
                if (attacker.CurrentMana >= attacker.ManaCost)
                    attacker.CurrentMana -= attacker.ManaCost;
                else // you cant do damage if you don't have the mana for it
                {
                    Console.WriteLine("You need to charge your mana. You did no damage this turn.");
                    Thread.Sleep(1000);
                    formula = 0;
                }
            }
            if (formula < 1) // you nudged them a little
                formula = 1;

            opp.CurrentHealth -= Convert.ToInt32(formula);
        }
       
        public void ChargeMana(IStats actorStats)
        {
            actorStats.CurrentMana = (int)(actorStats.CurrentMana * 1.1f);

            if (actorStats.CurrentMana > actorStats.MaxMana)
                actorStats.CurrentMana = actorStats.MaxMana;
        }

        public bool IsDefeated(IStats stats)
        {
            return stats.CurrentHealth <= 0;
        }
    }
}
