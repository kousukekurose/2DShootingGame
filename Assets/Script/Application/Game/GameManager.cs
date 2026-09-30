using MessagePipe;


namespace Project.Application.Game
{
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public class GameManager
    {
        private GameState _currentState = GameState.Menu;
        private int _score = 0;
        private int _currentWave = 1;
        private int _enemiesKilled = 0;
        private readonly IPublisher<GameStateChangedEvent> _stateChangedPublisher;
        private readonly IPublisher<ScoreUpdatedEvent> _scoreUpdatedPublisher;

        public GameState CurrentState => _currentState;
        public int Score => _score;
        public int CurrentWave => _currentWave;
        public int EnemiesKilled => _enemiesKilled;

        public GameManager(
            IPublisher<GameStateChangedEvent> stateChangedPublisher,
            IPublisher<ScoreUpdatedEvent> scoreUpdatedPublisher
        )
        {
            _stateChangedPublisher = stateChangedPublisher;
            _scoreUpdatedPublisher = scoreUpdatedPublisher;
        }

        public void StartGame()
        {
            _score = 0;
            _currentWave = 1;
            _enemiesKilled = 0;
        }

        public void PauseGame()
        {
            
        }

        public void ResumeGame()
        {
            
        }

        public void OnPlayerDeath()
        {
            
        }

        public void OnEnemyKilled()
        {
            
        }

        public void OnWaveComplete()
        {
            _currentWave++;
            _score += 100;
        }

        public void OnVictory()
        {
            
        }
    }

    public class GameStateChangedEvent
    {
        public GameState NewState{get; set;}
    }

    public class ScoreUpdatedEvent
    {
        public int NewScore{get; set;}
    }
}

