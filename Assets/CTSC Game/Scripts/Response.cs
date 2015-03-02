using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

public class Response : MonoBehaviour {

	public GameObject UI;
	private static StringBuilder qAndA = new StringBuilder();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void openUI(){
		UI.SetActive(true);
	}

	public void closeUI (){
		UI.SetActive (false);
	}

	public void recordResponses(string[] question, string[] response){
		for (int i = 0; i < question.Length; i++) {
			qAndA.Append(question[i] + ":" + response[i]);		
		}
		string writeTo = Directory.GetCurrentDirectory ();//.Create ("Responses").ToString();
//		foreach (Directory dir in Directory.GetParent().GetDirectories()){
//			if (dir.ToString().Equals("Response")){
//
//			}
//		}
		using (System.IO.StreamWriter file =
		       new System.IO.StreamWriter(@writeTo, true))
		{
			file.WriteLine(qAndA.ToString ());
		}

		closeUI ();
	}


}
