using UnityEngine;
using System.Collections;

public class GUIAction : ButtonDragDrop
{
    public ActionBase action;

    public override void SubmitAnswer()
    {
        base.SubmitAnswer();
        action.StartAction();
        HideGUI();
    }
}
