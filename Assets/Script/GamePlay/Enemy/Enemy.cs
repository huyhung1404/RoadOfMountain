using Script.GamePlay.Player;
using UnityEngine;

namespace Script.GamePlay.Enemy
{
    public class Enemy : MonoBehaviour
    {
        private AudioSource audioSource;
        [SerializeField] private AudioClip enemyDeathAudio;
        [SerializeField] private AudioClip enemyAttackAudio;
        [SerializeField] private Transform powerPosition;
        [SerializeField] private Vector3 powerOffset;
        [SerializeField] private int m_IndexDeath;
        private void Start()
        {
            if (m_IndexDeath == 0) return;
            PlayerMovement.Enemies.Add(this,m_IndexDeath);
        }

        public void EnemyDeath()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(enemyDeathAudio);
            if (m_IndexDeath == 0) return;
            PlayerMovement.Enemies.Remove(this);
        }
        
        
        public void KillPlayer(Vector3 _hit)
        {
            powerPosition.position = _hit + powerOffset;
            Animator enemyAnimator = gameObject.GetComponent<Animator>();
            audioSource = gameObject.AddComponent<AudioSource>();
            enemyAnimator.Play("attack_short_001 0");
            audioSource.PlayOneShot(enemyAttackAudio);
        }

        public void ChangeAttackRange(int _indexChange)
        {
            m_IndexDeath = _indexChange;
            PlayerMovement.Enemies[this] = m_IndexDeath;
        }
    }
}
