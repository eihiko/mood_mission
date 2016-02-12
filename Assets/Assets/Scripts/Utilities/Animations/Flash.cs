using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// gets all child images of a gameobject to flash
// this script goes on the parent gameobject
public class Flash : MonoBehaviour
{
    public float delaySeconds;
    private float timeElapsed;

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= delaySeconds)
        {
            timeElapsed = 0f;
            gameObject.GetComponentsInChildren<RawImage>().ToList().ForEach(image => image.enabled = !image.enabled);
        }
    }
}
