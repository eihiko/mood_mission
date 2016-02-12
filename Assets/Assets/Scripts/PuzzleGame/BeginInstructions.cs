using System.Collections;
using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.UI;

public class BeginInstructions : MonoBehaviour
{
    public AudioSource getFaceIntoOvalAudio;
    public AudioSource pictureCountDownAudio;
    public AudioSource cameraButtonAudio;
    public GameObject disablePanel;
    public GameObject cameraButtonArrow;
    public GameObject[] kidsFacesImages;
    
    [HideInInspector]
    public GameObject picturesToShow;

	void Start ()
	{
        Timeout.StopTimers();
        StartCoroutine(faceInstructions());
	}

    private IEnumerator faceInstructions()
    {
        yield return new WaitForSeconds(1.5f);

        var makingFacesAudioList = transform.FindChild("Audio")
            .FindChild("MakingFaces")
            .GetComponentsInChildren<AudioSource>().ToList();
        var makingFacesAudio = makingFacesAudioList.First(
            x => Scenes.GetLastSceneCompleted().ToLower().Contains(x.gameObject.name.ToLower()));
        Utilities.PlayAudio(makingFacesAudio);
        picturesToShow =
            kidsFacesImages.First(x => Scenes.GetLastSceneCompleted().ToLower().Contains(x.gameObject.name.ToLower()));
        picturesToShow.SetActive(true);
        yield return new WaitForSeconds(makingFacesAudio.clip.length);

        var emotionInstructions = transform.FindChild("Audio")
            .FindChild("Emotions")
            .GetComponentsInChildren<AudioSource>().ToList();
        var makeFaceInstruction =
            emotionInstructions.First(
                x => Scenes.GetLastSceneCompleted().ToLower().Contains(x.gameObject.name.ToLower()));

        Utilities.PlayAudio(makeFaceInstruction);
        yield return new WaitForSeconds(makeFaceInstruction.clip.length);
        yield return StartCoroutine(positionInstructions());
    }

    private IEnumerator positionInstructions()
    {
        if (!GameFlags.CameraTutorialHasRun)
        {
            Utilities.PlayAudio(getFaceIntoOvalAudio);
            yield return new WaitForSeconds(getFaceIntoOvalAudio.clip.length);

            Utilities.PlayAudio(cameraButtonAudio);
            cameraButtonArrow.SetActive(true);
            yield return new WaitForSeconds(cameraButtonAudio.clip.length);
            cameraButtonArrow.SetActive(false);
        }
        disablePanel.SetActive(false);
        GameFlags.CameraTutorialHasRun = true;
        Timeout.SetRepeatAudio(cameraButtonAudio);
        Timeout.StartTimers();
    }
}
