using UnityEngine;
using System.Collections;

public class TestMentorFollow : MonoBehaviour {

	public Transform player;
	public Transform pathPointers;
	private NavMeshAgent agent;
	private AnimationEngine animationEngine;
	private Vector3 previousLocation;
	private float velocity;
	//An arraylist to store the agent destinations
	private Vector3[] agentDests;
	public int currIndex;
	private bool lockTorkana;
	public int numDests = 30;
	private int endIndex = 0;
	private bool isActive = false;
	private bool pathComplete = true;
	private float speed = 4.8f;
	private Transform torkanaTrans;
	private Transform playerTrans;
	private float manhattan = 0.0f;
	private bool pathBegun = false;
	private float maxDistance = 9f;

	public void Start() {

//		foreach (Transform t in transform){
//			if (t.gameObject.name.Equals("mixamorig:Hips")){
//				torkanaTrans = t;
//				break;
//			}
//		}
		torkanaTrans = transform;
		playerTrans = player;
		agentDests = new Vector3[numDests];
		agent = GetComponent<NavMeshAgent>();
		animationEngine = GetComponent<AnimationEngine> ();
		currIndex = 0;
		//Iterate over the children of the pathPointers object
		foreach (Transform pointer in pathPointers) 
		{
			agentDests[currIndex] = pointer.position;
			currIndex++;
		}

		numDests = currIndex;
		currIndex = 0;
	//	lockTorkana = false;
		previousLocation = torkanaTrans.position;
		//InvokeRepeating ("moveTorkana", 0, 0.1f);
		isActive = true;
	}

	public void beginPath(int startIndex, int endIndex){
		this.currIndex = startIndex;
		this.endIndex = endIndex;
		this.pathComplete = false;
//		if (!agent.enabled)
//			agent.enabled = true;
		Vector3.MoveTowards (torkanaTrans.position, agentDests [currIndex], speed);
	//	agent.SetDestination(agentDests[startIndex]);
		pathBegun = true;
	}


	public bool isEnabled(){
		return isActive;
	}

	public bool isGoal(){
		if ((currIndex >= endIndex || currIndex == numDests) &&
		    Vector3.Distance (torkanaTrans.position, agentDests [endIndex]) < maxDistance) {
			return true;
		}
		return false;
	}

	void Update() {
		torkanaTrans = transform;
		playerTrans = player;
		animationEngine.setHasLimp (true);

		if (pathBegun) {
		//	GroundTorkana ();
			MoveTorkana ();
		}
	}

    void MoveTorkana(){
		if (isGoal()) {
			animationEngine.setMoveSpeed(0.0f);
			pathComplete = true;
			return;
		}
		
		manhattan = System.Math.Abs(torkanaTrans.position.x - playerTrans.position.x) +
			System.Math.Abs(torkanaTrans.position.y - playerTrans.position.y);
		
		//Debug.Log ("Manhattan distance between Torkana and Player is: " + manhattan);
		
		if (Vector3.Distance(torkanaTrans.position, playerTrans.position) < maxDistance) {
			// ||// (lockTorkana && (transform.position - player.position).magnitude < 7.5f))
			
			//Debug.Log ("Torkana is moving to the destination: " + currIndex); 
			//lockTorkana = false;
			//agent.SetDestination(agentDests[currIndex]);
			float step = speed * Time.deltaTime;
			animationEngine.setMoveSpeed (1f);
			torkanaTrans.position = Vector3.MoveTowards (torkanaTrans.position, agentDests [currIndex], step);
			
			Vector3 targetDir = agentDests [currIndex] - torkanaTrans.position;
			float step_rot = speed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards (torkanaTrans.forward, targetDir, step_rot, 0.0F);
			Debug.DrawRay (torkanaTrans.position, newDir, Color.red);
			torkanaTrans.rotation = Quaternion.LookRotation (newDir);
		} else {
			animationEngine.setMoveSpeed(0f);
		}
		
		//check Torkana's dist from the target dest
		manhattan = System.Math.Abs(torkanaTrans.position.x - agentDests[currIndex].x) +
			System.Math.Abs(torkanaTrans.position.y - agentDests[currIndex].y);
		//Debug.Log ("Torkana is: " + manhattan + " from the current goal dest.");
		//Debug.Log("Number of destinations is: " + numDests);
		
		if (Vector3.Distance(torkanaTrans.position, agentDests[currIndex]) < 1f) {
			previousLocation = agentDests[currIndex];
			++currIndex;
			//torkanaTrans.position = Vector3.MoveTowards(torkanaTrans.position, agentDests[++currIndex], step);
			//agent.SetDestination(agentDests[++currIndex]);
		}
	}

	void GroundTorkana()
	{

		CapsuleCollider torkCap = torkanaTrans.GetComponent<CapsuleCollider> ();
		//ray starts at player position and points down
		Ray ray = new Ray(torkanaTrans.position, Vector3.down);
		
		//will store info of successful ray cast
		RaycastHit hitInfo;
		
		//terrain should have mesh collider and be on custom terrain 
		//layer so we don't hit other objects with our raycast
		LayerMask layer = LayerMask.NameToLayer("Terrain");

		int layerMask = 1 << LayerMask.NameToLayer("Terrain");
		layerMask = ~layerMask;

		//cast ray
		if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
		{
			//get where on the z axis our raycast hit the ground
			float z = hitInfo.point.z;

//			Debug.Log("A raycast hit the terrain with z: " + z);
			
			//copy current position into temporary container
			Vector3 pos = torkanaTrans.position;
			
			//change z to where on the z axis our raycast hit the ground
			pos.z = z;
			
			//override our position with the new adjusted position.
			torkanaTrans.position = pos;
		}
	}
}