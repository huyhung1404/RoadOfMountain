using System.Collections;
using System.Linq;
using Script.Core;
using Script.GamePlay.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Script.GamePlay.Player
{
    public class Player : Singleton<Player>
    {
        public AudioClip RunAudio;
        public AudioClip DeathAudio;
        public AudioClip AttackAudio;
        public GameObject Blood;
        [SerializeField]
        private UnityEvent<bool> m_WaitAnimation;
        private Animator m_Animator;
        private AudioSource m_AudioSource;

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.clip = null;
        }

        public void PlayAnimation(string _name)
        {
            m_Animator.Play(_name);
        }

        public void PlayAudioOneShot(AudioClip _clip)
        {
            m_AudioSource.PlayOneShot(_clip);
        }

        public void PlayerMove()
        {
            m_AudioSource.clip = RunAudio;
            m_Animator.Play("Male_Walk");
            if(m_AudioSource.isPlaying) return;
            m_AudioSource.Play();
        }

        public void PlayerIdle()
        {
            m_AudioSource.clip = null;
            m_Animator.Play("Male Idle");
        }

        public void DeathByEnemy(int value)
        {
            foreach (var pair in PlayerMovement.Enemies.Where(pair => pair.Value == value))
                pair.Key.KillPlayer(transform.position);
            StartCoroutine(WaitLoadSceneDeathByEnemy());
        }
        private IEnumerator WaitLoadSceneDeathByEnemy()
        {
            yield return WAIT_ENEMY_ATTACK;
            m_Animator.Play("Male Die");
            m_AudioSource.PlayOneShot(DeathAudio);
            Blood.SetActive(true);
            yield return WAIT_HERO_DEATH;
            GameManager.Instance.ResetLevel();
        }
        
        public void DeathBySaw()
        {
            m_WaitAnimation?.Invoke(true);
            StartCoroutine(WaitLoadSceneDeathBySaw());
        }

        private IEnumerator WaitLoadSceneDeathBySaw()
        {
            m_Animator.Play("Male Die");
            m_AudioSource.PlayOneShot(DeathAudio);
            Blood.SetActive(true);
            yield return new WaitForSeconds(1f);
            GameManager.Instance.ResetLevel();
        }
        
        private readonly WaitForSeconds WAIT_ENEMY_ATTACK = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds WAIT_HERO_DEATH = new WaitForSeconds(0.75f);
    }
}
