namespace Project.Domain.Events
{
    public sealed class PlayerDied
    {
        public string PlayerId { get; }

        public PlayerDied(string playerId)
        {
            PlayerId = playerId;
        }
    }
}
