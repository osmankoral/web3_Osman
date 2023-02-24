using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Client.GamePlay;
using UnityEngine.Events;
using Server.GamePlay;

namespace Client.UI.MenuUI
{
    public class MenuLobbyController : MonoBehaviour
    {

        [SerializeField] private Image[] _heroHive;

        [SerializeField] private Image[] _hero;


        private int _currentButton;

        private void Start()
        {
           
        }
        public void ChallengeSceneButton()
        {
            int heroCount = 0;
            for(int i= 0; i < GameManager.Instance._selectedHeroes.Length; i++)
            {
                if (GameManager.Instance._selectedHeroes[i] != 0)
                {
                    heroCount++;
                }
            }

            if (heroCount > 0)
            {
                GameManager.Instance.SetHeroCount(heroCount);
                GameManager.Instance.SceneChange("ChallengeScene");
            }

            else
                Debug.Log("Kahraman Seçiniz !!");
        }

        public void HeroHiveSelect(int number)
        {
            _currentButton = number;
        }

        public void HeroSelect(int hero)
        {
            _heroHive[_currentButton - 1].color = _hero[hero].color;
            GameManager.Instance._selectedHeroes[_currentButton - 1] = hero;
        }

        public void SetEnemyCount(int number)
        {
            GameManager.Instance.SetEnemyCount(number);
        }
    }
}