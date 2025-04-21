using UnityEngine;

namespace Common
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] private string m_Nickname;
        public string Nickname => m_Nickname;
    }
}
