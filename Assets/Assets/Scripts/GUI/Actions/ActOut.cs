using UnityEngine;
using System.Collections;

public class ActOut : ButtonDragDrop
{
    public ActionBase actOutAction;

    public override void ButtonDown()
    {
        base.ButtonDown();
        Debug.Log("ActOut clicked");
    }

    public override void SubmitAnswer()
    {
        base.SubmitAnswer();
        Debug.Log("ActOut submitted");
        actOutAction.StartAction();
        HideGUI();
    }
}
