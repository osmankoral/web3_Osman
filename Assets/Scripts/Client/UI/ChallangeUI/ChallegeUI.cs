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
        [SerializeField] private Button[] _attackButton;
        [SerializeField] private Image _heroMark;

        [SerializeField] private CombatManagerClient _combatManager;

        private void OnEnable()
        {
            _combatManager.buttonActive += AttackButtonSet;
            _combatManager.battleEnd += PanelChange;
            _combatManager.skillSet += SkillSet;
        }

        private void OnDisable()
        {
            _combatManager.buttonActive -= AttackButtonSet;
            _combatManager.battleEnd -= PanelChange;
            _combatManager.skillSet -= SkillSet;
            
        }

        private void AttackButtonSet(bool isActive,int whichButton)
        {
            foreach(var button in _attackButton)
            {
                button.gameObject.SetActive(false);
            }
            _attackButton[whichButton].gameObject.SetActive(isActive);
            _heroMark.gameObject.SetActive(isActive);
        }
        private void SkillSet(int skillCount, Sprite[] a)
        {
            for(int i=0; i<skillCount; i++)
            {
                _attackButton[i].gameObject.SetActive(true);
                _attackButton[i].GetComponent<Image>().sprite = a[i];
            }
        }

        public void Attack(int wichSkill)
        {
            _combatManager.player_Action(wichSkill);
        }

        public void VictoryButton()
        {
            GameManager.ExpGain.Invoke();
            PlayerLevelClient.Instance.GoldGain(GameManager.Instance.GetEnemyCount());
            GameManager.Instance.SceneChange("MenuScene");
            
        }

        public void LoseButton()
        {
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
