using Script.GamePlay;
using Script.GamePlay.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private Text textTutorial;
        [TextArea]
        [SerializeField] private string[] texts;

        private void OnEnable()
        {
            textTutorial.text = texts[GameManager.CurrentMapIndex];
        }
    }
}
