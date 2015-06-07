using UnityEngine;
using System.Collections;

public class MinimapUpdater : MonoBehaviour {

	public GameObject playerMarker, objectiveArrow;
	public GameObject TorkanaMarker;
	public GameObject TavernMarker, SonMarker, MT3Marker, BlacksmithMarker, FT1Marker, GirlMarker, BoathouseMarker;

	public GameObject CurrentObjective;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		objectiveArrow.transform.LookAt (CurrentObjective.transform.position);
	}

	public void changeObjective(string objective){
		switch (objective) {
		case "Torkana":
			this.objectiveArrow.SetActive (true);
			this.objectiveArrow = this.TorkanaMarker;
			break;
		case "Son":
			this.objectiveArrow.SetActive (true);
			this.objectiveArrow = this.SonMarker;
			break;
		case "MT3":
			this.objectiveArrow.SetActive (true);
			this.objectiveArrow = this.MT3Marker;
			break;
		case "Blacksmith":
			this.objectiveArrow.SetActive (true);
			this.objectiveArrow = this.BlacksmithMarker;
			break;
		case "FT1":
			this.objectiveArrow.SetActive (true);
			this.objectiveArrow = this.FT1Marker;
			break;
		case "Girl":
			this.objectiveArrow.SetActive (true);
			this.objectiveArrow = this.GirlMarker;
			break;
		case "Boathouse":
			this.objectiveArrow.SetActive (true);
			this.objectiveArrow = this.BoathouseMarker;
			break;
		default:
			this.objectiveArrow.SetActive (false);
			break;
		}
	}
}
