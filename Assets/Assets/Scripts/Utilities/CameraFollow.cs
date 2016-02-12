using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform mainCamera;

	void Update ()
	{
        mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.position.y, mainCamera.position.z);
	}
}
