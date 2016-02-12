using UnityEngine;
using System.Collections;
using PuzzleTutorial;

namespace PuzzleTutorial
{
    public class ShowRedRotation : TutorialAction
    {
        public ObjectBase practiceTool;

        public override void DoAction()
        {
            FeedbackManager.Instance.feedback.sprite = FeedbackManager.Instance.RotationRedTexture;
            practiceTool.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
            StartCoroutine(PositionPracticeObject());
        }

        private IEnumerator PositionPracticeObject()
        {
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
            FeedbackManager.Instance.Disable(0.2f);
            practiceTool.transform.rotation = Quaternion.Euler(0f, 0f, 17.24014f);
        }
    }
}