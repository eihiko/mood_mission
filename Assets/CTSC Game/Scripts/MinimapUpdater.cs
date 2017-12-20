using UnityEngine;
using System.Collections;

public class MinimapUpdater : MonoBehaviour {

	public GameObject playerMarker, objectiveArrow;
	public GameObject TorkanaMarker;
	public GameObject TavernMarker, SonMarker, MT3Marker, BlacksmithMarker, FT1Marker, 
	GirlMarker, BoathouseMarker, MapMarker, GardenMarker, CityMarker, DoctorMarker, 
	AppleLadyMarker, TownCenterMarker, SewerMarker, DragonMarker;

	GameObject[] allMarkers = new GameObject[16];

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
		allMarkers [8] = MapMarker;
		allMarkers [9] = GardenMarker;
		allMarkers [10] = CityMarker;
		allMarkers [11] = DoctorMarker;
		allMarkers [12] = AppleLadyMarker;
		allMarkers [13] = TownCenterMarker;
		allMarkers [14] = SewerMarker;
		allMarkers [15] = DragonMarker;;
		changeObjective ("nothing");
		CurrentObjective = playerMarker;
	}
	
	// Update is called once per frame
	void Update () {
		objectiveArrow.transform.LookAt (CurrentObjective.transform.position);
		objectiveArrow.transform.Rotate (90f, 90f, 0f);
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
			this.CurrentObjective = this.TorkanaMarker;
			this.objectiveArrow.SetActive (true);
			deactivateOthers(0);
			playerMarker.SetActive(false);
			break;
		case "Tavern":
			this.CurrentObjective = this.TavernMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(1);
			playerMarker.SetActive(false);
			break;
		case "Son":
			this.CurrentObjective = this.SonMarker;
			this.objectiveArrow.SetActive (true);
			deactivateOthers(2);
			playerMarker.SetActive(false);
			break;
		case "MT3":
			this.CurrentObjective = this.MT3Marker;
			this.objectiveArrow.SetActive (true);
			deactivateOthers(3);
			playerMarker.SetActive(false);
			break;
		case "Blacksmith":
			this.CurrentObjective = this.BlacksmithMarker;
			this.objectiveArrow.SetActive (true);
			deactivateOthers(4);
			playerMarker.SetActive(false);
			break;
		case "FT1":
			this.CurrentObjective = this.FT1Marker;
			this.objectiveArrow.SetActive (true);
			deactivateOthers(5);
			playerMarker.SetActive(false);
			break;
		case "Girl":
			this.CurrentObjective = this.GirlMarker;
			this.objectiveArrow.SetActive (true);
			deactivateOthers(6);
			playerMarker.SetActive(false);
			break;
		case "Boathouse":
			this.CurrentObjective = this.BoathouseMarker;
			this.objectiveArrow.SetActive (true);
			deactivateOthers(7);
			playerMarker.SetActive(false);
			break;
		case "Map":
			this.CurrentObjective = this.MapMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(8);
			playerMarker.SetActive(false);
			break;
		case "Garden":
			this.CurrentObjective = this.GardenMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(9);
			playerMarker.SetActive(false);
			break;
		case "City":
			this.CurrentObjective = this.CityMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(10);
			playerMarker.SetActive(false);
			break;
		case "Doctor":
			this.CurrentObjective = this.DoctorMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(11);
			playerMarker.SetActive(false);
			break;
		case "Apple Lady":
			this.CurrentObjective = this.AppleLadyMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(12);
			playerMarker.SetActive(false);
			break;
		case "Town Center":
			this.CurrentObjective = this.TownCenterMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(13);
			playerMarker.SetActive(false);
			break;
		case "Sewer":
			this.CurrentObjective = this.SewerMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(14);
			playerMarker.SetActive(false);
			break;
		case "Dragon":
			this.CurrentObjective = this.DragonMarker;
			this.objectiveArrow.SetActive(true);
			deactivateOthers(15);
			playerMarker.SetActive(false);
			break;
		default:
			this.objectiveArrow.SetActive (false);
			playerMarker.SetActive(true);
			deactivateOthers(100); //Deactivate all markers
			break;
		}
	}
}
