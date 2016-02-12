using UnityEngine;
using System.Linq;
using Globals;
using UnityEngine.UI;

public class CompletedScenes : MonoBehaviour {

    private void Start()
    {
        ShowCompletedScenes();
        Utilities.PlayAudio(GameObject.Find("IntroAudio").GetComponent<AudioSource>());
        Timeout.StartTimers();
        Timeout.SetRepeatAudio(GameObject.Find("IntroAudio").GetComponent<AudioSource>());
    }

    private void ShowCompletedScenes()
    {
        foreach (var scene in Scenes.CompletedScenes)
        {
            var completedScene = GameObject.Find(scene);
            if (completedScene != null)
            {
                completedScene.GetComponent<Button>().enabled = false;
                completedScene.GetComponent<ButtonSceneLoad>().enabled = false;
                completedScene.transform.FindChild("Checkmark").gameObject.SetActive(true);
                completedScene.transform.FindChild("Text").GetComponent<RawImage>().color = new Color(0.66f, 0.66f, 0.66f, 1f);
            }
        }
    }
}
