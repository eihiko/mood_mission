using System.Collections;
using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.UI;

namespace ScaredScene
{
    public class AskToSolve : DefaultActionBase
    {
        public ExplainFear fearfulCharacter;
        public AudioSource switchToChildAudio;
        public GameObject PASSLetter;
        public GameObject[] PASSLetters;

        private GameObject parentToChildImage;

        protected void Start()
        {
            parentToChildImage = GameObject.Find("PassTabletCanvas").transform.FindChild("ParentToChild").gameObject;
        }

        protected override void DialogueAnimation()
        {
            base.DialogueAnimation();
            GameFlags.HasSeenPASS = true;
            PASSLetters.ToList().ForEach(x => x.SetActive(false));
            anim.SetTrigger("Talk");
        }

        protected override void AfterDialogue()
        {
            base.AfterDialogue();
            anim.SetTrigger("Idle");
            sceneReset.sceneToLoadIncorrect = "ScaredSceneSmallCitySituationActionsMenu";
            switchBackToChild();
        }

        private void switchBackToChild()
        {
            parentToChildImage.SetActive(true);
            Utilities.PlayAudio(switchToChildAudio);
        }

        protected override void BeforeExplanation()
        {
            base.BeforeExplanation();
            PASSLetter.SetActive(true);
            PASSLetter.GetComponent<Animator>().SetTrigger("BlowUp");
        }

        protected override IEnumerator BeforeAdditionalExplanationCoroutine()
        {
            PASSLetter.GetComponent<Animator>().SetTrigger("Empty");
            var list = PASSLetters.ToList();
            foreach (var letter in list)
            {
                letter.GetComponent<Animator>().SetTrigger("BlowUp");
                var letterAudio = letter.GetComponent<AudioSource>();
                Utilities.PlayAudio(letterAudio);
                yield return new WaitForSeconds(letterAudio.clip.length);
            }
            yield return base.BeforeAdditionalExplanationCoroutine();
        }
    }
}