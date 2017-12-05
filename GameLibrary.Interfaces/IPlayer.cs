using System.Collections.Generic;

namespace GameLibrary.Interfaces
{
    public interface IPlayer
    {
        string Name { get; }
        Dictionary<string, int> MonsterCollection { get; }
        IPoint CurrentLocation { get; }
        IPoint RespawnLocation { get; }
        IStats PlayerStats { get; }
        ICharacterMovement Movement { get; }
        ICombatActions CombatActions { get; }

        void PerformMovement();
        void UpdateLocation(IPoint point);
        void UpdateRespawn(IPoint point);

        void Attack(bool useMagic, IEnemy enemy);
        void ChargeMana(IEnemy enemy);
        bool IsDefeated(IEnemy enemy);

        void OpenMonsterCollection();

        void PrintStats(bool displayCombatStats = false);
        void ResetHealthMana();
    }
}
