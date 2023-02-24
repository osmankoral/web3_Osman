using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;

namespace Database
{
    public class PlayerDatabase : Singleton<PlayerDatabase>
    {
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

    }
}
