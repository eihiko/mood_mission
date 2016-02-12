using UnityEngine;

namespace HelpGUI
{
    public class HelpButton : HelpBase
    {
        protected override void DoubleClickAction()
        {
            base.DoubleClickAction();
            //GUIHelper.GetCurrentGUI().GetComponent<HintBase>().ShowHint();
            Debug.Log("Help clicked.");
        }
    }
}