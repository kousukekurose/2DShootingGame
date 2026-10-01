namespace Project.Application.Game
{
    public sealed class GameStateChangedEvent
    {
        public GameState NewState { get; set; }
    }
}
