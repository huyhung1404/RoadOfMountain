using System;
using System.Collections;
using Cinemachine;
using Script.Core;
using Script.Map;
using UnityEngine;

namespace Script.GamePlay.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public static MapInformation MapInformation;
        private GameObject currentMap;
        private CinemachineTransposer cameraOffset;
        private AudioSource mainCameraAudio;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Animator loadLevelAnimator;
        [SerializeField] private AudioClip winLevel;
        [SerializeField] private int m_FrameRate = -1;
        public static int CurrentMapIndex;
        private TextAsset m_TextMap;
        public Action ChangeMapInformation;

        private void Awake()
        {
            Application.targetFrameRate = m_FrameRate;
        }

        private void Start()
        {
            if (Camera.main is { }) mainCameraAudio = Camera.main.GetComponent<AudioSource>();
            cameraOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            SetMap(CurrentMapIndex);
        }

        public void NextLevel()
        {
            StartCoroutine(WaitLoadLevel(++CurrentMapIndex));
        }

        public void ResetLevel()
        {
            StartCoroutine(RestartLevel());
        }

        private void SetMap(int _index)
        {
            currentMap = Instantiate(Resources.Load<GameObject>($"Map/Map{_index}/Map{_index}"), transform.position,
                Quaternion.identity);
            m_TextMap = Resources.Load<TextAsset>($"Map/Map{_index}/Map{_index}");
            MapInformation = JsonUtility.FromJson<MapInformation>(m_TextMap.text);
            cameraOffset.m_FollowOffset = MapInformation.cameraOffset.ToVector3();
            ChangeMapInformation?.Invoke();
        }

        private IEnumerator WaitLoadLevel(int _index)
        {
            yield return WAIT_05F;
            mainCameraAudio.PlayOneShot(winLevel);
            loadLevelAnimator.SetTrigger("Start");
            yield return WAIT_1F;
            Destroy(currentMap);
            SetMap(_index);
            PlayerPrefs.SetInt("level", _index);
            if (_index > PlayerPrefs.GetInt("maxLevel"))
            {
                PlayerPrefs.SetInt("maxLevel", _index);
            }

            PlayerPrefs.Save();
        }
        private IEnumerator RestartLevel()
        {
            yield return WAIT_05F;
            loadLevelAnimator.SetTrigger("Start");
            yield return WAIT_1F;
            Destroy(currentMap);
            m_TextMap = Resources.Load<TextAsset>($"Map/Map{CurrentMapIndex}/Map{CurrentMapIndex}");
            MapInformation = JsonUtility.FromJson<MapInformation>(m_TextMap.text);
            ChangeMapInformation?.Invoke();
            yield return WAIT_025F;
            currentMap = Instantiate(Resources.Load<GameObject>("Map/Map" + CurrentMapIndex+"/Map" + CurrentMapIndex), transform.position,
                Quaternion.identity);
        }
        private readonly WaitForSeconds WAIT_025F = new WaitForSeconds(0.25f);
        private readonly WaitForSeconds WAIT_05F = new WaitForSeconds(0.5f);
        private readonly WaitForSeconds WAIT_1F = new WaitForSeconds(1f);
    }
}