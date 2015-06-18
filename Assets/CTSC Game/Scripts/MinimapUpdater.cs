using UnityEngine;
using System.Collections;

public class MinimapUpdater : MonoBehaviour {

	public GameObject playerMarker, objectiveArrow;
	public GameObject TorkanaMarker;
	public GameObject TavernMarker, SonMarker, MT3Marker, BlacksmithMarker, FT1Marker, GirlMarker, BoathouseMarker;

	GameObject[] allMarkers = new GameObject[8];

	public GameObject CurrentObjective;

	// Use this for initialization
	void Start () {
		allMarkers [0] = TorkanaMarker;
		allMarkers [1] = TavernMarker;
		allMarkers [2] = SonMarker;
		allMarkers [3] = MT3Marker;
		allMarkers [4] = BlacksmithMarker;
		allMarkers [5] = FT1Marker;
		allMarkers [6] = GirlMarker;
		allMarkers [7] = BoathouseMarker;
		changeObjective ("nothing");
	}
	
	// Update is called once per frame
	void Update () {
		objectiveArrow.transform.LookAt (CurrentObjective.transform.position);
	}

	public void deactivateOthers(int active){
		for (int i=0; i<allMarkers.Length; i++) {
			if (i==active)
				allMarkers[i].SetActive(true);
			else
				allMarkers[i].SetActive(false);
		}
	}

	public void changeObjective(string objective){
		switch (objective) {
		case "Torkana":
			this.objectiveArrow.SetActive (true);
			this.CurrentObjective = this.TorkanaMarker;
			deactivateOthers(0);
			break;
		case "Tavern":
			this.objectiveArrow.SetActive(true);
			this.CurrentObjective = this.TavernMarker;
			deactivateOthers(1);
			break;
		case "Son":
			this.objectiveArrow.SetActive (true);
			this.CurrentObjective = this.SonMarker;
			deactivateOthers(2);
			break;
		case "MT3":
			this.objectiveArrow.SetActive (true);
			this.CurrentObjective = this.MT3Marker;
			deactivateOthers(3);
			break;
		case "Blacksmith":
			this.objectiveArrow.SetActive (true);
			this.CurrentObjective = this.BlacksmithMarker;
			deactivateOthers(4);
			break;
		case "FT1":
			this.objectiveArrow.SetActive (true);
			this.CurrentObjective = this.FT1Marker;
			deactivateOthers(5);
			break;
		case "Girl":
			this.objectiveArrow.SetActive (true);
			this.CurrentObjective = this.GirlMarker;
			deactivateOthers(6);
			break;
		case "Boathouse":
			this.objectiveArrow.SetActive (true);
			this.CurrentObjective = this.BoathouseMarker;
			deactivateOthers(7);
			break;
		default:
			this.objectiveArrow.SetActive (false);
			break;
		}
	}
}
