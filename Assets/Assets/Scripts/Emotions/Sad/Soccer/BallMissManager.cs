using UnityEngine;
using System.Collections;

namespace SadScene
{
    public class BallMissManager : ObjectSequenceManager
    {
        public OutsideGroupSoccerBallMovement soccerBall;

        public void SetBallPreviousPosition()
        {
            soccerBall.SetPreviousPosition();
        }

        public override void NextInSequence()
        {
            if (currentIndex != 0) SequenceObjects[currentIndex - 1].SetActive(false);
            base.NextInSequence();
        }
    }
}