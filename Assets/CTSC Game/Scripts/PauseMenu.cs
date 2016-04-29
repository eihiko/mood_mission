using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PauseMenu : MonoBehaviour{
	
	public GUITexture pausedGUI;
    public Saver saver;
	GameObject user;
    string savepath;
	string userName;
	string databaseName;
	string tableName;
	const string userNameKey = "user";
	const string serializedDataKey = "serializedData";
	public List<Transform> myList = new List<Transform>();
	bool paused = false;
	dbAccess db;

	void Start() {
        Reset();
	}

    public void Reset()
    {
        savepath = Application.persistentDataPath + "/Saves/";
        paused = false;
        if (pausedGUI)
        {
            pausedGUI.enabled = false;
        }
        user = GameObject.Find("User");
        userName = user.GetComponent<LoginInfo>().getUserName();
        databaseName = user.GetComponent<LoginInfo>().getDatabaseName();
        tableName = user.GetComponent<LoginInfo>().getTableName();
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
            UnityEngine.Cursor.visible = false;
			return;

		}
        UnityEngine.Cursor.visible = true;
        GUIStyle box = "box";   
		GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 300, 400, 600), box);

		GUILayout.BeginVertical(); 
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Save Game")) {
            saver.Save(DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss") + ".dat");
		}
		GUILayout.Space(60);
        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        DirectoryInfo dir = new DirectoryInfo(savepath);
        FileInfo[] files = dir.GetFiles();
		foreach (FileInfo file in files) { 
			if (GUILayout.Button(file.Name)) {
                saver.Load(file.Name);
			} 
		} 
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	public void setPaused(bool paused) {
		this.paused = paused;
	}

}
