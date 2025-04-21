using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol
        }
        [SerializeField] private AIBehaviour m_AIBehaviour;
        [SerializeField] private AIPointPatrol m_PatrolPoint;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;
        [SerializeField] private float m_RandomSelectMovePointTime;
        [SerializeField] private float m_FindNewTargetTime;
        [SerializeField] private float m_ShootDelay;
        [SerializeField] private float m_EvadeRayLength;
        [SerializeField] public Transform[] m_PatrolTargets;
        [SerializeField] private float m_ProjectileSpeed;

        private float m_PatrolPointThreshold = 1.0f;

        private int m_CurrentPatrolPointIndex = 0;

        private SpaceShip m_SpaceShip;
        private Vector3 m_MovePosition;
        private Destructible m_SelectedTarget;
        private Timer m_RandomizerDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitTimers();
        }
        private void Update()
        {
            UpdateTimers();
            UpdateAI();
        }
        private void UpdateAI()
        {
            if(m_AIBehaviour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }
        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }
        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                if (m_SelectedTarget != null)
                {
                    Vector3 leadPosition = MakeLead(m_SelectedTarget.transform.position, m_SelectedTarget.Velocity, m_ProjectileSpeed);
                    m_MovePosition = leadPosition;
                }
                else
                {
                    if (m_PatrolPoint != null && m_PatrolTargets.Length <= 0)
                    {
                        bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                        if (isInsidePatrolZone == true)
                        {
                            if (m_RandomizerDirectionTimer.isFinished == true)
                            {
                                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;

                                m_MovePosition = newPoint;

                                m_RandomizerDirectionTimer.Start(m_RandomSelectMovePointTime);
                            }
                        }
                        else
                        {
                            m_MovePosition = m_PatrolPoint.transform.position;
                        }
                    }
                    else
                    {
                        if (m_PatrolTargets != null && m_PatrolTargets.Length > 0)
                        {
                            float distanceToCurrentPoint = Vector3.Distance(transform.position, m_PatrolTargets[m_CurrentPatrolPointIndex].position);

                            if (distanceToCurrentPoint < m_PatrolPointThreshold)
                            {
                                m_CurrentPatrolPointIndex = (m_CurrentPatrolPointIndex + 1) % m_PatrolTargets.Length;
                            }

                            m_MovePosition = m_PatrolTargets[m_CurrentPatrolPointIndex].position;
                        }
                    }
                }
            }
        }
        private Vector3 MakeLead(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
        {
            Vector3 relativePosition = targetPosition - transform.position;
            float distance = relativePosition.magnitude;
            float timeToTarget = distance / projectileSpeed;

            Vector3 futurePosition = targetPosition + targetVelocity * timeToTarget;
            return futurePosition;
        }

        private void ActionEvadeCollision()
        {
            if(Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + transform.right * 100.0f;
            }
        }
        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;
            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }
        private const float MAX_ANGLE = 45.0f;
        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);
            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);
            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;
            return -angle;
        }
        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.isFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.Start(m_ShootDelay);
            }
        }
        private void ActionFire()
        {
            if(m_SelectedTarget != null)
            {             
                if(m_FireTimer.isFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);
                    m_FindNewTargetTimer.Start(m_ShootDelay);
                }
            }
        }
        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = float.MaxValue;
            Destructible potentialTarget = null;

            foreach(var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;
                if (v.TeamId == Destructible.TeamIdNeutral) continue;
                if (v.TeamId == m_SpaceShip.TeamId) continue;
                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);
                if(dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }

        private void InitTimers()
        {
            m_RandomizerDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
            
        }
        private void UpdateTimers()
        {
            m_RandomizerDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }
        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;
            m_PatrolPoint = point;
        }
    }
}
