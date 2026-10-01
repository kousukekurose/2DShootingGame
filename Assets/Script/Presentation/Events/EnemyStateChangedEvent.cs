namespace Project.Presentation.Events
{
    public sealed class EnemyStateChangedEvent
    {
        public string EnemyId { get; set; }
        public string StateName { get; set; }
    }
}
