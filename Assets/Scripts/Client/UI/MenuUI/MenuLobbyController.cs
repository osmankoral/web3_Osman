using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Client.GamePlay;
using UnityEngine.Events;
using Server.GamePlay;
using Database;
using System.Collections.Generic;
using Assets.Scripts.Client.UI.MenuUI;

namespace Client.UI.MenuUI
{
    public class MenuLobbyController : MonoBehaviour
    {

        [SerializeField] private Image[] _heroHive;

        [SerializeField] private Image[] _hero;
        [SerializeField] private GameObject[] _heroesPrefabs;
        [SerializeField] private GameObject _heroesPanel;
        [SerializeField] private GameObject _emptyHeroesButton;

        private int[] _selectedHeroes = new int[5];
        private int _currentButton;

        private void Start()
        {
           
        }

        public void HasHeroList()
        {
            for(int j = 0; j< _heroesPanel.transform.childCount; j++)
            {
                Destroy(_heroesPanel.transform.GetChild(j).gameObject);
            }

             
            GameObject empty = Instantiate(_emptyHeroesButton, _heroesPanel.transform);
            empty.GetComponent<Button>().onClick.AddListener(() => HeroSelect(0, empty, 0));

            int listCount= PlayerDatabase.Instance.GetPlayerHeroAmount();
            int[] arr = PlayerDatabase.Instance.GetPlayerHasHeroes();

            bool isContinu = true;
            for(int i=0; i < listCount;  i++)
            {
                isContinu = true;
                for(int d=0; d < _selectedHeroes.Length; d++)
                {
                    if(i+1 == _selectedHeroes[d])
                    {
                        isContinu= false;
                        break;
                    }
                }
                if(isContinu)
                {

                    GameObject a = Instantiate(_heroesPrefabs[arr[i]], _heroesPanel.transform);
                    a.TryGetComponent<CanvasHeroHandle>(out CanvasHeroHandle chh);
                    chh.child = i+1;
                    a.GetComponent<Button>().onClick.AddListener(() => HeroSelect(chh.Index, chh.ThisGameObject, chh.child));
                }
            }


            /*for(int d=0; d < _selectedHeroes.Length; d++)
            {
                if (_selectedHeroes[d] != 0)
                { 
                    Destroy(_heroesPanel.transform.GetChild(_selectedHeroes[d]).gameObject);
                    Debug.Log(_selectedHeroes[d]);
                }
            }*/
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

        public void HeroSelect(int hero, GameObject obj, int index)
        {
            _heroHive[_currentButton - 1].color = obj.GetComponent<Image>().color;
            GameManager.Instance._selectedHeroes[_currentButton - 1] = hero;

                _selectedHeroes[_currentButton - 1] = index;

            _heroesPanel.SetActive(false);
        }

        public void SetEnemyCount(int number)
        {
            GameManager.Instance.SetEnemyCount(number);
        }
    }
}