using System.Collections;
using UnityEngine;

namespace SadScene
{
    public class ConeDisappear : MonoBehaviour
    {
        private bool shouldSink = false;
        private ConeManager coneManager;
        private ObjectSequenceManager ballMissManager;

        private void Start()
        {
            coneManager = transform.parent.GetComponent<ConeManager>();
            ballMissManager = coneManager.ballMissManager;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.name.ToLower().Equals("soccerball") && !shouldSink)
            {
                // not the first cone then play audio
                if (!gameObject.name.Contains("1"))
                    Utilities.PlayRandomAudio(transform.parent.FindChild("Audio").GetComponentsInChildren<AudioSource>());
                shouldSink = true;
                StartCoroutine(HideObject(other.transform));
            }
        }

        private IEnumerator HideObject(Transform soccerBall)
        {
            yield return new WaitForSeconds(1f);
            GetComponent<BoxCollider>().enabled = false;
            coneManager.NextInSequence();
            soccerBall.transform.position = new Vector3(soccerBall.transform.position.x,
                soccerBall.transform.position.y,
                coneManager.RandomizePositionZ());
            ballMissManager.NextInSequence();
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}