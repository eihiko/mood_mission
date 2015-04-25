using UnityEngine;
using System.Collections;

public class MentorFollow : MonoBehaviour {

	GameObject mentor;
	GameObject player;
	GameObject path_points;
	int curr_point_index = 0;
	int last_point_index = 0;
	bool goal = false;
	Transform mentorTf;
	Transform curr_point_tf;

	// Use this for initialization
	public void Start () {
		
	}

	void followPath(int startPoint, int endPoint){
		curr_point_tf = 
		mentorTf = mentor.transform;
		curr_point_index = startPoint;
		last_point_index = endPoint;
		goal = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!goal) {
	//		float step = speed * Time.deltaTime;
//			move.transform.position = Vector3.MoveTowards(move.transform.position, to.transform.position, step);
		}
	}
}
