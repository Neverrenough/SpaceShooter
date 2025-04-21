using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{


    public class PowerUpStats : PowerUp
    {
        public enum EffectType
        {
            AddAmmo,
            AddEnergy,
            SetInDestructible,
            AddSpeed
        }
        [SerializeField] private EffectType m_EffectType;
        [SerializeField] private float m_Value;
        protected override void OnPuckedUp(SpaceShip ship)
        {
            if (m_EffectType == EffectType.AddEnergy) ship.AddEnergy((int)m_Value);
            if (m_EffectType == EffectType.AddAmmo) ship.AddAmmo((int)m_Value);
            if (m_EffectType == EffectType.AddSpeed) ship.AddSpeed(m_Value);
        }
    }
}
