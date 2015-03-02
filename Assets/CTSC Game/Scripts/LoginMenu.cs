using UnityEngine;
using System.Collections;
using System;

public class LoginMenu : MonoBehaviour {

	const int MIN_PASS_LENGTH = 7;
//	string userName = "";
//	string password = "";
//	string label = "";
	const string userNameKey = "user";
	const string passwordKey = "password";
	const string serializedDataKey = "serializedData";
	const string DEFAULT_NEW_SAVE = "NEW GAME";

//	bool login = false;
//	bool createUser = false;
	ArrayList columnNames; 

	public GameObject errorLabel;
	public GameObject user;
	public GameObject eventHandler;
	LoginInfo loginInfoScript;
	EventHandler eventHandlerScript;
	UILabel errorLabelScript;
	 
	// This is the file path of the database file we want to use
	// Right now, it'll load MoodMission.sqdb in the project's root folder.
	// If one doesn't exist, it will be automatically created.
	public string DatabaseName = "MoodMission.sqdb";
	 
	// This is the name of the table we want to use
	public string TableName = "SaveTable";
	dbAccess db;
 
	void Start() {
		loginInfoScript = user.GetComponent<LoginInfo> ();
		eventHandlerScript = eventHandler.GetComponent<EventHandler> ();
		errorLabelScript = errorLabel.GetComponent<UILabel> ();
		db = new dbAccess();
		columnNames = new ArrayList();
		columnNames.Add (userNameKey);
		columnNames.Add (passwordKey);
		columnNames.Add (serializedDataKey);
		var columnValues = new ArrayList();
		columnValues.Add ("TEXT PRIMARY KEY NOT NULL");
		columnValues.Add ("TEXT NOT NULL CHECK (length(" + passwordKey + ") >= " + MIN_PASS_LENGTH + ")");
		columnValues.Add ("VARCHAR(25000) NOT NULL DEFAULT ''");
		createTable (db, TableName, columnNames, columnValues);  
    }

//	void OnGUI() {
//		GUI.Window (0, new Rect (Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2 - 70), LoginWindow, "Login");
//		if (login){
//			loginUser ();
//		}
//		if (createUser){
//			createNewUser ();
//		}
//	}

	void createTable(dbAccess db, string tableName, ArrayList columnNames, ArrayList columnValues){
		db.OpenDB(DatabaseName);
		try{
			db.CreateTable(tableName, columnNames, columnValues);
			Debug.Log("Created DB table");
		} catch (Exception e){
			Debug.Log (TableName + " already exists.");		
		}
	}

	public void loginUser(string userName, string password){
		Debug.Log (userName + ": " + password);
		// Give ourselves a dbAccess object to work with, and open it
		db = new dbAccess();
		db.OpenDB(DatabaseName);
		// Let's make sure we've got a table to work with as well!
		string tableName = TableName;
		string readValue = null;
		try {
			if (!userName.Equals("")){
				Debug.Log (db.ReadColumn(passwordKey, tableName, userNameKey, userName));
				if (password.Equals (db.ReadColumn(passwordKey, tableName, userNameKey, userName))){
					readValue = db.ReadColumn(serializedDataKey, tableName, userNameKey, userName);
					Debug.Log (readValue);
				} else {
					Debug.Log ("Password did not match.");
					errorLabelScript.text = "Password is incorrect.";
				}
			} else {
				Debug.Log("Please enter a valid user in the DB.");
				errorLabelScript.text = "Incorrect Username/Password";
			}
		}
		catch(Exception e) {// Do nothing - our table was already created
			Debug.Log(e.Message);
		}
        if (readValue != null) {
			if (readValue.Equals("NEW GAME")){
				setLoginInfo(userName, DatabaseName, tableName, readValue);
				eventHandlerScript.StartNewGame();
			} else {
				setLoginInfo(userName, DatabaseName, tableName, readValue);
				eventHandlerScript.LoadGame(readValue);
			}
		} else {
			errorLabelScript.text = "User does not exist";
		}
	}

	public void createNewUser(string userName, string password){
		// Give ourselves a dbAccess object to work with, and open it
		db = new dbAccess();
		db.OpenDB(DatabaseName);
		// Let's make sure we've got a table to work with as well!
		string tableName = TableName;
		var columnNames = new ArrayList ();
		columnNames.Add (userNameKey);
		columnNames.Add (passwordKey);
		columnNames.Add (serializedDataKey);
		var columnValues = new ArrayList ();
		columnValues.Add (userName);
		columnValues.Add (password);
		columnValues.Add (DEFAULT_NEW_SAVE);
		try {
			if (!userName.Equals("") && password.Length >= MIN_PASS_LENGTH) {
				db.Insert(tableName, columnNames, columnValues);
			} else {
				errorLabelScript.text = "Could not create new user";
			}
		}
		catch(Exception e) {// Do nothing - our table was already created
			Debug.Log (e.Message);
			errorLabelScript.text = "User already exists in system";
		}
//		createUser = false;
	}


//	void LoginWindow(int windowID) {
//		GUI.Label (new Rect (140, 40, 130, 100), "~~~~Username~~~~");
//		userName = GUI.TextField(new Rect(25, 60, 375, 30), userName);
//		GUI.Label (new Rect (140, 92, 130, 100), "~~~~~Password~~~~");
//		password = GUI.PasswordField (new Rect (25, 115, 375, 30), password, '*');
//
//		login = GUI.Button (new Rect (25, 160, 375, 50), "Login");
//		createUser = GUI.Button (new Rect (25, 210, 375, 50), "Create User");
//
//		GUI.Label (new Rect(55, 222, 250, 100), label);
//	}

	void setLoginInfo(string userName, string databaseName, string tableName, string saveData){
		loginInfoScript.setUserName (userName);
		loginInfoScript.setDatabaseName (databaseName);
		loginInfoScript.setTableName (tableName);
		loginInfoScript.setSaveData (saveData);
	}

}