using UnityEngine;

namespace SpaceShooter
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;
        [SerializeField] private float m_InterpolationLinear;
        [SerializeField] private float m_InterpolationAngular;
        [SerializeField] private float m_CameraZOffset;
        [SerializeField] private float m_ForwardOffset;

        private void FixedUpdate()
        {
            if (m_Target == null) return;
            Vector2 camPos = transform.position;
            Vector2 targetPos = m_Target.position + m_Target.transform.up * m_ForwardOffset;

            Vector2 newCamPos = Vector2.Lerp(camPos, targetPos, m_InterpolationLinear * Time.deltaTime);

            transform.position = new Vector3(newCamPos.x, newCamPos.y, m_CameraZOffset);

            if(m_InterpolationAngular > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, m_Target.rotation, m_InterpolationAngular * Time.deltaTime);
            }
        }
        public void SetTarget(Transform newTarget)
        {
            m_Target = newTarget;
        }
    }
}
