using System.Collections;
using Globals;
using UnityEngine;

namespace PuzzleTutorial
{
    public class PressPlay : TutorialAction
    {
        public override void DoAction()
        {
            StartCoroutine(DelayPressPlay());
        }

        private IEnumerator DelayPressPlay()
        {
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
            GUIManager.Instance.PlayLevel(true);
        }
    }
}