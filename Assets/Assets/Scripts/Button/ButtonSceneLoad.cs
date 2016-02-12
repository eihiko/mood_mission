using Globals;
using UnityEngine;

public class ButtonSceneLoad : ButtonSelect {

    public string sceneToLoad;
    public bool shouldAskParentPresent = false;
    public bool loadingEmotionScene = true;

    protected override void DoubleClickAction()
    {
        Timeout.StopTimers();
        if (string.IsNullOrEmpty(sceneToLoad)) return;
        if (GameObject.Find("MainCanvas") != null)
            Utilities.StopAudio(GameObject.Find("MainCanvas").GetComponent<AudioSource>());
        if (shouldAskParentPresent)
        {
            Scenes.NextSceneToLoad = sceneToLoad;
            Utilities.LoadScene("ParentPresentMenuScreen");
        }
        else
        {
            if (loadingEmotionScene)
                Utilities.LoadEmotionScene(sceneToLoad);
            else
                Utilities.LoadScene(sceneToLoad);
        }
    }

    protected override void Update()
    {
        // disable flashing
    }
}
