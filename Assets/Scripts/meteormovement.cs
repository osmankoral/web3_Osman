using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client.GamePlay;

public class meteormovement : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("on trigger...");
        if (other.transform.parent.TryGetComponent<EnemyPuppet>(out EnemyPuppet enemy))
        {
            enemy.Damage();
            Destroy(gameObject, 1f);
        }
    }
}
