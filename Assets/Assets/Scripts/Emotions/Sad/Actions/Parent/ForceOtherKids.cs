using UnityEngine;
using System.Collections;

namespace SadScene
{
    public class ForceOtherKids : IncorrectActionBase
    {
        protected override void DialogueAnimation()
        {
            anim.SetTrigger("Yell");
        }

        protected override void AfterDialogue()
        {
            anim.SetTrigger("Idle");
        }
    }
}