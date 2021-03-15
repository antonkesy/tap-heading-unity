/*
MIT License
Copyright (c) 2021 Anton Kesy
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System.Collections;
using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("playerMovementScript")] [SerializeField]
    private PlayerManager playerManager;

    [FormerlySerializedAs("levelGeneratorScript")] [SerializeField]
    private LevelManager levelManager;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;

    private bool _isRunning;

    private bool _waitingToStartFreshGame;

    private bool _waitingToRestartGame;

    private int _score;

    private int _highScore;


    [SerializeField] private AudioManager audioManager;

    private void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        uiManager.ShowStartMenuUI();
        audioManager.SetSound(PlayerPrefs.GetInt("soundOff", 1) == 0);
        SignInToGooglePlayServices();
        audioManager.PlayStartApplication();
        playerManager.SpawnPlayer();
    }

    private void SignInToGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, null);
    }

    // Update is called once per frame
    private void Update()
    {
        //DEBUG----------------------------------------------

        if (Input.GetKeyDown("space"))
        {
            if (_isRunning)
            {
                if (playerManager.CallChangeDirection())
                {
                    audioManager.PlayTapPlayer();
                }
            }
            else if (_waitingToStartFreshGame)
            {
                if (uiManager.isAboutOn()) return;
                audioManager.PlayTapPlayer();
                StartGame();
            }
            else if (_waitingToRestartGame)
            {
                if (uiManager.isAboutOn()) return;
                audioManager.PlayTapPlayer();
                RestartGame();
            }
        }

        //-------------------------------------------------

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetButtonDown("Fire1"))
        {
            if (Input.touches.Select(touch => touch.fingerId)
                .Any(id => EventSystem.current.IsPointerOverGameObject(id)))
            {
                return;
            }

            if (_isRunning)
            {
                if (playerManager.CallChangeDirection())
                {
                    audioManager.PlayTapPlayer();
                }
            }
            else if (_waitingToStartFreshGame)
            {
                if (uiManager.isAboutOn()) return;
                audioManager.PlayTapPlayer();
                StartGame();
            }
            else if (_waitingToRestartGame)
            {
                if (uiManager.isAboutOn()) return;
                audioManager.PlayTapPlayer();
                RestartGame();
            }
        }
    }

    internal void ReadyToStartGameCallback()
    {
        _waitingToStartFreshGame = true;
    }

    private void RestartGame()
    {
        _score = 0;
        uiManager.UpdateScoreText(_score);
        _isRunning = true;
        _waitingToRestartGame = false;
        levelManager.RestartGame();
        uiManager.ShowPlayUI();
        playerManager.StartMoving();
    }


    private void StartGame()
    {
        _score = 0;
        uiManager.UpdateScoreText(_score);
        _isRunning = true;
        _waitingToStartFreshGame = false;
        levelManager.StartGame();
        uiManager.ShowPlayUI();
        playerManager.StartMoving();
    }

    internal void CoinPickedUpCallback()
    {
        audioManager.PlayCollectCoin();
        uiManager.UpdateScoreText(++_score);
        levelManager.AddSpeed();
    }

    internal void DestroyPlayerCallback()
    {
        OnPlayerDestroy();
    }

    private void OnPlayerDestroy()
    {
        audioManager.PlayDestroyPlayer();
        cameraManager.StartShaking();
        levelManager.Pause();
        _isRunning = false;
        uiManager.ShowReturningMenuUI();
        StartCoroutine(WaitToRestart());
        CheckNewHighScore();
        CheckAchievement(_score);
        levelManager.LostGame();
        playerManager.SpawnPlayer();
    }

    private void CheckNewHighScore()
    {
        if (_highScore < _score)
        {
            _highScore = _score;
            uiManager.UpdateHighScoreText(_highScore);
            PlayerPrefs.SetInt("highScore", _highScore);
            audioManager.PlayNewHighScore();
            uiManager.FadeInNewHighScore();
            //TODO("UI Effects")
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                Social.ReportScore(_highScore, GPGSIds.leaderboard_high_score, success => { });
            }
        }
    }

    private void CheckAchievement(int highScore)
    {
        if (highScore == 0)
        {
            Social.ReportProgress(GPGSIds.achievement_oof, 0.0f, null);
        }

        if (highScore >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_triple_digest, 0.0f, null);
        }

        if (highScore >= 69)
        {
            Social.ReportProgress(GPGSIds.achievement_nice, 0.0f, null);
        }

        if (highScore >= 42)
        {
            Social.ReportProgress(
                GPGSIds.achievement_answer_to_the_ultimate_question_of_life_the_universe_and_everything, 0.0f, null);
        }

        if (highScore >= 10)
        {
            Social.ReportProgress(GPGSIds.achievement_double_digest, 0.0f, null);
        }
    }

    private IEnumerator WaitToRestart()
    {
        yield return new WaitForSecondsRealtime(1f);
        _waitingToRestartGame = true;
    }
}