using System.Collections;
using Globals;
using UnityEngine;

public class PassTablet : MonoBehaviour
{
    public GameObject arrows;

    private void Start()
    {
        StartCoroutine(playInstructions());
    }

    private IEnumerator playInstructions()
    {
        yield return new 
            WaitForSeconds(Sound.CurrentPlayingSound.clip.length - Sound.CurrentPlayingSound.time);
        Utilities.PlayAudio(GetComponent<AudioSource>());
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        arrows.SetActive(true);
        Timeout.SetRepeatAudio(GetComponent<AudioSource>());
        Timeout.StartTimers();
    }

    public void ImageClicked()
    {
        GUIHelper.NextGUI();
        arrows.SetActive(false);
        gameObject.SetActive(false);
    }
}
