using Globals;
using PuzzleMiniGame;
using UnityEngine;
using UnityEngine.UI;

public class StartGenerating : CameraActions
{
    public WebCamPhotoCamera webCam;
    public CreatePuzzleGrid gridGenerator;

    public override void RunPostPictureActions()
    {
        webCam.TurnOffCamera();
        webCam.gameObject.SetActive(false);
        gridGenerator.StartGeneratingGrid();
        gameObject.SetActive(false);
    }

    public override void RunPrePictureActions()
    {
        Timeout.StopTimers();
        GetComponent<Button>().interactable = false;
    }
}
