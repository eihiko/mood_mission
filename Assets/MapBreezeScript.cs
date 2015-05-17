using UnityEngine;
using System.Collections;

public class MapBreezeScript : MonoBehaviour {

	public GameObject map;
	bool hasTriggered;
	// Use this for initialization
	void Start () {
		hasTriggered = false;
	}


//	IEnumerator AnimWait()
//	{
//		yield return new WaitForSeconds (9.5f);
//	}

	// Update is called once per frame
	void Update () {
		if(!map.GetComponent<Animation>().IsPlaying("MapBreeze"))
		{
			map.SetActive(false);
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		Debug.Log ("Entered ontriggerenter!");
		if(!hasTriggered && other.tag == "Player") 
		{
			Debug.Log("Starting animations");
			map.SetActive(true);
			map.GetComponent<Animation>().Play();
			hasTriggered = true;
		}
	}
}
