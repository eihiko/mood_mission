using UnityEngine;
using System.Collections;
using System.Data;  // we import our  data class
using Mono.Data.Sqlite; // we import sqlite
using System.Collections.Generic;
using System;

public class dbAccess : MonoBehaviour {

	// variables for basic query access
	private string connection;
	private IDbConnection dbcon;
	private IDbCommand dbcmd;
	private IDataReader reader;
	
	public void OpenDB(string p) {
		connection = "URI=file:" + p; // we set the connection to our database
		dbcon = new SqliteConnection(connection);
		dbcon.Open();
		Debug.Log ("Database connection opened.");
	}
	
//	IDataReader BasicQuery(q : String, r : boolean):IDataReader{ // run a baic Sqlite query
//		dbcmd = dbcon.CreateCommand(); // create empty command
//		dbcmd.CommandText = q; // fill the command
//		reader = dbcmd.ExecuteReader(); // execute command which returns a reader
//		if(r) { // if we want to return the reader
//			return reader; // return the reader
//		}
//	}
	
//	// This returns a 2 dimensional ArrayList with all the
//	//  data from the table requested
//	function ReadFullTable(tableName : String) {
//		var query : String;
//		query = "SELECT * FROM " + tableName;
//		dbcmd = dbcon.CreateCommand();
//		dbcmd.CommandText = query; 
//		reader = dbcmd.ExecuteReader();
//		var readArray = new ArrayList();
//		while(reader.Read()) { 
//			var lineArray = new ArrayList();
//			for (var i:int = 0; i < reader.FieldCount; i++)
//				lineArray.Add(reader.GetValue(i)); // This reads the entries in a row
//			readArray.Add(lineArray); // This makes an array of all the rows
//		}
//		return readArray; // return matches
//	}

	//Reads the specified column from the specified table
	public string ReadColumn(string column, string tableName, string userKey, string userName) {
		string query;
		List<string> readValues = new List<string> ();
		query = "SELECT " + column + " FROM " + tableName + " WHERE " + userKey + "='" + userName + "'";
		Debug.Log (query);
		dbcmd = dbcon.CreateCommand();
		dbcmd.CommandText = query; 
		dbcmd.CommandType = CommandType.Text;
		reader = dbcmd.ExecuteReader();
		var readArray = new ArrayList();
		while (reader.Read ()){
			readValues.Add(Convert.ToString(reader[column]));
		}
		return readValues[0];
	}

	public void UpdateColumn(string tableName, string column, string columnValue, string userNameKey, string userName) {
		string query;
		query = "UPDATE " + tableName + " SET " + column + "='" + columnValue + "' WHERE " + userNameKey + "='" + userName + "'";
		dbcmd = dbcon.CreateCommand();
		dbcmd.CommandText = query; 
		reader = dbcmd.ExecuteReader();
	}
	
//	// This function deletes all the data in the given table.  Forever.  WATCH OUT! Use sparingly, if at all
//	function DeleteTableContents(tableName : String) {
//		var query : String;
//		query = "DELETE FROM " + tableName;
//		dbcmd = dbcon.CreateCommand();
//		dbcmd.CommandText = query; 
//		reader = dbcmd.ExecuteReader();
//	}
	
	public void CreateTable(string name, ArrayList col, ArrayList colType) { // Create a table, name, column array, column type array, may not have columns
		string query;

		query  = "CREATE TABLE " + name; 
		query += "(" + col[0] + " " + colType[0];
		for(var i = 1; i<col.Count; i++) {
			query += ", " + col[i] + " " + colType[i];
		}
		query += ")";

		dbcmd = dbcon.CreateCommand(); // create empty command
		dbcmd.CommandText = query; // fill the command
		reader = dbcmd.ExecuteReader(); // execute command which returns a reader
	}

	public void Insert(string tableName, ArrayList columns, ArrayList values) { // Specific insert with col and values
		string query;
		query = "INSERT INTO " + tableName + "(" + columns[0];
		
		for(var i = 1; i < columns.Count; i++) {
			query += ", " + columns[i];
		}
		
		query += ") VALUES ('" + values[0];
		
		for(var i = 1; i < values.Count; i++) {
			query += "', '" + values[i];
		}
		query += "')";

		dbcmd = dbcon.CreateCommand();
		dbcmd.CommandText = query; 
		reader = dbcmd.ExecuteReader();
	}
	
//	public void CreateTableIfNotExists(string name, ArrayList col, ArrayList colType) { // Create a table, name, column array, column type array
//		string query;
//		query  = "CREATE TABLE IF NOT EXISTS " + name + "(" + col[0] + " " + colType[0];
//		for(var i=1; i<col.Count; i++) {
//			query += ", " + col[i] + " " + colType[i];
//		}
//		query += ")";
//		dbcmd = dbcon.CreateCommand(); // create empty command
//		dbcmd.CommandText = query; // fill the command
//		reader = dbcmd.ExecuteReader(); // execute command which returns a reader
//		Debug.Log ("Created DB table that did not exist.");
//	}
//	
//	public void CreateTableIfNotExists(string name) { // Create a table
//		string query;
//		query  = "CREATE TABLE IF NOT EXISTS " + name + "(default text)";
//		dbcmd = dbcon.CreateCommand(); // create empty command
//		dbcmd.CommandText = query; // fill the command
//		reader = dbcmd.ExecuteReader(); // execute command which returns a reader
//		Debug.Log ("Created DB table that did not exist.");
//	}
	
//	//ALTER TABLE {tableName} ADD COLUMN COLNew {type};
//	public void CreateRowInTable(string tableName, string name, string type) { // Create a column with name and type in tableName
//		string query;
//		query  = "ALTER TABLE " + tableName + " ADD COLUMN" + name + " " + type;
//		dbcmd = dbcon.CreateCommand(); // create empty command
//		dbcmd.CommandText = query; // fill the command
//		reader = dbcmd.ExecuteReader(); // execute command which returns a reader
//	}
	
//	public void InsertIntoSingle(string tableName, string colName, string value) { // single insert 
//		string query;
//		Debug.Log (value);
//		query = "INSERT INTO " + tableName + "(" + colName + ") VALUES (" + value + ")";
//		dbcmd = dbcon.CreateCommand(); // create empty command
//		dbcmd.CommandText = query; // fill the command
//		reader = dbcmd.ExecuteReader(); // execute command which returns a reader
//	}
	

//	
//	function InsertInto(tableName : String, values : Array) { // basic Insert with just values
//		var query : String;
//		query = "INSERT INTO " + tableName + " VALUES (" + values[0];
//		for(var i=1; i<values.length; i++) {
//			query += ", " + values[i];
//		}
//		query += ")";
//		dbcmd = dbcon.CreateCommand();
//		dbcmd.CommandText = query; 
//		reader = dbcmd.ExecuteReader(); 
//	}
//	
//	// This function reads a single column
//	//  wCol is the WHERE column, wPar is the operator you want to use to compare with, 
//	//  and wValue is the value you want to compare against.
//	//  Ex. - SingleSelectWhere("puppies", "breed", "earType", "=", "floppy")
//	//  returns an array of matches from the command: SELECT breed FROM puppies WHERE earType = floppy;
//	//function SingleSelectWhere(tableName : String, itemToSelect : String, wCol : String, wPar : String, wValue : String):Array { // Selects a single Item
//	function SingleSelectWhere(tableName : String, itemToSelect : String, wCol : String, wPar : String, wValue : String):List.<String>{ // Selects a single Item
//		var query : String;
//		query = "SELECT " + itemToSelect + " FROM " + tableName + " WHERE " + wCol + wPar + wValue; 
//		dbcmd = dbcon.CreateCommand();
//		dbcmd.CommandText = query; 
//		reader = dbcmd.ExecuteReader();
//		//var readArray = new Array();
//		var readArray:List.<String> = new List.<String>();
//		while(reader.Read()) { 
//			//readArray.Push(reader.GetString(0)); // Fill array with all matches
//			var japanese:String = reader.GetString(0);
//			Debug.Log(japanese);
//			readArray.Add(japanese); // Fill array with all matches
//			var url:String = reader.GetString(1);
//			Debug.Log(url);
//			readArray.Add(url); // Fill array with all matches
//		}
//		return readArray; // return matches
//	}
	
	public void CloseDB() {
		reader.Close(); // clean everything up
		reader = null; 
		dbcmd.Dispose(); 
		dbcmd = null; 
		dbcon.Close(); 
		dbcon = null; 
	}
}
