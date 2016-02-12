using UnityEngine;
using System.Collections;

namespace SadScene
{
    public class PlayAlone : ActionBase
    {
        public OutsideGroupSoccerBallMovement soccerBall;

        public override void StartAction()
        {
            base.StartAction();
            TurnAround();
        }

        private void TurnAround()
        {
            anim.SetTrigger("PlayAlone");
            soccerBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            soccerBall.GetComponent<Rigidbody>().angularDrag = 20f;
            soccerBall.transform.position = new Vector3(soccerBall.transform.position.x, soccerBall.transform.position.y, 80.335f);
            var force = new Vector3(-363f, 0f, 0f);
            soccerBall.ApplyForce(force);
        }

        public void PlayAloneEvent()
        {
            soccerBall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX |
                                                               RigidbodyConstraints.FreezePositionZ;
            anim.SetTrigger("PlayAlone");
            TriggerCorrect();
        }
    }
}