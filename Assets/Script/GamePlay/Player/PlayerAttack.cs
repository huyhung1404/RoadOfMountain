using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Script.GamePlay.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private Vector3 directionAttack;
        private bool isDestroying;
        [Header("Event")]
        [SerializeField] private UnityEvent<bool> WaitAttack;
        [SerializeField] private UnityEvent ContinueMove;

        private void Start()
        {
            InvokeRepeating(nameof(PlayerAttackUpdate),0f,0.1f);
        }

        private void PlayerAttackUpdate()
        {
            if (isDestroying) return;
            directionAttack = transform.TransformDirection(Vector3.forward);
            if (!Physics.Raycast(transform.position + 10 * Vector3.up, directionAttack, out RaycastHit hit, 15,
                1 << 9)) return;
            isDestroying = true;
            WaitAttack.Invoke(true);
            StartCoroutine(DestroyEnemy(hit.collider));
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private IEnumerator DestroyEnemy(Collider enemy)
        {
            Animator enemyAnimator = enemy.gameObject.GetComponent<Animator>();
            Player.Instance.PlayAnimation("Male Attack 1");
            yield return WAIT_PLAY_ANIMATION;
            Player.Instance.PlayAudioOneShot(Player.Instance.AttackAudio);
            yield return WAIT_PLAY_ANIMATION;
            enemyAnimator.Play("dead");
            enemy.GetComponent<Enemy.Enemy>().EnemyDeath();
            yield return WAIT_ENEMY_DEATH;
            Destroy(enemy.gameObject);
            ContinueMove.Invoke();
            isDestroying = false;
        }

        private readonly WaitForSeconds WAIT_PLAY_ANIMATION = new WaitForSeconds(0.3f);
        private readonly WaitForSeconds WAIT_ENEMY_DEATH = new WaitForSeconds(1.5f);
    }
}
