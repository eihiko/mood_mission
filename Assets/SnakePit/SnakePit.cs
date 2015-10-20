using UnityEngine;
using System.Collections;

public class SnakePit : MonoBehaviour {

	public GameObject snake;
	public float spawnRange;
	public float resetRange;
	public int count;
	public int rate;

	// Use this for initialization
	void Start () {
	
		for(int i = 0; i < count; i++){
			MakeSnake();
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Random.Range (0,100) < rate){
			MakeSnake ();
		}
			
	}
	
	private void MakeSnake(){
		GameObject s = Instantiate (snake);
		Vector3 v = new Vector3(Random.Range(-spawnRange/2f,spawnRange/2f), 0f,Random.Range(-spawnRange/2f,spawnRange/2f));
		s.transform.position = transform.position + v;
		s.transform.eulerAngles = new Vector3(0,Random.Range (0,360),0);
		s.GetComponent<Leash>().Setup (transform, resetRange);
    }
   }
    