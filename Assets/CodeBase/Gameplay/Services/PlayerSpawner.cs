using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private CameraController m_CameraControllerPrefab;
        [SerializeField] private Player m_PlayerPrefab;
        [SerializeField] private ShipInputController m_ShipInputControllerPrefab;
        [SerializeField] private VirtualGamePad m_VirtualGamePadPrefab;

        [SerializeField] private Transform m_SpawnPoint;

        public Player Spawn()
        {
            CameraController cameraController = Instantiate(m_CameraControllerPrefab);
            VirtualGamePad virtualGamePad = Instantiate(m_VirtualGamePadPrefab);

            ShipInputController shipInputController = Instantiate(m_ShipInputControllerPrefab);
            shipInputController.Construct(virtualGamePad);

            Player player = Instantiate(m_PlayerPrefab);
            player.Construct(cameraController, shipInputController, m_SpawnPoint);

            return player;
        }
    }
}
