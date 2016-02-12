using SadScene;
using UnityEngine;

namespace SadScene
{
    public class BallMiss : MonoBehaviour
    {
        private BallMissManager ballMissManager;

        private void Start()
        {
            ballMissManager = transform.parent.GetComponent<BallMissManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<OutsideGroupSoccerAnimation>() != null)
            {
                LaneAppear.shouldShowLanes = false;
                LaneAppear.HideAllLanes();
                ballMissManager.SetBallPreviousPosition();
                var ballMissAudio = transform.parent.GetComponent<AudioSource>();
                other.GetComponent<OutsideGroupSoccerAnimation>().StopMoving();
                other.GetComponent<OutsideGroupSoccerAnimation>().ResetCamera(true, false);
                other.GetComponent<OutsideGroupSoccerAnimation>().ResetPosition(ballMissAudio);
            }
        }
    }
}