using Database;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Client.UI.MenuUI
{
    public class HeroesUI : MonoBehaviour
    {
        private void OnEnable()
        {
            int[] arr = PlayerDatabase.Instance.GetPlayerHasHeroes();
            for(int i=0; i< arr.Length; i++)
            {
                Debug.Log(arr[i]);
            }
        }

    }
}