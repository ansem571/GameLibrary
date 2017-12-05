using GameLibrary.Enums;
using GameLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GameLibrary.Models
{
    public class CombatActions : ICombatActions
    {
        public void Attack(params object[] args)
        {
            bool useMagic = false;
            IPlayer player = null;
            IEnemy enemy = null;
            ActorEnum attacker = ActorEnum.None;
            foreach (var arg in args)
            {
                if (arg is bool)
                    useMagic = (bool)arg;
                if (arg is IPlayer)
                    player = (IPlayer)arg;
                if (arg is IEnemy)
                    enemy = (IEnemy)arg;
                if (arg is ActorEnum)
                    attacker = (ActorEnum)arg;
            }
            Attack(useMagic, player ?? throw new Exception("cannot convert to IPlayer"), enemy ?? throw new Exception("cannot convert to IEnemy"), attacker);
        }

        public void ChargeMana(params object[] args)
        {
            IPlayer player = null;
            IEnemy enemy = null;
            ActorEnum actor = ActorEnum.None;
            List<object> unnecessaryParams = new List<object>();
            foreach (var arg in args)
            {
                if (arg is IPlayer)
                    player = (IPlayer)arg;
                else if (arg is IEnemy)
                    enemy = (IEnemy)arg;
                else if (arg is ActorEnum)
                    actor = (ActorEnum)arg;
                else
                    unnecessaryParams.Add(arg);
            }

            if (unnecessaryParams.Count > 0)
                Console.WriteLine($"{unnecessaryParams.Count} unconverted params");
            ChargeMana(player, enemy, actor);
        }

        public bool IsDefeated(IStats stats)
        {
            return stats.CurrentHealth <= 0;
        }

        private void Attack(bool useMagic, IStats attacker, IStats opp)
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

        private void Attack(bool useMagic, IPlayer player, IEnemy enemy, ActorEnum attacker)
        {
            switch (attacker)
            {
                case ActorEnum.Player:
                    {
                        Attack(useMagic, player.PlayerStats, enemy.Stats);
                    }
                    break;
                case ActorEnum.Enemy:
                    {
                        Attack(useMagic, enemy.Stats, player.PlayerStats);
                    }
                    break;
                case ActorEnum.None:
                    {
                        throw new Exception("No attacker defined");
                    }
            }
        }
        private void ChargeMana(IStats actorStats)
        {
            actorStats.CurrentMana = (int)(actorStats.CurrentMana * 1.1f);

            if (actorStats.CurrentMana > actorStats.MaxMana)
                actorStats.CurrentMana = actorStats.MaxMana;
        }

        private void ChargeMana(IPlayer player, IEnemy enemy, ActorEnum actor)
        {
            switch (actor)
            {
                case ActorEnum.Player:
                    {
                        ChargeMana(player.PlayerStats);
                    }
                    break;
                case ActorEnum.Enemy:
                    {
                        ChargeMana(enemy.Stats);
                    }
                    break;
                case ActorEnum.None:
                    {
                        throw new Exception("No attacker defined");
                    }
            }
        }
    }
}
