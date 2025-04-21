using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class ShipInputController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }

        [SerializeField] private ControlMode m_ControlMode;

        public  void Construct(VirtualGamePad virtualGame)
        {
            m_VirtualCamePad = virtualGame;
        }
        private SpaceShip m_TargetShip;
        private VirtualGamePad m_VirtualCamePad;
        private void Start()
        {
            if (m_ControlMode == ControlMode.Keyboard)
            {
                m_VirtualCamePad.VirtualJoystick.gameObject.SetActive(false);
                m_VirtualCamePad.MobileFirePrimary.gameObject.SetActive(false);
                m_VirtualCamePad.MobileFireSecondary.gameObject.SetActive(false);

            }
            else
            {
                m_VirtualCamePad.VirtualJoystick.gameObject.SetActive(true);
                m_VirtualCamePad.MobileFirePrimary.gameObject.SetActive(true);
                m_VirtualCamePad.MobileFireSecondary.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (m_TargetShip == null) return;

            if (m_ControlMode == ControlMode.Keyboard) ControlKeyboard();
            if (m_ControlMode == ControlMode.Mobile) ControlMobile();
        }
        private void ControlMobile()
        {
            Vector3 dir = m_VirtualCamePad.VirtualJoystick.Value;

            var dot = Vector2.Dot(dir, m_TargetShip.transform.up);
            var dot2 = Vector2.Dot(dir, m_TargetShip.transform.right);

            if (m_VirtualCamePad.MobileFirePrimary.IsHold == true) m_TargetShip.Fire(TurretMode.Primary);
            if (m_VirtualCamePad.MobileFireSecondary.IsHold == true) m_TargetShip.Fire(TurretMode.Secondary);

            m_TargetShip.ThrustControl = Mathf.Max(0, dot);
            m_TargetShip.TorqueControl = -dot2;
        }
        private void ControlKeyboard()
        {
            float thrust = 0;
            float torque = 0;

            if (Input.GetKey(KeyCode.UpArrow)) thrust = 1.0f;
            if (Input.GetKey(KeyCode.DownArrow)) thrust = -1.0f;
            if (Input.GetKey(KeyCode.LeftArrow)) torque = 1.0f;
            if (Input.GetKey(KeyCode.RightArrow)) torque = -1.0f;

            if (Input.GetKey(KeyCode.Space)) m_TargetShip.Fire(TurretMode.Primary);
            if (Input.GetKey(KeyCode.X)) m_TargetShip.Fire(TurretMode.Secondary);

            m_TargetShip.ThrustControl = thrust;    
            m_TargetShip.TorqueControl = torque;
        }
        public void SetTargetShip(SpaceShip ship)
        {
            m_TargetShip = ship;
        }
    }
}
