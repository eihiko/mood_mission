using UnityEngine;
using System.Collections;

public class Hide : ButtonDragDrop
{
    public ActionBase hideAction;

    public override void ButtonDown()
    {
        base.ButtonDown();
        Debug.Log("Hide clicked");
    }

    public override void SubmitAnswer()
    {
        base.SubmitAnswer();
        Debug.Log("Hide submitted");
        hideAction.StartAction();
        HideGUI();
    }
}
