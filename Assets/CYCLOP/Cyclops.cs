using UnityEngine;
using System.Collections;

public class Cyclops : MonoBehaviour {

	public const int ATTACK = 0;
	public const int SIT = 1;
	public const int APPLES = 2;

	private Transform tree;
	private Transform cyclops;
	private Rigidbody cyclopsBody;
	public float walkSpeed;
	public float stopDistance;
	
	private Animator anim;
	
	private float inclination = 0;
	
	// Use this for initialization
	void Start () {
		tree = transform.Find("AppleTree");
		cyclops = transform.Find ("CyclopsModel");
		cyclopsBody = cyclops.GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator>();
	}
	
	void FixedUpdate () {
		
		if(anim.GetCurrentAnimatorStateInfo(0).IsName ("Walk")){
			cyclops.LookAt (tree.position);
			StartCoroutine("MoveCyclops");
			if ( (tree.position - cyclops.position).magnitude  < stopDistance){
				//cyclopsBody.velocity = Vector3.zero;
				//anim.SetBool ("Walk",false);
				//anim.SetBool ("Attack",true);
				Destroy(this.gameObject);
			}
		}else{
			cyclopsBody.velocity = Vector3.zero;
		}
	}
	
	public int CorrectSuggestion(){
		if(Random.value < inclination){
			anim.SetBool("Walk", true);
			anim.SetBool ("Sit", false);
			anim.SetBool ("Attack", false);
			return APPLES;
		}
		else{
			inclination += 0.25f;
			if(Random.value < 0.5f){
				anim.SetBool ("Walk", false);
				anim.SetBool ("Sit", true);
				anim.SetBool ("Attack", false);
				return SIT;
			}
			anim.SetBool ("Walk", false);
			anim.SetBool ("Sit", false);
			anim.SetBool ("Attack", true);
			return ATTACK;
		}
	}
	
	public int IncorrectSuggestion(){
		anim.SetBool("Walk", false);
		anim.SetBool ("Sit", false);
		anim.SetBool ("Attack", true);
		return ATTACK;
	}
	
	public void CorrectSuggestionWrapper(){
		CorrectSuggestion ();
	}
	
	public void IncorrectSuggestionWrapper(){
		IncorrectSuggestion ();
	}
	
	private IEnumerator MoveCyclops(){
		yield return new WaitForSeconds(0);
		cyclopsBody.velocity = (cyclops.forward * walkSpeed);
	}
}
