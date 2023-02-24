using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Database;


namespace Server.GamePlay
{
    public class PlayerLevelServer
    {
        private int _maxExperience = 4;

        public void PlayerGainExperience(int experience)
        {
            int tempExp = PlayerDatabase.Instance.PlayerHasExperienceGet() + experience;

            if(PlayerDatabase.Instance.PlayerHasExperienceGet() + experience >= _maxExperience)
            {
                PlayerDatabase.Instance.PlayerLevelSet(PlayerDatabase.Instance.PlayerLevelGet() + 1);
                PlayerDatabase.Instance.PlayerHasExperienceSet(tempExp - _maxExperience);
            }

            else
            {
                PlayerDatabase.Instance.PlayerHasExperienceSet(tempExp);

            }
        }
    }
}
