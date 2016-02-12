using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    protected WebCamTexture webCamTexture;
    private IList<WebCamDevice> webCamDevices;
    private DeviceOrientation defaultOrientation;
    private Vector3 originalRotation;
    public TakenImage takenPhotoTexture2D;

    protected virtual void Start()
    {
        webCamDevices = WebCamTexture.devices;
        defaultOrientation = Input.deviceOrientation;
        originalRotation = GetComponent<RawImage>().transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (webCamTexture != null)
        {
            GetComponent<RawImage>().texture = webCamTexture;
            if (!webCamTexture.videoVerticallyMirrored)
                GetComponent<RawImage>().transform.localScale = new Vector3(-1, 1);
        }
        // this assumes the game is only allowed in landscape mode
        var opposite = determineOrientation();
        if (opposite == DeviceOrientation.Unknown) return;
        GetComponent<RawImage>().transform.localRotation = 
            Quaternion.Euler(Input.deviceOrientation == opposite ? 
            new Vector3(originalRotation.x, originalRotation.y, 180f) : originalRotation);
    }

    private DeviceOrientation determineOrientation()
    {
        if (defaultOrientation == DeviceOrientation.LandscapeLeft)
        {
            return DeviceOrientation.LandscapeRight;
        }
        if (defaultOrientation == DeviceOrientation.LandscapeRight)
        {
            return DeviceOrientation.LandscapeLeft;
        }
        return DeviceOrientation.Unknown;
    }

    public void TurnOnCamera()
    {
        webCamTexture = new WebCamTexture();
        var lastDevice = WebCamTexture.devices[WebCamTexture.devices.Length - 1].name;
        webCamTexture.deviceName = lastDevice;
        webCamTexture.Play();
    }

    public void CycleToNextDevice()
    {
        webCamTexture.Stop();
        var currentIndex = webCamDevices.IndexOf(webCamDevices.First(x => x.name.Equals(webCamTexture.deviceName)));
        ++currentIndex;
        if (currentIndex >= webCamDevices.Count)
        {
            currentIndex = 0;
        }
        webCamTexture.deviceName = webCamDevices[currentIndex].name;
        webCamTexture.Play();
    }

    public void TurnOffCamera()
    {
        webCamTexture.Stop();
    }

    public void TakePhoto()
    {
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        if (takenPhotoTexture2D != null) takenPhotoTexture2D.Image = photo;
    }
}
