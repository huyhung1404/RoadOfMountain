using Script.GamePlay;
using Script.GamePlay.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI
{
    public class SelectMenu : CanvasManager
    {
        private int maxLevel;
        [SerializeField] private Transform maps;
        private void Awake(){
            maxLevel = PlayerPrefs.GetInt("maxLevel");
            for (int i = 0; i < maxLevel; i++)
            {
                maps.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        public void SelectMap(int _index)
        {
            clickSource.PlayOneShot(clickAudio);
            if (_index > maxLevel) return;
            GameManager.CurrentMapIndex = _index;
            SceneManager.LoadScene(3);
        }
    }
}
