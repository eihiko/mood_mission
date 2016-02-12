using System.Collections;
using UnityEngine;

namespace Globals
{
    public class CityPreloader : MonoBehaviour
    {
        private void Start()
        {
            if (CityInitializer.City == null)
                Application.LoadLevelAdditiveAsync("SmallCity");
            else
            {
                CityInitializer.City.SetActive(false);
            }
        }
    }
}