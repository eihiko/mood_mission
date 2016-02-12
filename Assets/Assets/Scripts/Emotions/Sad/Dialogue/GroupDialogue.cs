using UnityEngine;
using System.Collections;

namespace SadScene
{
    public class GroupDialogue : MonoBehaviour
    {
        public AudioSource cantPlay;
        public AudioSource dontGetToPlay;
        public OutsideGroupDialogue otherCharacter;
        public static bool shouldStopPlaying;

        private Animator anim;
        private Quaternion originalRotation;

        private void Start()
        {
            anim = GetComponent<Animator>();
            shouldStopPlaying = true;
            originalRotation = transform.rotation;
        }

        public void ExplainCantPlay()
        {
            anim.applyRootMotion = true;
            GetComponent<CapsuleCollider>().enabled = false;
            anim.SetTrigger("CantPlay");
        }

        public void TriggerCantPlayDialogue()
        {
            anim.SetTrigger("CantPlay");
            Utilities.PlayAudio(cantPlay);
        }

        public void NoneAreHereDialogue()
        {
            anim.SetTrigger("Idle");
            otherCharacter.NoneAreHereDialogue();
        }

        public void DontGetToPlay()
        {
            Utilities.PlayAudio(dontGetToPlay);
            anim.SetTrigger("DontGetToPlay");
        }

        public void TriggerWalkAway()
        {
            anim.SetTrigger("Idle");
            anim.SetTrigger("TurnToFriends");
            otherCharacter.WalkAway();
        }

        public void ResumePlaying()
        {
            transform.rotation = originalRotation;
            anim.SetTrigger("Idle");
            anim.applyRootMotion = false;
            shouldStopPlaying = false;
            var animationScripts = transform.parent.GetComponentsInChildren<GroupSoccerAnimation>();
            foreach (var groupSoccerAnimation in animationScripts)
            {
                groupSoccerAnimation.ResetCapsuleColliders();
            }
        }
    }
}