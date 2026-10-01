namespace Project.Domain.Events
{
    public sealed class EnemyKilled
    {
        public string EnemyId { get; }

        public EnemyKilled(string enemyId)
        {
            EnemyId = enemyId;
        }
    }
}
