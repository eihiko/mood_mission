using UnityEngine;
using System.Collections;

namespace SadScene
{
    public class InvalidateEmotion : IncorrectActionBase
    {
        protected override void DialogueAnimation()
        {
            anim.SetTrigger("Invalidate");
        }

        protected override void AfterDialogue()
        {
            anim.SetTrigger("Idle");
        }
    }
}