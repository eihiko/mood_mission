using UnityEngine;
using System.Collections;

public class Hydra : MonoBehaviour {

	public enum Monster{
		Hydra,
		Dragon
	}

	public Monster mon;
	private StoredBool trigger;

	public Transform target;
	public MissionManager mm;
	
	public int hitsToDefeat;
	public int numThoughts;
	private int[] hitsLeftPerThought;
	
	public float ballSpeed;
	public float ballDelay;

	public bool isFiring;
	
	public GameObject fireball;
	public GameObject posball;
	public GameObject negball;
	
    public Transform firelauncher;
	public Transform poslauncher;
	public Transform neglauncher;
	
	public AudioClip fireSound;
	public AudioClip posSound;
	public AudioClip negSound;

	public AudioClip [] negativeThoughts;
	
    public const int FIREBALL = 0;
	public const int POSBALL = 1;
	public const int NEGBALL = 2;

	public bool[] trumped;
	public int numTrumped;
	
	private AudioClip [] launchSounds;
	
	private GameObject [] balls;

	private Transform [] launchers;
	
	private float launchTimer = 0;
	
	private Animator a;

	// Use this for initialization
	void Start () {
		switch (mon) {
		case Monster.Hydra:
			trigger = mm.hydraDefeated;
			break;
		case Monster.Dragon:
			trigger = mm.dragonDefeated;
			break;
		default:
			trigger = mm.hydraDefeated;
			break;
		}
		isFiring = false;
		hitsLeftPerThought = new int[numThoughts];
		trumped = new bool[numThoughts];
		numTrumped = 0;
		for (int i=0; i<hitsLeftPerThought.Length; i++) {
			hitsLeftPerThought [i] = hitsToDefeat;
			trumped[i] = false;
		}
	
		balls = new GameObject[]{fireball, posball, negball};
		launchers = new Transform[]{firelauncher, poslauncher, neglauncher};
		launchSounds = new AudioClip[]{fireSound, posSound, negSound};
		
		a = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		//If all of the thoughts have been defeated thoroughly, set the monster inactive and set hydraDefeated to true
		if (numTrumped == trumped.Length) {
			trigger.setFlag();
			this.gameObject.SetActive (false);
		}
	
		if (launchTimer < ballDelay){
			launchTimer += Time.deltaTime;
		}
		else if (isFiring==true){
			launchTimer = 0;
			a.SetTrigger ("Launch");
			if (mon==Monster.Dragon){
				GameObject Head = GameObject.Find ("mountain_dragon_ Head");
				Head.transform.LookAt(target.transform.position);
			}
			else
				this.transform.LookAt(target.transform.position);
		}
		
	
	}
	
	public void LaunchRandom(){
		int balltype = Random.Range (0,balls.Length);
		switch (balltype){
			case Hydra.FIREBALL:
				Launch (Hydra.FIREBALL);
				break;
			case Hydra.POSBALL:
				Launch (Hydra.POSBALL);
				break;
			case Hydra.NEGBALL:
				Launch (Hydra.NEGBALL);
				break;
			default:
				Debug.LogError ("Error: Undefined hydraball!");
				break;
		}
	}
	
	public void Launch(int balltype){
		GameObject ball = Instantiate (balls[balltype]);
		Hydraball hydraball = ball.gameObject.GetComponent<Hydraball> ();
		hydraball.type = balltype;
		ball.transform.position = launchers[balltype].position;
		ball.transform.LookAt (target.transform.position);
		ball.GetComponent<Rigidbody>().AddForce (ball.transform.forward * ballSpeed, ForceMode.VelocityChange);
		AudioSource.PlayClipAtPoint(launchSounds[balltype], ball.transform.position);
		if (balltype == Hydra.NEGBALL) {
			int thought=0;
			bool thoughtFound=false;
			while(!thoughtFound){
				thought = Random.Range (0,this.negativeThoughts.Length);
				if (!trumped[thought]){
					thoughtFound = true;
				}
			}
			hydraball.thought = thought;
			AudioSource.PlayClipAtPoint (this.negativeThoughts [thought], ball.transform.position);
		} else {
			hydraball.thought = -1;
		}
	}

	public void DecrementHits(int thoughtToDecrement){
		hitsLeftPerThought [thoughtToDecrement]--;
		if (hitsLeftPerThought [thoughtToDecrement] <= 0) {
			trumped[thoughtToDecrement]=true;
			numTrumped++;
		}
	}
}
