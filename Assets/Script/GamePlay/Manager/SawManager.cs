using Script.GamePlay.Enemy;
using Script.GamePlay.Player;
using UnityEngine;

namespace Script.GamePlay.Manager
{
    public class SawManager : MonoBehaviour
    {
        private AudioSource audioSource;
        private SawMove[] listSaws;
        [SerializeField] private AudioClip saw;

        private void Start()
        {
            PlayerMovement.PlayerMoving += RunSaw;
            
            audioSource = GetComponent<AudioSource>();
            listSaws = new SawMove[transform.childCount];
            for (int i = 0; i < listSaws.Length; i++)
            {
                listSaws[i] = transform.GetChild(i).GetComponent<SawMove>();
            }
        }

        private void RunSaw()
        {
            foreach (SawMove sawScript in listSaws)
            {
                sawScript.RunSaw();
            }
            audioSource.PlayOneShot(saw);
        }

        private void OnDisable()
        {
            PlayerMovement.PlayerMoving -= RunSaw;
        }
    }
}
