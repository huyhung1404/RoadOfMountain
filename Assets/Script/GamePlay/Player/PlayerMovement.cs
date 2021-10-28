using System;
using System.Collections.Generic;
using Script.GamePlay.Manager;
using Script.Map;
using UnityEngine;

namespace Script.GamePlay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        #region MapProperties

        public static readonly Dictionary<Enemy.Enemy, int> Enemies = new Dictionary<Enemy.Enemy, int>();
        private MapPosition m_CurrentPosition;
        private Vector3 m_TargetPosition;
        private bool m_EndPoint;
        private int m_SizeMap;
        private int m_Index;

        #endregion

        #region MovementProperties

        private const float m_Speed = 30f;
        private CharacterController m_Character;
        private bool isMoving;
        private bool waitingToBeKilled;
        private bool isWaitingAnimation;
        public static event Action PlayerMoving;

        #endregion

        #region InputProperties

        private PlayerInput playerInput;
        private Vector2 m_InputValue;
        private Vector2 m_LastValue;

        #endregion

        private void Awake()
        {
            GameManager.Instance.ChangeMapInformation += MapInformationHasChange;
            playerInput = new PlayerInput();
        }

        public void OnEnable()
        {
            playerInput.Player.Enable();
        }

        private void Start()
        {
            m_Character = gameObject.GetComponent<CharacterController>();
            playerInput.Player.Movement.performed += ctx => MoveInputHandle(ctx.ReadValue<Vector2>());
        }

        public void OnDisable()
        {
            playerInput.Player.Disable();
            if (GameManager.Instance == null) return;
            GameManager.Instance.ChangeMapInformation -= MapInformationHasChange;
        }

        private void MapInformationHasChange()
        {
            Enemies.Clear();
            m_CurrentPosition = GameManager.MapInformation.mapPositions[0];
            transform.position = m_CurrentPosition.position.ToVector3();
            Player.Instance.PlayerIdle();
            Player.Instance.Blood.SetActive(false);
            waitingToBeKilled = false;
            isWaitingAnimation = false;
            m_SizeMap = GameManager.MapInformation.mapPositions.Count - 1;
            m_EndPoint = false;
        }

        private void MoveInputHandle(Vector2 _direction)
        {
            if (waitingToBeKilled) return;
            if (isWaitingAnimation) return;
            if (isMoving) return;
            m_Index = -1;
            if (_direction.x == -1)
            {
                if (m_CurrentPosition.leftIndex == -1) return;
                m_Index = m_CurrentPosition.leftIndex;
                m_CurrentPosition = GameManager.MapInformation.mapPositions[m_Index];
            }
            else if (_direction.x == 1)
            {
                if (m_CurrentPosition.rightIndex == -1) return;
                m_Index = m_CurrentPosition.rightIndex;
                m_CurrentPosition = GameManager.MapInformation.mapPositions[m_Index];
            }
            else if (_direction.y == -1)
            {
                if (m_CurrentPosition.backIndex == -1) return;
                m_Index = m_CurrentPosition.backIndex;
                m_CurrentPosition = GameManager.MapInformation.mapPositions[m_Index];
            }
            else if (_direction.y == 1)
            {
                if (m_CurrentPosition.forwardIndex == -1) return;
                m_Index = m_CurrentPosition.forwardIndex;
                m_CurrentPosition = GameManager.MapInformation.mapPositions[m_Index];
            }

            if (m_Index == m_SizeMap)
                m_EndPoint = true;

            if (Enemies.ContainsValue(m_Index))
                waitingToBeKilled = true;

            m_InputValue = _direction;
            MovePlayer();
        }

        private void MovePlayer()
        {
            transform.rotation = Quaternion.Euler(0, m_InputValue.x * -90f + (m_InputValue.y > 0 ? 0 : -1) * 180, 0);
            m_TargetPosition = m_CurrentPosition.position.ToVector3();
            Player.Instance.PlayerMove();
            PlayerMoving?.Invoke();
            isMoving = true;
        }

        public void WaitDecision(bool _waitingAttack)
        {
            Player.Instance.PlayerIdle();
            m_LastValue = m_InputValue;
            isWaitingAnimation = _waitingAttack;
            isMoving = false;
        }

        public void ContinueMove()
        {
            m_InputValue = m_LastValue;
            Player.Instance.PlayerMove();
            isMoving = true;
        }

        private void FixedUpdate()
        {
            if (!isMoving) return;
            if ((transform.position - m_TargetPosition).sqrMagnitude < 1f)
            {
                WaitDecision(false);
                if (waitingToBeKilled)
                    Player.Instance.DeathByEnemy(m_Index);
                if (m_EndPoint)
                    GameManager.Instance.NextLevel();
                return;
            }

            m_Character.Move((m_TargetPosition - transform.position).normalized * (m_Speed * Time.fixedDeltaTime));
        }
    }
}