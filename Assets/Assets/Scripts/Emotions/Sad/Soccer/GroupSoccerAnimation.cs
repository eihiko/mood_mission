using System;
using UnityEngine;
using System.Collections;

namespace SadScene
{
    public class GroupSoccerAnimation : MonoBehaviour
    {
        public GroupSoccerBallMovement soccerBall;
        public Transform head;

        private Animator anim;
        private Action<Animator> animationTrigger;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void KickForward()
        {
            stopBallAnimation(animator => { anim.SetTrigger("KickForward"); });
        }

        public void KickForwardEvent()
        {
            soccerBall.KickBallForward(transform.forward.normalized);
            StartCoroutine(RestoreCollider());
        }

        public void KickLeft()
        {
            stopBallAnimation(animator => { anim.SetTrigger("KickLeft"); });
        }

        public void KickLateralEvent()
        {
            soccerBall.KickBallLateral(head.forward);
            StartCoroutine(RestoreCollider());
        }

        public void KickRight()
        {
            stopBallAnimation(animator => { animator.SetTrigger("KickRight"); });
        }

        public void StopBallEvent()
        {
            soccerBall.NeutralizeForce();
            if (GroupDialogue.shouldStopPlaying)
            {
                anim.SetTrigger("Idle");
                return;
            }
            if (animationTrigger != null) animationTrigger(anim);
        }

        private void stopBallAnimation(Action<Animator> trigger)
        {
            animationTrigger = trigger;
            anim.SetTrigger("StopBall");
        }

        private IEnumerator RestoreCollider()
        {
            yield return new WaitForSeconds(1f);
            GetComponent<CapsuleCollider>().enabled = true;
            animationTrigger = null;
            anim.SetTrigger("Idle");
        }

        public void ResetCapsuleColliders()
        {
            GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}

