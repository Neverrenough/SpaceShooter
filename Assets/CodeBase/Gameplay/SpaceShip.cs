using Common;
using UnityEngine;


namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [SerializeField] private Sprite m_PreviewImage;

        [Header("Space ship")]
        [SerializeField] private float m_Mass;
        [SerializeField] private float m_Thrust;
        [SerializeField] private float m_Mobility;
        [SerializeField] private float m_MaxLinearVelocity;
        [SerializeField] private float m_MaxAngularVelocity;
        private Rigidbody2D m_Rigid;

        public float MaxLinearVelocity => m_MaxLinearVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;
        public Sprite PreviewImage => m_PreviewImage;

        private float m_Timer;
        [SerializeField] private float m_TimeActive;

        private bool NewSpeed = false;

        #region Public API

        public float ThrustControl { get; set; }
        
        public float TorqueControl { get; set; }

        #endregion

        #region Unity Events

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            InitOffensive();
        }
        private void Update()
        {
            if (NewSpeed == true)
            {
                m_Timer += Time.deltaTime;
                if (m_Timer > m_TimeActive)
                {
                    m_MaxLinearVelocity = 15;
                    NewSpeed = false;
                }       
            }
        }
        private void FixedUpdate()
        {
            UpdateRigidBody();

            

            UpdateEnergyRegen();
        }

        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(m_Thrust * ThrustControl * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        #endregion

        [SerializeField] private Turret[] m_Turrets;

        public void Fire(TurretMode mode)
        {
            for(int i = 0; i< m_Turrets.Length; i++)
            {
                if(m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private float m_LinearVelocity;
        [SerializeField] private int m_EnergyRegenPerSecond;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }
        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }
        public void AddSpeed(float speed)
        {
            m_MaxLinearVelocity += speed;
            NewSpeed = true;
        }
        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
           
        }
        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        public bool DrawEnergy(int count)
        {
            if (count == 0) return true;

            if (m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }
            return false;
        }

        public bool DrawAmmo(int count)
        {
            if (count == 0) return true;

            if(m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }
            return false;
        }
        public void AssignWeapon(TurretProperties props)
        {
            for(int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }
    }
}

