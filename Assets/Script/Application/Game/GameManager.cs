using MessagePipe;
using VContainer.Unity;
using System;

namespace Project.Application.Game
{
    public enum GameState
    {
        Menu,
        CountDown,
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public class GameManager : IInitializable, IDisposable
    {
        private GameState _currentState = GameState.Menu;
        private int _score = 0;
        private int _currentWave = 1;
        private int _enemiesKilled = 0;
        private readonly IPublisher<GameStateChangedEvent> _stateChangedPublisher;
        private readonly IPublisher<ScoreUpdatedEvent> _scoreUpdatedPublisher;
        private readonly ISubscriber<Project.Application.Abstractions.Events.PlayerDeathEvent> _playerDeathSubscriber;
        private readonly DisposableBagBuilder _bag = DisposableBag.CreateBuilder();
        public GameState CurrentState => _currentState;
        public int Score => _score;
        public int CurrentWave => _currentWave;
        public int EnemiesKilled => _enemiesKilled;

        public GameManager(
            IPublisher<GameStateChangedEvent> stateChangedPublisher,
            IPublisher<ScoreUpdatedEvent> scoreUpdatedPublisher,
            ISubscriber<Project.Application.Abstractions.Events.PlayerDeathEvent> playerDeathSubscriber
        )
        {
            _stateChangedPublisher = stateChangedPublisher;
            _scoreUpdatedPublisher = scoreUpdatedPublisher;
            _playerDeathSubscriber = playerDeathSubscriber;
        }
        private void SetState(GameState state)
        {
            _currentState = state;
            _stateChangedPublisher.Publish(new GameStateChangedEvent { NewState = state });
        }

        public void BeginCountdown()
        {
            if(_currentState != GameState.Menu) return;
            SetState(GameState.CountDown);
        }

        public void StartGame()
        {
            if(_currentState != GameState.CountDown) return;
            _score = 0;
            _currentWave = 1;
            _enemiesKilled = 0;
            _scoreUpdatedPublisher.Publish(new ScoreUpdatedEvent { NewScore = 0 });
            SetState(GameState.Playing);
        }

        public void PauseGame()
        {
            SetState(GameState.Paused);
        }

        public void ResumeGame()
        {
            SetState(GameState.Playing);
        }

        public void OnPlayerDeath()
        {
            Project.Infrastructure.Unity.Logging.UnityLogger.Log("Player死亡通知受け取った");
            SetState(GameState.GameOver);
        }

        public void Initialize()
        {
            _playerDeathSubscriber.Subscribe(x =>
            {
                OnPlayerDeath();
            }).AddTo(_bag);
        }

        public void OnEnemyKilled()
        {
            _enemiesKilled++;
            _score += 100;
            _scoreUpdatedPublisher.Publish(
                new ScoreUpdatedEvent 
                { 
                    NewScore = _score 
                });
        }

        public void OnWaveComplete()
        {
            _currentWave++;
            _score += 1000;
            _scoreUpdatedPublisher.Publish(
                new ScoreUpdatedEvent 
                { 
                    NewScore = _score 
                });
        }

        public void OnVictory()
        {
            SetState(GameState.Victory);
        }

        public void Dispose()
        {
            _bag.Build().Dispose();
        }
    }

}
