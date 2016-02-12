using UnityEngine;
using System.Collections;
using System.Linq;

namespace SadScene
{
    public class SideLinesWatching : ActionBase
    {
        public GameObject[] parents;
        public GroupSoccerBallMovement groupSoccerBall;

        public override void StartAction()
        {
            base.StartAction();
            TurnAround();
        }

        private void TurnAround()
        {
            anim.SetTrigger("Watch");
            groupSoccerBall.RestartSoccerGame();
            transform.FindChild("CameraFollow").GetComponent<CameraFollow>().enabled = true;
        }

        public void WalkTowardOthersEvent()
        {
            parents.ToList().ForEach(x => x.GetComponent<BoxCollider>().enabled = false);
            anim.SetTrigger("Watch");
            GetComponent<CapsuleCollider>().enabled = true;
            GetComponent<OutsideGroupSoccerAnimation>().SetWalkAwaySpeed(true, 0.5f, 0f);
        }
        
        public void StartWatchingFromSidelines()
        {
            GetComponent<CharacterMovement>().StopWalking(false);
            transform.FindChild("CameraFollow").GetComponent<CameraFollow>().enabled = false;
            TriggerIncorrect();
        }
    }
}