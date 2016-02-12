using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Globals;
using UnityEngine.UI;

namespace ScaredScene
{
    public class ExplainFear : ActionBase
    {
        public Animator otherAnim;
        public AudioSource scaredDialogue;
        public AudioSource afraidToFallDialogue;
        public AudioSource switchToParentAudio;

        private GameObject childToParentImage;

        private void Start()
        {
            childToParentImage = GameObject.Find("PassTabletCanvas").transform.FindChild("ChildToParent").gameObject;
        }

        private void VoiceFear()
        {
            anim.SetTrigger("Talking");
            Utilities.PlayAudio(scaredDialogue);
        }

        public void GetEncouragement()
        {
            anim.SetTrigger("Idle");
            if (GameFlags.AdultIsPresent)
            {
                sceneReset.sceneToLoadIncorrect = "ScaredSceneSmallCityParentPayAttentionAskActionsMenu";
                switchToParent();
            }
            else
            {
                otherAnim.GetComponent<Conversation>().GiveEncouragement();
            }
        }

        private void switchToParent()
        {
            childToParentImage.SetActive(true);
            Utilities.PlayAudio(switchToParentAudio);
        }

        public void StartJumpSequence()
        {
            gameObject.GetComponent<FearfulMovement>().WalkBackwards();
        }

        public void AfraidToFall()
        {
            anim.SetTrigger("ScaredToFall");
            Utilities.PlayAudio(afraidToFallDialogue);
        }

        public void GetComfort()
        {
            anim.SetTrigger("Idle");
            otherAnim.GetComponent<Conversation>().GiveComfort();
        }

        public override void StartAction()
        {
            base.StartAction();
            ShowCorrect(true);
            StartCoroutine(Explain());
        }

        private IEnumerator Explain()
        {
            Utilities.PlayAudio(actionExplanation);
            yield return new WaitForSeconds(actionExplanation.clip.length);
            ShowCorrect(false);
            VoiceFear();
        }

        public void GoToMiniGame()
        {
            sceneReset.TriggerCorrect(null, Scenes.GetNextMiniGame(), false);
        }
    }
}