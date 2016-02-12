using UnityEngine;
using System.Collections;
using SadScene;

public class RetryPoint : MonoBehaviour
{
    public static GameObject PreviousRetryPoint;
    public OutsideGroupSoccerBallMovement soccerBall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OutsideGroupSoccerAnimation>() != null)
        {
            other.GetComponent<OutsideGroupSoccerAnimation>().StopWalkingBackwards();
            gameObject.SetActive(false);
            soccerBall.ResetPosition();
            LaneAppear.shouldShowLanes = true;
            PreviousRetryPoint = gameObject;
        }
    }
}
