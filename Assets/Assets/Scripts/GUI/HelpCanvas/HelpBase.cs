using UnityEngine;
using System.Collections;

namespace HelpGUI
{
    public abstract class HelpBase : ButtonSelect {
        
        protected override void DoubleClickAction()
        {
            Utilities.StopAudio(instructions);
        }

        protected override void Update()
        {
            // disable flashing
        }
    }
}

