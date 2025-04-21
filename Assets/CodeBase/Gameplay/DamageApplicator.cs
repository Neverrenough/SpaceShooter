using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class DamageApplicator : MonoBehaviour
    {
        public static string IngoreTag = "WorldBoundry";
        [SerializeField] private float m_VelocityDamageModifier;
        [SerializeField] private float m_DamageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == IngoreTag) return;

            var destructible = transform.root.GetComponent<Destructible>();

            if(destructible != null)
            {
                destructible.ApplyDamage((int)m_DamageConstant + (int)(m_VelocityDamageModifier * collision.relativeVelocity.magnitude));
            }
        }
    }
}
