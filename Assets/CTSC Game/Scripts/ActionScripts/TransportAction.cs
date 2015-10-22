using UnityEngine;
using System.Collections;

public class TransportAction : MissionAction {

	private GameObject player;
	private TransportPlayer transporter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool execute() {
		transporter.TransportNow (player);
		return true;
	}

	public TransportAction(GameObject player, TransportPlayer transporter){
		this.player = player;
		this.transporter = transporter;
	}


}
