using UnityEngine;
using System.Collections;

public class ResolutionManager : MonoBehaviour 
{
    public Transform[] toReposition;
    public Transform toolbox;

	private float scaleFactor;

    // Use this for initialization
    void Start()
    {
		scaleFactor = Camera.main.aspect / 1.33f;

        foreach (Transform item in toReposition)
            item.position = new Vector3(item.position.x * scaleFactor, item.position.y, item.position.z); 

		toolbox.position = new Vector3((scaleFactor - 1) * 4, toolbox.position.y, toolbox.position.z);
    }
}