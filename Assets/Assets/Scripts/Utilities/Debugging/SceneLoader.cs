using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Pause pauseButton;
    public Dropdown scenes;
    public Toggle[] flags;
    public Dropdown[] stringFlags;
    public Dropdown lastSceneCompleted;
    public static string sceneToLoad;

    public void Initialize()
    {
        sceneToLoad = Application.loadedLevelName;
        updateUI();
    }

    private void updateUI()
    {
        flags.ToList().ForEach(flag =>
        {
            var flagText = flag.name;
            var field = typeof(GameFlags).GetField(flagText);
            flag.isOn = (bool)field.GetValue(field);
        });
        stringFlags.ToList().ForEach(flag =>
        {
            var flagText = flag.name;
            var field = typeof(GameFlags).GetField(flagText);
            flag.value = flag.options.FindIndex(x => x.text.Equals((string)field.GetValue(field)));
        });
        lastSceneCompleted.value = lastSceneCompleted.options.FindIndex(x => x.text.Equals(Scenes.GetLastSceneCompleted()));
    }

    public void LoadScene()
    {
        pauseButton.UnPauseGame();
        flags.ToList().ForEach(flag =>
        {
            var field = typeof(GameFlags).GetField(flag.name);
            field.SetValue(field, flag.isOn);
        });
        stringFlags.ToList().ForEach(flag =>
        {
            var field = typeof(GameFlags).GetField(flag.name);
            field.SetValue(field, flag.options[flag.value].text);
        });
        Scenes.ResetValues();
        Scenes.LoadingSceneThroughDebugging = true;
        Scenes.CompletedScenes.Add(lastSceneCompleted.options[lastSceneCompleted.value].text);
        Utilities.LoadScene(sceneToLoad);
    }

    public void UpdateSceneToLoad()
    {
        sceneToLoad = scenes.options[scenes.value].text;
    }
}
