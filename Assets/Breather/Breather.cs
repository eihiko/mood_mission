using UnityEngine;
using System.Collections;

public class Breather : MonoBehaviour {


	public float length;
	public float sweetSpotRatio;
	public float maxSpeed;
	public float minSpeed;
	public float winCount;
	private float speed;
	private Transform start;
	private Rigidbody marker;
	private Transform good;
	private BoxCollider col;
	private Transform bad;
	private bool won = false;
	private bool inSweetSpot;
	
	void Start () {
	
		marker = GetComponentInChildren<Rigidbody>();
		col = GetComponent<BoxCollider>();
		good = transform.FindChild ("Good");
		bad = transform.FindChild("Bad");
		
		bad.localScale = new Vector3(length, 0.9f, 0.9f);
		good.localScale = new Vector3(length * sweetSpotRatio, 1f, 1f);
		col.size = new Vector3(length * sweetSpotRatio, 1f, 1f);
		
		start = transform;
		marker.position = new Vector3(marker.position.x - (length / 2f), marker.position.y, marker.position.z);
		speed = maxSpeed;
		marker.velocity = Vector3.right * speed;
		
	}

	void FixedUpdate () {
		if(speed <= minSpeed){
			won = true;
		}
		if((transform.position - marker.position).magnitude > (length / 2)){
			marker.velocity = -marker.velocity;
		}
	}
	
	void OnTriggerEnter(Collider c){
		inSweetSpot = true;
	}
	
	void OnTriggerExit(Collider c){
		inSweetSpot = false;
	}
	
	void Update(){
		if(Input.GetKeyDown (KeyCode.Space)){
			if(inSweetSpot){
				speed -= ((maxSpeed - minSpeed) / winCount);
			}
			else{
				speed += ((maxSpeed - minSpeed) / winCount);
			}
			speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
			marker.velocity = (marker.velocity.normalized * speed);
		}
	}
	
	public bool hasWon(){
		return won;
	}
}
