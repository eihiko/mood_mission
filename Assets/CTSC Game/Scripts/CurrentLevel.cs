using UnityEngine;

[System.Serializable]
[SerializeAll]
public class CurrentLevel : MonoBehaviour {
	
	EventHandler.GameLocation currLocation;
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setCurrLocation(EventHandler.GameLocation location){
		this.currLocation = location;
	}
	
	public EventHandler.GameLocation getCurrLocation(){
		return this.currLocation;
	}
}
