using UnityEngine;
using System.Collections;

namespace AngryScene
{
    public class PlayAudioOnEvent : MonoBehaviour
    {
        public AudioSource audioToPlay;

        public void PlayAudioEvent()
        {
            StartCoroutine(playAudio());
        }

        private IEnumerator playAudio()
        {
            Utilities.PlayAudio(audioToPlay);
            yield return new WaitForSeconds(audioToPlay.clip.length);
            GUIHelper.NextGUI();
        }
    }
}