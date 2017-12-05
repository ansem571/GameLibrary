using System;

namespace GameLibrary.Interfaces
{
    public interface IBattleManager
    {
        Random Randomizer { get; }
        Action Victory { get; set; }
        Action Defeat { get; set; }

        void InitActions(Action win, Action lose);
        void Battle(IPlayer player, IEnemy enemy);
    }
}
