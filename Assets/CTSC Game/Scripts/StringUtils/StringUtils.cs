using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class StringUtils : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void readFile(int eventCount, string path){

		foreach (string line in File.ReadAllLines(@path))
		{

			for (int i = 0; i < eventCount; i++){

				if (line.Contains("episode") & line.Contains("2006"))
				{
					Console.WriteLine(line);
				}
			}
		}
	}
}
