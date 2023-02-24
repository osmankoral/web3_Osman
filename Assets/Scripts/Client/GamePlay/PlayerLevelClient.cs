using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;
using Server.GamePlay;

namespace Client.GamePlay
{
    public class PlayerLevelClient : Singleton<PlayerLevelClient>
    {
        private PlayerLevelServer _server = new PlayerLevelServer();

        private void OnEnable()
        {
            GameManager.ExpGain.AddListener(() => ExprienceGain());
        }

        private void OnDisable()
        {
            GameManager.ExpGain.RemoveAllListeners();
        }

        public void ExprienceGain()
        {
            _server.PlayerGainExperience(1);
        }

    }
}
