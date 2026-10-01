using System;
using Cysharp.Threading.Tasks;
using Project.Application.Game;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Project.Presentation.UI
{
    public sealed class CountdownController : MonoBehaviour
    {
        [SerializeField] private Text countdownText;
        [SerializeField, Min(1)] private int countdownSeconds = 3;
        [SerializeField, Min(0f)] private float startTextDuration = 0.5f;

        private GameManager _gameManager;

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Start()
        {
            RunCountdownAsync().Forget();
        }

        private async UniTaskVoid RunCountdownAsync()
        {
            var cancellationToken = this.GetCancellationTokenOnDestroy();

            if (_gameManager == null)
            {
                Debug.LogError("GameManager was not injected into CountdownController.", this);
                return;
            }

            if (countdownText == null)
            {
                Debug.LogError("Countdown Text is not assigned.", this);
                return;
            }

            countdownText.gameObject.SetActive(true);
            _gameManager.BeginCountdown();

            for (var count = countdownSeconds; count > 0; count--)
            {
                countdownText.text = count.ToString();
                await UniTask.Delay(
                    TimeSpan.FromSeconds(1),
                    DelayType.UnscaledDeltaTime,
                    cancellationToken: cancellationToken);
            }

            countdownText.text = "START";
            await UniTask.Delay(
                TimeSpan.FromSeconds(startTextDuration),
                DelayType.UnscaledDeltaTime,
                cancellationToken: cancellationToken);

            countdownText.gameObject.SetActive(false);
            _gameManager.StartGame();
        }
    }
}
