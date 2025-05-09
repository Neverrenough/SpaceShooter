using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class Projectile : ProjectileBase
    {
        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;
        protected override void OnHit(Destructible destructible)
        {
            if (m_Parent == Player.Instance.ActiveShip)
            {
                Player.Instance.AddScore(destructible.ScoreValue);
                if (destructible is SpaceShip)
                {
                    Player.Instance.AddKill();
                }
            }
        
        }
        protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            if (m_ImpactEffectPrefab != null)
                Instantiate(m_ImpactEffectPrefab, pos, Quaternion.identity);

            Destroy(gameObject, 0);
        }
    }
}
