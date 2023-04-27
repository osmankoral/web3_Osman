using Client.GamePlay;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Client.GamePlay
{
    public class CombatReady : MonoBehaviour
    {
        CombatManagerClient _combat;

        void Start()
        {
            _combat = FindObjectOfType<CombatManagerClient>();
            StartCoroutine(Loader());
        }

        IEnumerator Loader()
        {
            yield return new WaitForSeconds(1f);
            _combat.ReadyGameStart();
        }

    }
}