using UnityEngine;
using System.Collections;
using Globals;

namespace SadScene
{
    public class OutsideGroupDialogue : MonoBehaviour
    {
        public AudioSource canIPlay;
        public AudioSource noneAreHere;
        public GroupDialogue otherCharacter;
        public CameraFollow cameraFollow;

        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void StartDialogue()
        {
            GetComponent<OutsideGroupSoccerAnimation>().tutorial.DisableHelpGUI();
            GroupDialogue.shouldStopPlaying = true;
            anim.SetTrigger("CanIPlay");
            StartCoroutine(DelayPlayAudio(canIPlay));
        }

        private IEnumerator DelayPlayAudio(AudioSource source)
        {
            Timeout.StopTimers();
            yield return new WaitForSeconds(1f);
            Utilities.PlayAudio(source);
        }

        public void ExplainCantPlay()
        {
            anim.SetTrigger("Idle");
            otherCharacter.ExplainCantPlay();
        }

        public void NoneAreHereDialogue()
        {
            anim.SetTrigger("NoneAreHere");
            Utilities.PlayAudio(noneAreHere);
        }

        public void DontGetToPlay()
        {
            anim.SetTrigger("Idle");
            otherCharacter.DontGetToPlay();
        }

        public void WalkAway()
        {
            anim.SetTrigger("WalkAway");
            cameraFollow.enabled = true;
        }

        public WaitForSeconds PlayDialogue(AudioSource dialogue)
        {
            anim.SetTrigger("Talk");
            Utilities.PlayAudio(dialogue);
            return new WaitForSeconds(dialogue.clip.length);
        }

        public void TriggerIdleAnimation()
        {
            anim.SetTrigger("Idle");
        }
    }
}