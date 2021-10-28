using DG.Tweening;
using Script.GamePlay.Manager;
using Script.Map;
using UnityEngine;

namespace Script.GamePlay.Enemy
{
    public class SawMove : MonoBehaviour
    {
        private int lengthWay;
        private int currentIndex;
        private int direction = 1;
        private Vector3 offset;
        [SerializeField] private int[] sawWay;
        [SerializeField] private float m_Time = 1f;
        [SerializeField] private Ease m_Ease;

        public void RunSaw()
        {
            currentIndex += direction;
            transform.DOMove(GameManager.MapInformation
                    .mapPositions[sawWay[currentIndex]].position.ToVector3() + offset, m_Time)
                .SetEase(m_Ease)
                .OnComplete(SetDirection);
            
        }

        private void Start()
        {
            lengthWay = sawWay.Length;
            offset = transform.position - GameManager.MapInformation.mapPositions[sawWay[0]].position.ToVector3();
            currentIndex = 0;
        }

        private void SetDirection()
        {
            if (currentIndex == lengthWay - 1)
            {
                direction = -1;
            }
            else if (currentIndex == 0)
            {
                direction = 1;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Player.Player.Instance.DeathBySaw();
        }
    }
}