using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [SerializeField] private LevelProperties m_LevelProperties;
        [SerializeField] private Text m_LevelTitle;
        [SerializeField] private Image m_PreviewImage;
        private void Start()
        {
            if (m_LevelProperties == null) return;
            m_PreviewImage.sprite = m_LevelProperties.PreviewImage;
            m_LevelTitle.text = m_LevelProperties.Title;
        }
        public void LoadLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }
    }
}
