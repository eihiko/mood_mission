using UnityEngine;

namespace ScaredScene
{
    public class Support : DefaultActionBase
    {
        public GameObject PASSLetter;

        protected override void DialogueAnimation()
        {
            anim.SetTrigger("Talk");
            anim.speed = dialogue.clip.length/anim.GetCurrentAnimatorStateInfo(0).length;
        }

        protected override void AfterDialogue()
        {
            anim.speed = 1;
            anim.SetTrigger("Idle");
            sceneReset.sceneToLoadIncorrect = "ScaredSceneSmallCityParentSolveActionsMenu";
            GUIHelper.NextGUI();
        }

        protected override void BeforeExplanation()
        {
            base.BeforeExplanation();
            PASSLetter.SetActive(true);
            PASSLetter.GetComponent<Animator>().SetTrigger("BlowUp");
        }

        protected override void BeforeAdditionalExplanation()
        {
            base.BeforeAdditionalExplanation();
            PASSLetter.GetComponent<Animator>().SetTrigger("Empty");
        }
    }
}