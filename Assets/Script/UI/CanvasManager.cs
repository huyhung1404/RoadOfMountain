using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] protected AudioClip clickAudio;
        [SerializeField] protected AudioSource clickSource;
        
        public void PopupMenu(GameObject _gameObject)
        {
            clickSource.PlayOneShot(clickAudio);
            _gameObject.SetActive(!_gameObject.activeSelf);
        }

        public void BackMainMenu()
        {
            clickSource.PlayOneShot(clickAudio);
            SceneManager.LoadScene(1);
        }
    }
}