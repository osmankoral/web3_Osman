using Database;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI.MenuUI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Image _sprite;
        [SerializeField] private TextMeshProUGUI _goldText;

        private void OnEnable()
        {
            _goldText.text = PlayerPrefs.GetInt("PlayerGold").ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerPrefs.SetInt("PlayerGold", 10);
                Debug.Log(PlayerPrefs.GetInt("PlayerGold"));
                _goldText.text = PlayerPrefs.GetInt("PlayerGold", 10).ToString();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                for(int i=0; i<4; i++)
                {
                    PlayerDatabase.Instance.IncreseHeroAmount(1, i);
                }

                for (int i = 0; i < 4; i++)
                {
                    PlayerDatabase.Instance.IncreseHeroAmount(1, i);
                }
            }
        }
        public void OpenEgg()
        {
            if(PlayerPrefs.GetInt("PlayerGold") > 9)
            {
                int randHero = Random.Range(0, 3);
                Debug.Log("Hero Kazandın");
                PlayerPrefs.SetInt("PlayerGold", PlayerPrefs.GetInt("PlayerGold") - 10);
                _goldText.text = PlayerPrefs.GetInt("PlayerGold").ToString();
                PlayerDatabase.Instance.IncreseHeroAmount(1, randHero);
            }

            else
            {
                Debug.Log("para yok");
            }
        }
    }
}