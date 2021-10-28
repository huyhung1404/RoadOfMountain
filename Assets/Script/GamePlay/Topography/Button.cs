using UnityEngine;

namespace Script.GamePlay.Topography
{
    public class Button : MonoBehaviour
    {
        [SerializeField] private MapMoving[] m_MapMoving;
        private void OnTriggerEnter(Collider other)
        {
            foreach (MapMoving map in m_MapMoving)
            {
                map.Move();
            }
        }
    }
}