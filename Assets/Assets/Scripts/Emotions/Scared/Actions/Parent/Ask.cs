using UnityEngine;
using System.Collections;
using System.Linq;
using Globals;

namespace ScaredScene
{
    public class Ask : DefaultActionBase
    {
        public Conversation child;
        public AudioSource itsTooFar;
        public GameObject[] PASSLetters;

        protected override void DialogueAnimation()
        {
            base.DialogueAnimation();
            PASSLetters.ToList().First(x => x.name.ToLower().Equals("ask")).GetComponent<Animator>().SetTrigger("Empty");
            anim.SetTrigger("Talk");
        }

        protected override void AfterDialogue()
        {
            base.AfterDialogue();
            anim.SetTrigger("Idle");
            sceneReset.sceneToLoadIncorrect = "ScaredSceneSmallCityParentSupportActionsMenu";
            StartCoroutine(NextGUI());
        }

        protected override void BeforeExplanation()
        {
            base.BeforeExplanation();
            PASSLetters.ToList().ForEach(x => x.SetActive(true));
            PASSLetters.ToList().First(x => x.name.ToLower().Equals("payattention")).GetComponent<Animator>().SetTrigger("BlowUp");
        }

        protected override void BeforeAdditionalExplanation()
        {
            base.BeforeAdditionalExplanation();
            PASSLetters.ToList().First(x => x.name.ToLower().Equals("payattention")).GetComponent<Animator>().SetTrigger("Empty");
            PASSLetters.ToList().First(x => x.name.ToLower().Equals("ask")).GetComponent<Animator>().SetTrigger("BlowUp");
        }

        private IEnumerator NextGUI()
        {
            yield return child.PlayDialogue(itsTooFar);
            child.other.SetTrigger("Idle");
            GUIHelper.NextGUI();
        }
    }
}