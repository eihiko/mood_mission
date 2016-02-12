using SadScene;
using UnityEngine;

public class SadTutorial : TutorialBase
{
    public GameObject luis;
    public GroupSoccerBallMovement groupSoccerBall;

    protected override void HelpExplanationComplete()
    {
        base.HelpExplanationComplete();
        GUIInitialization.Initialize();
        groupSoccerBall.RestartSoccerGame();
        luis.GetComponent<OutsideGroupSoccerAnimation>().KickForwardWithDelay();
    }
}