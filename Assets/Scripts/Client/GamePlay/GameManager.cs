using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework.Core;
using UnityEngine.Events;

namespace Client.GamePlay
{

    public class GameManager : Singleton<GameManager>
    {
        public bool IsGameReady;

        public string den = "dada";
        [HideInInspector] public static UnityEvent ExpGain = new();

        [SerializeField] private int _heroCount;
        [SerializeField] private int _enemyCount;

        [SerializeField] public int[] _selectedHeroes = new int[5];
        [SerializeField] public int[] _selectedEnemy = new int[5];

        public void SceneChange(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void SetHeroCount(int heroCount)
        {
            
            _heroCount = (heroCount > 5) ? 5 : heroCount;
        }

        public void SetEnemyCount(int enemyCount)
        {
            _enemyCount = (enemyCount > 5) ? 5 : enemyCount;
        }    
        
        public int GetHeroCount()
        {
            return _heroCount;
        }
        
        public int GetEnemyCount()
        {
            return _enemyCount;
        }
    }
}
