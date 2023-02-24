using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client.GamePlay;
using Database;
using TMPro;

namespace Client.UI.MenuUI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerLevelText;
        [SerializeField] private TextMeshProUGUI _playerExperienceText;


        private void Start()
        {
            PlayerLevelText();
            PlayerExperienceText();
        }

        private void OnEnable()
        {
            GameManager.ExpGain.AddListener(() => PlayerDetailText()); 
        }
        private void PlayerLevelText()
        {
            _playerLevelText.text = PlayerDatabase.Instance.PlayerLevelGet().ToString();
        }
        private void PlayerExperienceText()
        {
            _playerExperienceText.text = PlayerDatabase.Instance.PlayerHasExperienceGet().ToString();
        }

        public void PlayerDetailText()
        {
            PlayerLevelText();
            PlayerExperienceText();
        }
    }
}
