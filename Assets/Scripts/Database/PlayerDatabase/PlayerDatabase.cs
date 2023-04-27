using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;
using System.Linq;

namespace Database
{
    public class PlayerDatabase : Singleton<PlayerDatabase>
    {
        [SerializeField] private List<int> _playersHero = new List<int>();


        public int PlayerLevelGet()
        {
            return PlayerPrefs.GetInt("PlayerLevelDB", 1);
        }

        public void PlayerLevelSet(int playerLevel)
        {
            PlayerPrefs.SetInt("PlayerLevelDB", playerLevel);
            
        }

        public int PlayerHasExperienceGet()
        {
            return PlayerPrefs.GetInt("PlayerExperienceDB", 0);
        }

        public void PlayerHasExperienceSet (int playerExperience)
        {
            PlayerPrefs.SetInt("PlayerExperienceDB", playerExperience);
        }

        public int GetPlayerHeroAmount()
        {
            return PlayerPrefs.GetInt("PlayerHeroAmount", 0);
        }

        public void IncreseHeroAmount(int heroAmount, int whichHero)
        {
            PlayerPrefs.SetInt("PlayerHeroAmount", PlayerPrefs.GetInt("PlayerHeroAmount") + heroAmount);
            IncreseHero(whichHero);
        }
        
        public void DicreseHeroAmount(int heroAmount)
        {
            PlayerPrefs.SetInt("PlayerHeroAmount", PlayerPrefs.GetInt("PlayerHeroAmount") - heroAmount);
        }

        public int[] GetPlayerHasHeroes()
        {
            PlayerHasHeroes(0);
            int[] tempHeroesArray = _playersHero.ToArray();
            return tempHeroesArray;
        }
        private void PlayerHasHeroes(int num)
        {
            _playersHero.Clear();
            int tempAmount = GetPlayerHeroAmount() - num;
            for (int i = 0; i < tempAmount; i++)
            {
                _playersHero.Add(PlayerPrefs.GetInt("Player1Hero"+ i.ToString()));
            }
        }

        private void IncreseHero(int selected)
        {
           if (GetPlayerHeroAmount() > 1)
                PlayerHasHeroes(1);

            _playersHero.Add(selected);
            PlayerPrefs.SetInt("Player1Hero" + (_playersHero.Count - 1).ToString(), _playersHero[_playersHero.Count - 1]);

        }

        public void DecreseHero(int select)
        {
            _playersHero.RemoveAt(select);
            PlayerHasHeroes(1);
        }


    }
}
