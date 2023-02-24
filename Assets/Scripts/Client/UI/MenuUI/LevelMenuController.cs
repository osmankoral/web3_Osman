using System.Collections;
using UnityEngine;
using Client.GamePlay;

namespace Client.UI.MenuUI
{
    public class LevelMenuController : MonoBehaviour
    {
        public void SetEnemyCount(int count)
        {
            GameManager.Instance.SetEnemyCount(count);
        }
        
    }
}