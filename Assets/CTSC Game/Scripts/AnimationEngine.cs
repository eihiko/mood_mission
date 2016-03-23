using UnityEngine;
using System.Collections;

public class AnimationEngine : MonoBehaviour {
	//types of animations possible for this engine
	public enum Type { WALK, IDLE, LIMP, STRAFERIGHT,
		STRAFELEFT, WAVEHAND, WAVEARMS, JUMP, 
		RUN, SIT, TURN }
	public Animator animator;
	public float walkTime = 10.0f;
	public float runTime = 5.0f;
	public float jumpTime = 1.0f;
	public float strafeLeftTime = 6.0f;
	public float strafeRightTime = 4.0f;
	public float waveHandTime = 2.0f;
	public float waveArmsTime = 5.0f;
	public float limpTime = 5.0f;
	public float sitTime = 7.0f;
	public float rotation = 0.0f;

	Transform This;
	bool holdRot, strafing;
	
	// Use this for initialization
	void Start () {
		holdRot = false;
		animator = GetComponent<Animator> ();
		//StartCoroutine ("Animate");
		//This = transform;
		
	}
	
	IEnumerator Animate(){
		return null;
		/*setMoveSpeed (0.5f);
		yield return new WaitForSeconds(walkTime);
		setMoveSpeed (1.5f);
		yield return new WaitForSeconds (runTime);
		setHeight (.4f);
		yield return new WaitForSeconds (jumpTime);
		setHeight (.1f);
		yield return new WaitForSeconds (runTime);
		setStrafeLeft (true);
		yield return new WaitForSeconds (strafeLeftTime);
		setMoveSpeed (.5f);
		yield return new WaitForSeconds (strafeLeftTime);
		setStrafeLeft (false);
		setMoveSpeed (1.5f);
		setStrafeRight (true);
		yield return new WaitForSeconds (strafeRightTime);
		setMoveSpeed (.5f);
		yield return new WaitForSeconds (strafeRightTime);
		setStrafeRight (false);
		yield return new WaitForSeconds (walkTime);
		setStrafeLeft (true);
		yield return new WaitForSeconds (strafeLeftTime);
		setStrafeLeft (false);
		yield return new WaitForSeconds (walkTime);
		setStrafeRight (true);
		yield return new WaitForSeconds (strafeRightTime);
		setStrafeRight (false);
		yield return new WaitForSeconds (walkTime);
		setMoveSpeed (0.0f);
		setWaveHand (true);
		yield return new WaitForSeconds (waveHandTime);
		setWaveHand (false);
		setWaveArms (true);
		yield return new WaitForSeconds (waveArmsTime);
		setWaveArms (false);
		setHeight (.4f);
		yield return new WaitForSeconds (jumpTime);
		setHeight (.1f);
		setHasLimp (true);
		setMoveSpeed (.3f);
		yield return new WaitForSeconds (limpTime);
		setMoveSpeed (0.0f);
		setHasLimp (false);
		animator.SetTrigger ("SitTrigger");
		setSitting (true);
		yield return new WaitForSeconds (sitTime);
		setSitting (false);
		yield return new WaitForSeconds (sitTime);
		setMoveSpeed (-.5f);
		yield return new WaitForSeconds (walkTime);
		setMoveSpeed (-1.2f);
		yield return new WaitForSeconds (runTime);
		setMoveSpeed (-.5f);
		yield return new WaitForSeconds (walkTime);
		setMoveSpeed (0.0f);*/
	}
	
	public void setRotation(float rot){
		transform.localEulerAngles = new Vector3(0f, rot, 0f);
		//This.rotation.Set(This.rotation.x, rot, This.rotation.z, This.rotation.w);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (holdRot && !strafing){
			//setRotation (rotation);
		}
		//Debug.Log("Has bool is: " + animator.GetBool("hasLimp"));
		//if(animator.get
		//Debug.Log ("current movespeed is: " + animator.GetFloat(Animator.StringToHash("MoveSpeed")));
	}
	
	public void setYaw(float yaw){
		animator.SetFloat("Yaw", yaw);
	}
	
	public void setHeight(float height){
		animator.SetFloat("Height", height);
	}
	
	public void setMoveSpeed(float moveSpeed){
		animator.SetFloat("MoveSpeed", moveSpeed);
		if (moveSpeed > 1f || moveSpeed < 0f) {
			holdRot = true;
		} else {
			holdRot = false;
		}
	}
	
	public void setSitting(bool sit){
		animator.SetBool("Sit", sit);
	}
	
	public void setStrafeLeft(bool strafeLeft){
		strafing = strafeLeft;
		animator.SetBool("StrafeLeft", strafeLeft);
	}
	
	public void setStrafeRight(bool strafeRight){
		strafing = strafeRight;
		animator.SetBool("StrafeRight", strafeRight);
	}
	
	public void setWaveHand(bool waveHand){
		animator.SetBool("WaveHand", waveHand);
	}
	
	public void setWaveArms(bool waveArms){
		animator.SetBool ("WaveArms", waveArms);
	}
	
	public void setHasLimp(bool hasLimp){
		animator.SetBool("HasLimp", hasLimp);
	}

	public void setWalking(bool walking){
		animator.SetBool ("Walking", walking);
	}
}
