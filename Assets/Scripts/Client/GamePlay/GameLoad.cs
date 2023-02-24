using Client.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoad : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerPrefab;

    void Start()
    {
        if(!GameManager.Instance.IsGameReady)
        {
            Instantiate(gameManagerPrefab);
            GameManager.Instance.IsGameReady = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
