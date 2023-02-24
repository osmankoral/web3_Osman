using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client.GamePlay;

namespace Client.Client.UI.ChallengeUI
{
    public class ChallegeUI : MonoBehaviour
    {
        [SerializeField] private GameObject _losePanale;
        [SerializeField] private GameObject _winPanale;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Image _heroMark;

        [SerializeField] private CombatManagerClient _combatManager;

        private void OnEnable()
        {
            _combatManager.buttonActive += AttackButtonSet;
            _combatManager.battleEnd += PanelChange;
        }

        private void OnDisable()
        {
            _combatManager.buttonActive -= AttackButtonSet;
            _combatManager.battleEnd -= PanelChange;
            
        }

        private void AttackButtonSet(bool isActive, Vector3 position)
        {
            _attackButton.gameObject.SetActive(isActive);
            _heroMark.gameObject.SetActive(isActive);

            _heroMark.transform.position = position;
        }

        public void Attack()
        {
            CombatManagerClient.Player_Action.Invoke();
        }

        public void MenuSceneButton()
        {
            PlayerLevelClient.Instance.ExprienceGain();
            GameManager.Instance.SceneChange("MenuScene");
            
        }

        private void PanelChange(bool isACtive)
        {
            if(isACtive)
            {
                _winPanale.gameObject.SetActive(true);
            }

            else 
            {
                _losePanale.gameObject.SetActive(true);
            }
        }

    }
}
