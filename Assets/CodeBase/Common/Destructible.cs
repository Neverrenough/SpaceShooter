using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class Destructible : Entity
    {
        #region Properties

        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        [SerializeField] private int m_Hitpoints;

        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;
        public int MaxHitPoints;

        public Vector3 Velocity { get; set; }
        #endregion

        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_Hitpoints;
            MaxHitPoints = m_CurrentHitPoints;
            transform.SetParent(null);
        }


        #endregion

        #region Public API

        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;
            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0) OnDeath();
        }

        protected virtual void OnDeath()
        {
            Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        private static HashSet<Destructible> m_AllDestructibles;
        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null) m_AllDestructibles = new HashSet<Destructible>();
            m_AllDestructibles.Add(this);
        }
        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }
        public const int TeamIdNeutral = 0;
        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;  

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        #endregion

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
        
    }
}

