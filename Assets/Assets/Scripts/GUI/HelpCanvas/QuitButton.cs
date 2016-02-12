using UnityEngine;
using System.Collections;

namespace HelpGUI
{
    public class QuitButton : HelpBase
    {

        protected override void DoubleClickAction()
        {
            base.DoubleClickAction();
            Utilities.LoadScene("MainMenuScreen");
        }
    }
}