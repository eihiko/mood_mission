using UnityEngine;
using System.Collections;
using Globals;

namespace PuzzleTutorial
{
    public class RunTutorial : MonoBehaviour
    {
        public Transform[] beginningMarkers;
        public Transform[] markers;
        public Transform[] endMarkers;
        public AudioSource repeatAudioGame;
        public AudioSource repeatAudioMenu;

        // Use this for initialization
        private void Start()
        {
            GUIManager.Instance.enabled = false;
            GameObject.Find("Overlord").GetComponent<InputManager>().enabled = false;
            StartCoroutine(ShowMarkers());
            StartCoroutine(ShowMarkersExtra(markers));
            Timeout.SetRepeatAudio(repeatAudioGame);
            Timeout.StopTimers();
        }

        public void ExplainEndButtons()
        {
            Timeout.SetRepeatAudio(repeatAudioMenu);
            if (!GameFlags.PuzzleTutorialHasRun)
            {
                StartCoroutine(ShowMarkersExtra(endMarkers));
                GameFlags.PuzzleTutorialHasRun = true;
            }
            else
            {
                var finishRestart = transform.Find("Finish").FindChild("FinishRestart").gameObject;
                if (finishRestart != null) finishRestart.SetActive(false);
            }
            GameObject.Find("Overlord").GetComponent<InputManager>().enabled = true;
            if (endMarkers.Length != 0) Timeout.StartTimers();
        }

        public void HideTutorialShelf()
        {
            var shelfTutorial = GameObject.Find("ShelfTutorial");
            if (shelfTutorial != null) shelfTutorial.SetActive(false);
        }

        IEnumerator ShowMarkers()
        {
            yield return new WaitForSeconds(1);

            //Scale up the markers
            StartCoroutine(ScaleObject(beginningMarkers[0], Vector2.one, 0.25f, 0));
            StartCoroutine(ScaleObject(beginningMarkers[1], Vector2.one, 0.25f, 0));
            Utilities.PlayAudio(GetComponent<AudioSource>());

            yield return new WaitForSeconds(GetComponent<AudioSource>() != null ? GetComponent<AudioSource>().clip.length : 2);

            //Scale down the markersto zero
            beginningMarkers[0].localScale = Vector2.zero;
            beginningMarkers[1].localScale = Vector2.zero;

            if (markers.Length == 0 && beginningMarkers.Length != 0)
            {
                GUIManager.Instance.enabled = true;
                GameObject.Find("Overlord").GetComponent<InputManager>().enabled = true;
                Timeout.StartTimers();
            }
        }

        IEnumerator ShowMarkersExtra(Transform[] markersToShow)
        {
            yield return new WaitForSeconds(2);

            foreach (Transform marker in markersToShow)
            {
                yield return new WaitForSeconds(1);

                //Scale up the markers
                StartCoroutine(ScaleObject(marker, Vector2.one, 0.25f, 0));
                var markerAudio = marker.GetComponent<AudioSource>();
                var action = marker.GetComponent<TutorialAction>();
                if (action != null) action.DoAction();
                Utilities.PlayAudio(markerAudio);

                yield return new WaitForSeconds(markerAudio.clip.length);

                //Scale down the markersToShow to zero
                marker.localScale = Vector2.zero;
            }
            GUIManager.Instance.enabled = true;
            if (endMarkers.Length == 0 && markers.Length != 0)
            {
                GameObject.Find("Overlord").GetComponent<InputManager>().enabled = true;
                Timeout.StartTimers();
            }
        }

        IEnumerator ScaleObject(Transform obj, Vector2 end, float time, float delay)
        {
            yield return new WaitForSeconds(delay);

            Vector2 originalScale = obj.localScale;

            float rate = 1.0f / time;
            float i = 0.0f;

            while (i < 1.0f)
            {
                i += Time.deltaTime * rate;
                obj.localScale = Vector2.Lerp(originalScale, end, i);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}