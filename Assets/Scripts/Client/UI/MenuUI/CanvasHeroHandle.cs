using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Client.UI.MenuUI
{
    public class CanvasHeroHandle : MonoBehaviour
    {
        public int Index;
        public int child;
        public GameObject ThisGameObject;
        
        void Start()
        {
            ThisGameObject = gameObject;
        }

        
    }
}