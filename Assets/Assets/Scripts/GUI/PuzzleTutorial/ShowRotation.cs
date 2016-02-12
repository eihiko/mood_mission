using UnityEngine;
using System.Collections;

namespace PuzzleTutorial
{
    public class ShowRotation : TutorialAction
    {
        public ObjectBase practiceTool;

        public override void DoAction()
        {
            FeedbackManager.Instance.Setup(practiceTool, FeedbackManager.TargetState.rotating);
            StartCoroutine(HideAnimation());
        }

        private IEnumerator HideAnimation()
        {
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
            gameObject.SetActive(false);
        }
    }
}