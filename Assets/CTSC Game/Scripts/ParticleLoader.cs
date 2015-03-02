using UnityEngine;
using System.Collections;

public class ParticleLoader : MonoBehaviour {
	
	public Transform particleEffects;
	public float particleRange = 4f;
	private Transform player;
	
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		if (player != null) {
			Debug.Log("Found player object!");
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		turnOnEffectsInRange ();
	}

	void turnOnEffectsInRange(){
		bool inRange = false;
		ParticleSystem particleSystem = null;
		foreach (Transform p in particleEffects){
			inRange = isInRange(p);
			particleSystem = p.GetComponent<ParticleSystem>();
			if (particleSystem != null){
 				if (inRange && !particleSystem.isPlaying){
					particleSystem.Play();
				} else if (!inRange && particleSystem.isPlaying){
					particleSystem.Stop();
				}
			} else {
				if(inRange && p.tag == "Lights") {
					p.gameObject.SetActive(true);
				} else if (!inRange && p.gameObject.activeSelf){
					p.gameObject.SetActive(false);
				}
			}
		}
	}

	bool isInRange(Transform p){
		Vector3 header = p.position - player.position;
		// Determine if target is within range.
		return (header.sqrMagnitude < particleRange * particleRange);
	}
}
