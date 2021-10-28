using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.GamePlay.Manager
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] private Animator transition;

        public void LoadLevel()
        {
            StartCoroutine(WaitLoadLevel(SceneManager.GetActiveScene().buildIndex+1));
        }

        private IEnumerator WaitLoadLevel(int levelIndex)
        {
            yield return new WaitForSeconds(0.5f);
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(1f);
            if(levelIndex > PlayerPrefs.GetInt("maxLevel")){
                PlayerPrefs.SetInt("maxLevel", levelIndex);
            }
            PlayerPrefs.Save();
            SceneManager.LoadScene(levelIndex);
        }
    }
}
