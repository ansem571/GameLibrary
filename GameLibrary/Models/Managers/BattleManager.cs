using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Models.Managers
{
    public class BattleManager : IBattleManager
    {
        public Random Randomizer { get; }
        private int randomizeInt = 100;
        public Action Victory { get; set; }
        public Action Defeat { get; set; }

        public BattleManager()
        {
            Randomizer = new Random();
            Randomizer = new Random(Randomizer.Next(randomizeInt));
        }

        public void InitActions(Action win, Action lose)
        {
            Victory = win;
            Defeat = lose;
        }

        public void Battle(IPlayer player, IEnemy enemy)
        {
            while (true)
            {
                try
                {
                    var enemyAttackChance = Randomizer.Next(randomizeInt) + 1;
                    bool enemyCanAttack = enemyAttackChance <= 90;

                    var name = (enemy.IsBoss ? "Boss" : "") + enemy.Name + " " + enemy.GetType().Name;

                    Console.WriteLine($"You are battling {name}");
                    enemy.Stats.DisplayStats(true);
                    Console.WriteLine();

                    player.PrintStats(true);

                    var option = StaticHelperClass.ReadWriteOptions(new List<string> { "Attack w/out Magic", $"Attack w/ Magic({player.PlayerStats.ManaCost}MP)", "Charge Mana", "Retreat" });


                    //The if conditional is if either side is defeated.
                    switch (option)
                    {
                        case 1:
                            {
                                if (Attack(player, enemy, false, enemyAttackChance, enemyCanAttack))
                                    return;
                            }
                            break;
                        case 2:
                            {
                                if (Attack(player, enemy, true, enemyAttackChance, enemyCanAttack))
                                    return;
                            }
                            break;
                        case 3:
                            {
                                if (ChargeMana(player, enemy, enemyAttackChance, enemyCanAttack))
                                    return;
                            }
                            break;
                        case 4:
                            {
                                Console.WriteLine("WIMP");
                                Defeat();
                                return;
                            }
                        default:
                            {
                                Console.WriteLine("Cannot perform action.\r\n");
                            }
                            break;
                    }
                    Console.Clear();
                }
                catch (Exception e)
                {
                    StaticHelperClass.PrintException(e);
                }
            }
        }

        private bool Attack(IPlayer player, IEnemy enemy, bool useMagic, int enemyAttackChance, bool enemyCanAttack)
        {
            if (player.PlayerStats.AttackSpeed >= enemy.Stats.AttackSpeed)
            {
                player.Attack(useMagic, enemy);
                if (enemy.Stats.CurrentHealth <= 0)
                {
                    Victory();
                    return true;
                }
                if (enemyCanAttack)
                {
                    enemy.Attack(enemyAttackChance % 2 == 0 && enemy.Stats.CurrentMana >= enemy.Stats.ManaCost, player);
                    if (player.IsDefeated(enemy))
                    {
                        Defeat();
                        return true;
                    }
                }
            }
            else
            {
                enemy.Attack(enemyAttackChance % 2 == 0 && enemy.Stats.CurrentMana >= enemy.Stats.ManaCost, player);
                if (player.IsDefeated(enemy))
                {
                    Defeat();
                    return true;
                }
                player.Attack(useMagic, enemy);
                if (enemy.Stats.CurrentHealth <= 0)
                {
                    Victory();
                    return true;
                }
            }
            return false;
        }

        private bool ChargeMana(IPlayer player, IEnemy enemy, int enemyAttackChance, bool enemyCanAttack)
        {
            if (player.PlayerStats.AttackSpeed >= enemy.Stats.AttackSpeed)
            {
                player.ChargeMana(enemy);
                if (enemyCanAttack)
                {
                    enemy.Attack(enemyAttackChance % 2 == 0 && enemy.Stats.CurrentMana >= enemy.Stats.ManaCost, player);
                    if (player.IsDefeated(enemy))
                    {
                        Defeat();
                        return true;
                    }
                }
            }
            else
            {
                if (enemyCanAttack)
                {
                    enemy.Attack(enemyAttackChance % 2 == 0 && enemy.Stats.CurrentMana >= enemy.Stats.ManaCost, player);
                    if (player.IsDefeated(enemy))
                    {
                        Defeat();
                        return true;
                    }
                }
                player.ChargeMana(enemy);
            }
            return false;
        }
    }
}
