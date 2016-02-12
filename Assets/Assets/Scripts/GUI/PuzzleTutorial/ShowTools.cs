using UnityEngine;
using System.Collections;

namespace PuzzleTutorial
{
    public class ShowTools : TutorialAction
    {
        public ObjectBase practiceTool;

        public override void DoAction()
        {
            ToolboxManager.Instance.ButtonPressed();
            GetComponent<Animator>().enabled = true;
            StartCoroutine(HideTools());
        }

        private IEnumerator HideTools()
        {
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
            ToolboxManager.Instance.ButtonPressed();
            practiceTool.gameObject.SetActive(true);
            practiceTool.transform.position = new Vector3(.93f, 2.65f, 0f);
            GetComponent<Animator>().enabled = false;
        }
    }
}