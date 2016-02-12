using UnityEngine;
using System.Collections;

namespace SadScene
{
    public class YellAtOthers : ActionBase
    {
        public AudioSource yellAtOthersDialogue;

        public override void StartAction()
        {
            base.StartAction();
            TurnAround();
        }

        private void TurnAround()
        {
            anim.SetTrigger("Yell");
        }

        public void StartYellingEvent()
        {
            anim.SetTrigger("Yell");
            StartCoroutine(PlayYellingAudio());
        }

        public void IdleFromYellingEvent()
        {
            anim.SetTrigger("Idle");
        }

        private IEnumerator PlayYellingAudio()
        {
            Utilities.PlayAudio(yellAtOthersDialogue);
            yield return new WaitForSeconds(yellAtOthersDialogue.clip.length);
            TriggerIncorrect();
        }
    }
}