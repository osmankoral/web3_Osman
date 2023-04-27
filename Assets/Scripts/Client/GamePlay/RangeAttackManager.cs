using Client.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackManager : MonoBehaviour
{
    public GameObject Enemy;

    void Update()
    {
        if(Enemy != null)
        {
            Vector3 enemyPos = Enemy.transform.position;
            enemyPos.y = transform.position.y;
            transform.LookAt(enemyPos);
            transform.position += transform.forward * 3f * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.gameObject == Enemy)
        {
            Enemy.TryGetComponent<EnemyPuppet>(out EnemyPuppet puppet);
            puppet.Damage();
            Destroy(gameObject);
        }
    }
}
