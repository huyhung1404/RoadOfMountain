using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.GamePlay.Manager;
using UnityEngine;

namespace Script.GamePlay.Topography
{
    public class MapMoving : MonoBehaviour
    {
        [Serializable]
        private struct EnemyChange
        {
            public Enemy.Enemy Enemy;
            public int Index;
            public int OriginIndex;
        }

        [Serializable]
        private struct PositionChange
        {
            [Serializable]
            public enum Direction
            {
                FORWARD,
                BACK,
                LEFT,
                RIGHT
            }
            public int Index;
            public int Change;
            public int Origin;
            public Direction type;
        }
        [SerializeField] private Vector3 m_Target;
        [SerializeField] private List<EnemyChange> m_EnemyChanges;
        [SerializeField] private List<PositionChange> m_PositionChange;
        [SerializeField] private float m_Time = 1.5f;
        [SerializeField] private Ease m_Ease;
        private Vector3 m_OriginPosition;
        private event Action changeTarget;
        private bool isGoOut;

        private void Start()
        {
            m_OriginPosition = transform.position;
            if (m_EnemyChanges.Count != 0)
            {
                changeTarget += ChangeEnemyTarget;
            }

            if (m_PositionChange.Count != 0)
            {
                changeTarget += ChangePositionMove;
            }
        }

        public void Move()
        {
            changeTarget?.Invoke();
            GetComponent<AudioSource>().Play();
            transform.DOMove(m_Target, m_Time).SetEase(m_Ease).OnComplete(() =>
            {
                isGoOut = !isGoOut;
                GetComponent<AudioSource>().Stop();
                (m_Target, m_OriginPosition) = (m_OriginPosition, m_Target);
            });
        }

        private void ChangeEnemyTarget()
        {
            foreach (EnemyChange enemy in m_EnemyChanges)
            {
                if (!isGoOut)
                {
                    enemy.Enemy.ChangeAttackRange(enemy.Index);
                    continue;
                }
                enemy.Enemy.ChangeAttackRange(enemy.OriginIndex);
            }
        }

        private void ChangePositionMove()
        {
            foreach (PositionChange position in m_PositionChange)
            {
                if (!isGoOut)
                {
                    switch (position)
                    {
                        case { type: PositionChange.Direction.FORWARD }:
                            GameManager.MapInformation.mapPositions[position.Index].forwardIndex = position.Change;
                            continue;
                        case { type: PositionChange.Direction.BACK }:
                            GameManager.MapInformation.mapPositions[position.Index].backIndex = position.Change;
                            continue;
                        case { type: PositionChange.Direction.LEFT }:
                            GameManager.MapInformation.mapPositions[position.Index].leftIndex = position.Change;
                            continue;
                        case { type: PositionChange.Direction.RIGHT }:
                            GameManager.MapInformation.mapPositions[position.Index].rightIndex = position.Change;
                            continue;
                    }
                }
                switch (position)
                {
                    case { type: PositionChange.Direction.FORWARD }:
                        GameManager.MapInformation.mapPositions[position.Index].forwardIndex = position.Origin;
                        break;
                    case { type: PositionChange.Direction.BACK }:
                        GameManager.MapInformation.mapPositions[position.Index].backIndex = position.Origin;
                        break;
                    case { type: PositionChange.Direction.LEFT }:
                        GameManager.MapInformation.mapPositions[position.Index].leftIndex = position.Origin;
                        break;
                    case { type: PositionChange.Direction.RIGHT }:
                        GameManager.MapInformation.mapPositions[position.Index].rightIndex = position.Origin;
                        break;
                }
            }
        }
    }
}