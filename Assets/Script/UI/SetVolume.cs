using UnityEngine;
using UnityEngine.Audio;

namespace Script.UI
{
    public class SetVolume : MonoBehaviour
    {
        public AudioMixer mixer;
        public void SetLevel(float value){
            mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
            if(value == 0){
                mixer.SetFloat("MusicVol" , -80);
            }
        }
        // Start is called before the first frame update
    
    }
}
