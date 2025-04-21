using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{


    public class Player : SingltonBase<Player>
    {
        public static SpaceShip SelectedSpaceShip;

        [SerializeField] private int m_NumLives;
        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        public SpaceShip ActiveShip => m_Ship;



        private CameraController m_CameraController;
        private ShipInputController m_MovementController;
        private Transform m_SpawnPoint;

        public CameraController CameraController => m_CameraController; 
        public void Construct(CameraController cameraController, ShipInputController shipInputController, Transform spawnPoint)
        {
            m_CameraController = cameraController;
            m_MovementController = shipInputController;
            m_SpawnPoint = spawnPoint;
        }


        private SpaceShip m_Ship;

        private int m_Score;
        private int m_NumKills;

        public int Score => m_Score;
        public int NumKills => m_NumKills;
        public int NumLives => m_NumLives;

        public SpaceShip shipPrefab
        {
            get
            {
                if(SelectedSpaceShip == null)
                {
                    return m_PlayerShipPrefab;
                }
                else
                {
                    return SelectedSpaceShip;
                }
            }
        }

        private void Start()
        {
            Respawn();
        }
        private void OnShopDeath()
        {
            m_NumLives--;
            if (m_NumLives > 0) Respawn();
        }
        private void Respawn()
        {
            var newPlayerShip = Instantiate(shipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            m_CameraController.SetTarget(m_Ship.transform);
            m_MovementController.SetTargetShip(m_Ship);
        }

        public void AddKill()
        {
            m_NumKills += 1;
        }
        public void AddScore(int num)
        {
            m_Score += num;
        }
    }
}
