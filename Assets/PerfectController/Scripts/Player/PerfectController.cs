using UnityEngine;

[System.Serializable]
[SerializeAll]
public class PerfectController : MonoBehaviour {
	public float maxSpeed;
	public float accl;
	public float fric;
	public float jump;
	public float grav;
	public float airBias;
	public float Xsensitivity;
	public float Ysensitivity;
	public bool useViewBob;
	public float bobStr;
	public float bobFreq;
	public float eyesHeight;
	public float yaw, pitch;
	
	private float bobTime;
	private Vector3 velocity;
	private Transform cam;
	private CharacterController controller;
	private bool isGrounded;
	private bool isControllable;
	private bool cameraControllable;

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {
		isControllable = true;
		cameraControllable = false;
		cam = transform.FindChild("Camera").transform;
		controller = gameObject.GetComponent<CharacterController>();
		pitch = 0;
		velocity = Vector3.zero;
	}

	public void UpdatePlayer(EventHandler.GameState state){
		//Lines added here to fix loading issues.  Tried OnLevelWasLoaded, didn't always work.
		cam = transform.FindChild("Camera").transform;
		//Debug.Log (cam.ToString () + " was found?");
		controller = gameObject.GetComponent<CharacterController>();

		if (state != EventHandler.GameState.PAUSE) {
			snapToGround ();
			//Debug.Log("Snapped!");
			checkGrounded ();

			if (!isControllable){

				applyMouseLook (true);
			} else {
				applyMouseLook(false);
			}
			
			applyMoveForces (isGrounded ? 1 : airBias);
			capXZVelocity ();
			if (isControllable) {
				controller.Move (velocity);
				if (isGrounded && useViewBob)
					addViewBob ();
			} else {
				controller.Move (new Vector3 (0, 0, 0));
			}
		} else {
            controller.Move(new Vector3(0, 0, 0));
        }
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.normal.y > 0.9f) {
			//myGround = hit.collider.transform;
		}
	}
	
	private void applyMouseLook(bool freeze) {
		if (!freeze) {
			yaw += Input.GetAxis ("Mouse X") * Xsensitivity;
			pitch -= Input.GetAxis ("Mouse Y") * Ysensitivity;
			pitch = Mathf.Clamp (pitch, -60f, 60f);
		}
		transform.localEulerAngles = new Vector3(0, yaw, 0);
//<<<<<<< HEAD
		//cam.localEulerAngles = new Vector3 (pitch, 9, 0);
//=======
		cam.localEulerAngles = new Vector3(pitch, 0, 0);
//>>>>>>> refs/remotes/origin/save
	}
	
	private void applyMoveForces(float mult) {
		if (Input.GetKey(KeyCode.W)) {
			velocity += transform.forward*accl*mult;
		}
		if (Input.GetKey(KeyCode.S)) {
			velocity -= transform.forward*accl*mult;
		}
		if (Input.GetKey(KeyCode.A)) {
			velocity -= transform.right*accl*mult;
		}
		if (Input.GetKey(KeyCode.D)) {
			velocity += transform.right*accl*mult;
		}
		if (Input.GetKey(KeyCode.Space)) {
			if (isGrounded) {
				velocity.y = jump;
				isGrounded = false;
			} else {
				if (velocity.y > 0f) {
					velocity.y -= Time.deltaTime*grav*.9f;
				} else {
					velocity.y -= grav*Time.deltaTime;
				}
			}
		} else if (!isGrounded) {
			velocity.y -= grav*Time.deltaTime;
		} else {
			velocity.y = 0f;
		}
	}
	
	/* 
	 * Limits the XZ velocity of the controller, thus leaving falling and jumping untouched
	 */ 
	private void capXZVelocity() {
		Vector3 xzVelo = new Vector3(velocity.x, 0f, velocity.z);
		if (xzVelo.magnitude > maxSpeed) {
			Vector3 n = xzVelo.normalized*maxSpeed;
			n.y = velocity.y;
			velocity = n;
		}
		velocity -= xzVelo*fric;
	}
	
	private void checkGrounded() {
		if (controller){
		    isGrounded = controller.isGrounded;
		}
	}
	
	private void addViewBob() {
		Vector3 xzVelo = new Vector3(velocity.x, 0f, velocity.z);
		float bobV;
		if (xzVelo.magnitude == 0f) {
			bobTime = 0f;
			bobV = 0f;
		} else {
			bobTime = (bobTime+Time.deltaTime)%(Mathf.PI*2/bobFreq);
			bobV = Mathf.Sin(bobTime*bobFreq)*bobStr*xzVelo.magnitude/maxSpeed;
		}
		cam.transform.localPosition = new Vector3(0f, bobV+eyesHeight, 0f);
	}
	
	private void snapToGround() {
		if (!isGrounded) return;
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray();
		ray.origin = transform.position;
		ray.direction = -transform.up;
		Physics.Raycast(ray, out hit, 1.5f, ~LayerMask.NameToLayer("Ground"));
		controller.Move(new Vector3(0f, -hit.distance, 0f));
	}

	public void setIsControllable(bool controllable){
		isControllable = controllable;
	}

	public bool getIsControllable(){
		return isControllable; 
	}

	public void setIsCameraControllable(bool controllable) {
		cameraControllable = controllable;
	}
}