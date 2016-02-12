using UnityEngine;
using System.Collections;
using Globals;
using UnityEngine.UI;

namespace SadScene
{
    public class ConveySadness : CorrectActionBase
    {
        public AudioSource switchToParentAudio;

        private GameObject childToParentImage;

        protected void Start()
        {
            childToParentImage = GameObject.Find("PassTabletCanvas").transform.FindChild("ChildToParent").gameObject;
        }

        protected override void DialogueAnimation()
        {
            base.DialogueAnimation();
            anim.SetTrigger("Talk");
        }

        protected override void AfterDialogue()
        {
            base.AfterDialogue();
            anim.SetTrigger("Idle");
            sceneReset.sceneToLoadIncorrect = "SadSceneSmallCityParentPayAttentionAskActionsMenu";
            switchToParent();
        }

        private void switchToParent()
        {
            if (GameFlags.AdultIsPresent)
            {
                childToParentImage.SetActive(true);
                Utilities.PlayAudio(switchToParentAudio);
            }
            else
            {
                GUIHelper.NextGUI();
            }
        }
    }
}