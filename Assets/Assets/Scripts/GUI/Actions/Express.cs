using UnityEngine;
using System.Collections;

public class Express : ButtonDragDrop
{
    public ActionBase expressAction;

    public override void ButtonDown()
    {
        base.ButtonDown();
        Debug.Log("Express clicked");
    }

    public override void SubmitAnswer()
    {
        base.SubmitAnswer();
        Debug.Log("Express submitted");
        expressAction.StartAction();
        HideGUI();
    }
}
