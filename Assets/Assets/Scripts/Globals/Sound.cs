using UnityEngine;
using System.Collections;

public static class Sound
{
    public static AudioSource CurrentPlayingSound;

    public static void ResetValues()
    {
        CurrentPlayingSound = null;
    }
}
