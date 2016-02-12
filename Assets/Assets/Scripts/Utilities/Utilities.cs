using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Globals;

public class Utilities : MonoBehaviour {

    private static readonly List<string> sceneFilters = new List<string>
    {
        "ActionsMenu", 
        "Situation", 
        "Emotion", 
        "Parent",
        "PayAttentionAsk",
        "Solve",
        "Support"
    }; 

    public static void PlayAudio(AudioSource audioSource, bool stopCurrentAudio = true)
    {
        if (audioSource != null)
        {
            if (stopCurrentAudio) StopAudio(Sound.CurrentPlayingSound);
            Sound.CurrentPlayingSound = audioSource;
            audioSource.Play();
        }
    }

    public static void PlayAudioUnBlockable(AudioSource audioSource, bool stopCurrentAudio = false)
    {
        if (audioSource != null)
        {
            if (stopCurrentAudio) StopAudio(Sound.CurrentPlayingSound);
            audioSource.Play();
        }
    }

    public static void PauseAudio(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public static void UnPauseAudio(AudioSource audioSource)
    {
        if (audioSource != null && Sound.CurrentPlayingSound == audioSource && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    public static void StopAudio(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
    }

    public static void LoadEmotionScene(string sceneToLoad)
    {
        CoinPile.ResetCoinPileSize();
        if (CityInitializer.City != null)
        {
            CityInitializer.City.SetActive(true);
            PlayAudioUnBlockable(CityInitializer.City.GetComponent<AudioSource>());
        }
        LoadScene(sceneToLoad);
    }

    public static void LoadScene(string sceneToLoad)
    {
        Timeout.StopTimers();
        Timeout.Instance.StopAllCoroutines();
        CanvasList.ResetIndex();
        StopAudio(Sound.CurrentPlayingSound);
        if (sceneToLoad.ToLower().Contains("mainmenu"))
        {
            CityInitializer.City.SetActive(false);
            StopAudio(CityInitializer.City.GetComponent<AudioSource>());
        }
        if (sceneToLoad.ToLower().Contains("minigame") && !Application.loadedLevelName.ToLower().Contains("minigame"))
        {
            CityInitializer.City.SetActive(false);
            StopAudio(CityInitializer.City.GetComponent<AudioSource>());
            var sceneName = Application.loadedLevelName;
            sceneFilters.ForEach(f => sceneName = sceneName.Replace(f, ""));
            if (!Scenes.CompletedScenes.Contains(sceneName) && !Scenes.LoadingSceneThroughDebugging) 
                Scenes.CompletedScenes.Add(sceneName);
        }
        Scenes.LoadingSceneThroughDebugging = false;
        GameObject.Find("LoadingIndicatorCanvas").GetComponent<Canvas>().enabled = true;
        if (sceneToLoad != "") Timeout.Instance.StartCoroutine(loadLevelAsync(sceneToLoad));
    }

    private static IEnumerator loadLevelAsync(string sceneToLoad)
    {
        yield return Application.LoadLevelAsync(sceneToLoad);
    }

    public static AudioSource PlayRandomAudio(IList<AudioSource> audioSources)
    {
        var audioToPlay = audioSources[Random.Range(0, audioSources.Count())];
        PlayAudio(audioToPlay);
        return audioToPlay;
    }
}
