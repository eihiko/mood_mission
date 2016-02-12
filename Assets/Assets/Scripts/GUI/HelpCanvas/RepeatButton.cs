using Globals;

namespace HelpGUI
{
    public class RepeatButton : HelpBase
    {
         protected override void DoubleClickAction()
        {
            base.DoubleClickAction();
            Utilities.PlayAudio(Timeout.GetRepeatAudio());
        }
    }
}