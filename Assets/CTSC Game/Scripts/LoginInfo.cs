using UnityEngine;
using System.Collections;

public class LoginInfo : MonoBehaviour {

	private string userName;
	private string databaseName;
	private string tableName;
	private string saveData;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	public void setUserName(string name){
		userName = name;
	}

	public string getUserName(){
		return userName;
	}

	public void setDatabaseName(string name){
		databaseName = name;
	}

	public string getDatabaseName(){
		return databaseName;
	}

	public void setTableName(string name){
		tableName = name;
	}
	
	public string getTableName(){
		return tableName;
	}
	
	public void setSaveData(string data){
		saveData = data;
	}
	
	public string getSaveData(){
		return saveData;
	}
}
