using Script.GamePlay;
using Script.GamePlay.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.UI
{
    public class GameMenu : CanvasManager
    {
        [SerializeField] private Text startButton;

        private void Start()
        {
            if (PlayerPrefs.GetInt("maxLevel") == 0)
            {
                startButton.text = "Start";
                return;
            }

            startButton.text = "Continue";
        }

        public void StartGame()
        {
            clickSource.PlayOneShot(clickAudio);
            int _level = PlayerPrefs.GetInt("level");
            GameManager.CurrentMapIndex = _level;
            SceneManager.LoadScene(3);
        }

        public void SelectMap()
        {
            clickSource.PlayOneShot(clickAudio);
            SceneManager.LoadScene(2);
        }

        public void ExitGame()
        {
            clickSource.PlayOneShot(clickAudio);
            Application.Quit();
        }
    }
}