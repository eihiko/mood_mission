using UnityEngine;
using System.Collections;
using Globals;

public class IntroAudio : MonoBehaviour
{
    public int SecondsBetweenRepeat;
    private bool screenClicked = false;
    private int counter = 0;

    public void ScreenClicked()
    {
        if (screenClicked) return;
        screenClicked = true;
        StartCoroutine(StartAudio(SecondsBetweenRepeat));
    }

    IEnumerator StartAudio(int seconds)
    {
        while (screenClicked)
        {
            if (counter >= 4)
            {
                screenClicked = false;
                counter = 0;
            }
            Utilities.PlayAudio(GetComponent<AudioSource>());
            yield return new WaitForSeconds(seconds);
            ++counter;
        }
    }
}
