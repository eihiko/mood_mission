using UnityEngine;
using System.Collections;
using System.Linq;

namespace SadScene
{
    public class ForceIntoGame : ActionBase
    {
        public GameObject[] parents;
        public GameObject[] otherPlayers;
        public Transform groupSoccerBall;

        public override void StartAction()
        {
            base.StartAction();
            TurnAround();
        }

        private void TurnAround()
        {
            anim.SetTrigger("ForceIntoGame");
            GetComponent<OutsideGroupSoccerAnimation>().HideSoccerBall();
            transform.FindChild("CameraFollow").GetComponent<CameraFollow>().enabled = true;
            GroupDialogue.shouldStopPlaying = true;
            groupSoccerBall.localPosition = new Vector3(193.386f, 3.9f, 80.403f);
            groupSoccerBall.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            groupSoccerBall.GetComponent<GroupSoccerBallMovement>().NeutralizeForce();
            groupSoccerBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            otherPlayers.ToList().ForEach(x => x.GetComponent<CapsuleCollider>().enabled = false);
        }

        public void RunTowardsOthersEvent()
        {
            parents.ToList().ForEach(x => x.GetComponent<BoxCollider>().enabled = false);
            anim.SetBool("Run", true);
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<OutsideGroupSoccerAnimation>().SetWalkAwaySpeed(true, 2f, 0f);
        }

        public void ForceKickBallForward()
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<OutsideGroupSoccerAnimation>().SetWalkAwaySpeed(false, 0f, 0f);
            transform.FindChild("CameraFollow").GetComponent<CameraFollow>().enabled = false;
            anim.SetBool("Run", false);
            anim.SetTrigger("KickForward");
        }

        public void ForceKickBallForwardEvent()
        {
            groupSoccerBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            groupSoccerBall.GetComponent<GroupSoccerBallMovement>().KickBallForward(new Vector3(1f, 0.72f, 0f), 250f);
        }

        public void ShiftIdleEvent()
        {
            anim.SetTrigger("Idle");
            TriggerIncorrect();
        }
    }
}