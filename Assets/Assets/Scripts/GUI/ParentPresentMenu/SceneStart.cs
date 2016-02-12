using UnityEngine;
using System.Collections;

namespace ParentPresentMenu
{
    public class SceneStart : MonoBehaviour
    {
        public AdultPresent adultPresentCanvas;

        private void Start()
        {
            GUIInitialization.Initialize();
            StartCoroutine(adultPresentCanvas.DelayShowCanvas());
        }
    }
}