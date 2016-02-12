using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.UI;

public class WebCamPhotoCamera : WebCam
{
    public AudioSource pictureCountDownAudio;
    public AudioSource cameraShutterAudio;
    public AudioSource keepPictureAudio;
    public CameraActions cameraActions;
    public GameObject[] buttonsToDisable;
    public GameObject[] buttonsToEnable;
    public BeginInstructions instructions;

    protected override void Start()
    {
        base.Start();
        TurnOnCamera();
    }

    public void TakePhoto()
    {
        if (!webCamTexture.isPlaying) TurnOnCamera();
        Utilities.StopAudio(Sound.CurrentPlayingSound);
        setInteractable(buttonsToEnable, false);
        StopAllCoroutines();
        StartCoroutine(StartTakingPhoto());
    }

    private IEnumerator StartTakingPhoto()
    {
        cameraActions.RunPrePictureActions();
        Utilities.PlayAudio(pictureCountDownAudio);
        yield return new WaitForSeconds(pictureCountDownAudio.clip.length + 0.5f);
        Utilities.PlayAudio(cameraShutterAudio);
        yield return new WaitForSeconds(cameraShutterAudio.clip.length);

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        if (takenPhotoTexture2D != null) takenPhotoTexture2D.Image = photo;
        
        showYesNoButtons();
        TurnOffCamera();
        GetComponent<RawImage>().texture = photo;

        Utilities.PlayAudio(keepPictureAudio);
        yield return new WaitForSeconds(keepPictureAudio.clip.length);
        Timeout.SetRepeatAudio(keepPictureAudio);
        Timeout.StartTimers();
    }

    public void KeepPhoto()
    {
        StopAllCoroutines();
        instructions.picturesToShow.SetActive(false);
        Utilities.StopAudio(Sound.CurrentPlayingSound);
        setAllButtons(buttonsToEnable, false);
        cameraActions.RunPostPictureActions();
    }

    private void setAllButtons(GameObject[] buttons, bool setActive)
    {
        foreach (var button in buttons)
        {
            button.SetActive(setActive);
        }
    }

    private void setInteractable(GameObject[] buttons, bool interactable)
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Button>().interactable = interactable;
        }
    }

    private void showYesNoButtons()
    {
        setAllButtons(buttonsToDisable, false);
        setAllButtons(buttonsToEnable, true);
        setInteractable(buttonsToEnable, true);
    }
}
