using System;

namespace GameLibrary.Interfaces
{
    public interface IBattleManager
    {
        void Battle(IPlayer player, IEnemy enemy, Action victory, Action defeat);
    }
}
