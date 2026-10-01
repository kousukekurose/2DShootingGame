namespace Project.Presentation.Events
{
    public sealed class PlayerStateChangedEvent
    {
        public string StateName { get; set; }
        public float Timestamp { get; set; }
    }
}
