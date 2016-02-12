using UnityEngine;
using System.Collections;
using Globals;

public class Pause : MonoBehaviour
{
    public GameObject debugOptions;
    public SceneLoader sceneLoaderButton;
    private float originalScale;
    private bool paused = false;

    private void Start()
    {
        originalScale = Time.timeScale;
    }

    public void OnClick()
    {
        if (paused) UnPauseGame();
        else pauseGame();
    }

    private void pauseGame()
    {
        Time.timeScale = 0;
        paused = true;
        debugOptions.SetActive(true);
        sceneLoaderButton.Initialize();
        Timeout.StopTimers();
        Utilities.PauseAudio(Sound.CurrentPlayingSound);
    }

    public void UnPauseGame()
    {
        Time.timeScale = originalScale;
        paused = false;
        debugOptions.SetActive(false);
        Timeout.StartTimers();
        Utilities.UnPauseAudio(Sound.CurrentPlayingSound);
    }
}
