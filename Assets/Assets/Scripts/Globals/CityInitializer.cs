using System.Collections;
using UnityEngine;

namespace Globals
{
    public class CityInitializer : MonoBehaviour
    {
        public static GameObject City;
        public bool ShouldDisable = true;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            City = gameObject;
            if (!ShouldDisable) return;
            StartCoroutine(DelayDisable());
        }

        private IEnumerator DelayDisable()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}