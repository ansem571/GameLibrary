namespace GameLibrary.Interfaces
{
    public interface IEnemy
    {
        void Attack(bool useMagic, IPlayer player);

        bool IsEnemyBoss();

        bool IsDefeated(IPlayer player);
        IStats GetCurrentStats();
    }
}
