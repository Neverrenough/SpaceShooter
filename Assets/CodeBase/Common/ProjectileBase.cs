using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public abstract class ProjectileBase : Entity
    {
        [SerializeField] private float m_Velocity;
        [SerializeField] private float m_LifeTime;
        [SerializeField] private int m_Damage;
        
        private float m_Timer;
        protected Destructible m_Parent;

        protected virtual void OnHit(Destructible destructible) { }
        protected virtual void OnHit(Collider2D collider2D) { }
        protected virtual void OnProjectileLifeEnd(Collider2D col, Vector2 pos) { }

        private void Update()
        {
            float StepLenght = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * StepLenght;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, StepLenght);
            if (hit)
            {
                OnHit(hit.collider);
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();
                if(dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);

                    OnHit(dest);  
                }
                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_LifeTime) Destroy(gameObject);
                OnProjectileLifeEnd(hit.collider, hit.point);
        }      
        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
        }
    }
}
