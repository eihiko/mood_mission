using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseMenu : MonoBehaviour{
	
	public GUITexture pausedGUI;
	GameObject user;
	string userName;
	string databaseName;
	string tableName;
	const string userNameKey = "user";
	const string serializedDataKey = "serializedData";
	public List<Transform> myList = new List<Transform>();
	bool paused = false;
	dbAccess db;

	void Start() {
		if (pausedGUI){
			pausedGUI.enabled = false;
		}
		user = GameObject.Find ("User");
		userName = user.GetComponent<LoginInfo> ().getUserName ();
		databaseName = user.GetComponent<LoginInfo> ().getDatabaseName ();
		tableName = user.GetComponent<LoginInfo> ().getTableName ();
		//Debug.Log ("User: " + userName + "db: " + databaseName + "table: " + tableName);
	}

	void Update() { 
		if (paused) { 
			//pause the game
			if (pausedGUI)
				pausedGUI.enabled = true;
		} else {
			//unpause the game
			if (pausedGUI)
				pausedGUI.enabled = false;
		}
	}

	void OnGUI() {
		if (!paused) {
			GUILayout.BeginArea(new Rect(200, 10, 400, 20));
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
//			GUILayout.Label("Press Escape to Pause");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
			return;
		}
	   
		GUIStyle box = "box";   
		GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 300, 400, 600), box);

		GUILayout.BeginVertical(); 
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Save Game")) {
			//Debug.Log (userName + " " + databaseName + " " + tableName);
			LevelSerializer.SaveGame(userName);
			string serializedGame = LevelSerializer.SerializeLevel();
			if (serializedGame == null || serializedGame.Equals("")){
				Debug.Log ("Game was not serialized properly.");
			}
			saveGameToDB(serializedGame);
		}
		GUILayout.Space(60);
		foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName]) { 
			if (GUILayout.Button(sg.Caption)) { 
				LevelSerializer.LoadNow(sg.Data);
				Time.timeScale = 1;
			} 
		} 
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	public void setPaused(bool paused) {
		this.paused = paused;
	}

	public void saveGameToDB(string serializedGame){
		db = new dbAccess();
		db.OpenDB(databaseName);
//		try{
		db.UpdateColumn(tableName, serializedDataKey, serializedGame, userNameKey, userName);
//			Debug.Log("Saved game to database under user " + userName);
//		} catch (Exception e){
//			Debug.Log ("Could not save game for " + userName);		
//		}
	}
}
