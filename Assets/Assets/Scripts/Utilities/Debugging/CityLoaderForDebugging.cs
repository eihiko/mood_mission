using Globals;
using UnityEngine;

public class CityLoaderForDebugging : MonoBehaviour {

	void Start ()
	{
        if (CityInitializer.City == null) 
            Application.LoadLevelAdditive("SmallCityForDebugging");
	}
}
