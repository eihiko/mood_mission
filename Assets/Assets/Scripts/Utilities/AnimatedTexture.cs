using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimatedTexture : MonoBehaviour
{
    public int rows;
    public int columns;
    public int frameRate = 20;

    private RawImage rawImage;
    private float fps;
    private float timePast;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        fps = 1.0f/frameRate;
    }

    void Update()
    {
        timePast += Time.deltaTime;
        if (timePast >= fps)
        {
            var offset = rows/(float) columns;
            rawImage.uvRect = rawImage.uvRect.x + offset > 1.0f
                ? new Rect(0f, rawImage.uvRect.y, rawImage.uvRect.width, rawImage.uvRect.height)
                : new Rect(rawImage.uvRect.x + offset, rawImage.uvRect.y, rawImage.uvRect.width, rawImage.uvRect.height);
            timePast = 0f;
        }
    }
}
