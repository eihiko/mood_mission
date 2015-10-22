using UnityEngine;
using System.Collections;

public class HydraTarget : MonoBehaviour {

	public AudioClip firehit;
	public AudioClip shieldhit;
	public GameObject shield;
	public int complimentScrolls;
	public int torkanaScrolls;
	public Hydra hydra;
	public MissionManager manager;

	private KeyCode[] scrollUsed;
	
	void Start(){
		shield.SetActive(false);
		scrollUsed = new KeyCode[hydra.numThoughts];
		scrollUsed [0] = KeyCode.Alpha1;
		scrollUsed [1] = KeyCode.Alpha2;
		if (hydra.numThoughts == 4) {
			scrollUsed[2] = KeyCode.Alpha3;
			scrollUsed[3] = KeyCode.Alpha4;
		}
	}
	
	void Update(){
		if(Input.GetKey(KeyCode.Space)){
			shield.SetActive (true);
		}
		else{
			shield.SetActive(false);
		}
	}
	
	void OnCollisionEnter(Collision c){
		Hydraball ball = c.gameObject.GetComponent<Hydraball>();
		if(null != ball){
			switch (ball.type){
				case Hydra.FIREBALL:
					FireballHit();
					break;
				case Hydra.NEGBALL:
					NegballHit(ball);
					break;
				case Hydra.POSBALL:
					PosballHit();
					break;
				default:
					Debug.LogError ("Error: Undefined hydraball!");
					break;
			}
		}
	}

	public void setNumberOfComplimentScrolls(int scrolls){
		complimentScrolls = scrolls;
	}
	
	private void FireballHit(){
		AudioSource.PlayClipAtPoint(firehit, transform.position);
		if(Input.GetKey(KeyCode.Space)){
			AudioSource.PlayClipAtPoint(shieldhit, transform.position);
		}
	}
	
	private void NegballHit(Hydraball ball){
		if (!manager.hydraHit1.isSet ()) {
			manager.hydraHit1.setFlag ();
		} else if (!manager.hydraHit2.isSet ()) {
			manager.hydraHit2.setFlag();
		}
		int thought = ball.thought;
		if (Input.GetKey(this.scrollUsed[thought])){
			hydra.DecrementHits(thought);
		}
	}
	
	private void PosballHit(){
	
	}
}
