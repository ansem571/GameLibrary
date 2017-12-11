using GameLibrary.Helpers;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace GameLibrary.Models.Managers
{
    public class BattleManager : IBattleManager
    {
        private Random Randomizer { get; set; }
        private int randomizeInt = 100;
        private Action Victory;
        private Action Defeat;

        public BattleManager()
        {
            Randomizer = new Random();
            Randomizer = new Random(Randomizer.Next(randomizeInt));
        }

        public void Battle(IPlayer player, IEnemy enemy, Action victory, Action defeat)
        {
            Victory = victory ?? throw new ArgumentNullException(nameof(victory));
            Defeat = defeat ?? throw new ArgumentNullException(nameof(defeat));

            while (true)
            {
                try
                {
                    var enemyAttackChance = Randomizer.Next(randomizeInt) + 1;
                    bool enemyCanAttack = enemyAttackChance <= 90;

                    var name = (enemy.IsEnemyBoss() ? "Boss" : "") + enemy.GetType().Name;

                    Console.WriteLine($"You are battling {name}");
                    enemy.GetCurrentStats().DisplayStats(true);
                    Console.WriteLine();

                    player.PrintStats(true);

                    var option = StaticHelperClass.ReadWriteOptions(new List<string> { "Attack w/out Magic", $"Attack w/ Magic({player.GetCostOfSpell()}MP)", "Charge Mana", "Retreat" });

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
            if (player.GetCurrentStats().AttackSpeed >= enemy.GetCurrentStats().AttackSpeed)
            {
                player.Attack(useMagic, enemy);
                if (enemy.GetCurrentStats().CurrentHealth <= 0)
                {
                    Victory();
                    return true;
                }
                if (enemyCanAttack)
                {
                    enemy.Attack(enemyAttackChance % 2 == 0 && enemy.GetCurrentStats().CurrentMana >= enemy.GetCurrentStats().ManaCost, player);
                    if (player.IsDefeated(enemy))
                    {
                        Defeat();
                        return true;
                    }
                }
            }
            else
            {
                enemy.Attack(enemyAttackChance % 2 == 0 && enemy.GetCurrentStats().CurrentMana >= enemy.GetCurrentStats().ManaCost, player);
                if (player.IsDefeated(enemy))
                {
                    Defeat();
                    return true;
                }
                player.Attack(useMagic, enemy);
                if (enemy.GetCurrentStats().CurrentHealth <= 0)
                {
                    Victory();
                    return true;
                }
            }
            return false;
        }

        private bool ChargeMana(IPlayer player, IEnemy enemy, int enemyAttackChance, bool enemyCanAttack)
        {
            if (player.GetCurrentStats().AttackSpeed >= enemy.GetCurrentStats().AttackSpeed)
            {
                player.ChargeMana(enemy);
                if (enemyCanAttack)
                {
                    enemy.Attack(enemyAttackChance % 2 == 0 && enemy.GetCurrentStats().CurrentMana >= enemy.GetCurrentStats().ManaCost, player);
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
                    enemy.Attack(enemyAttackChance % 2 == 0 && enemy.GetCurrentStats().CurrentMana >= enemy.GetCurrentStats().ManaCost, player);
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
