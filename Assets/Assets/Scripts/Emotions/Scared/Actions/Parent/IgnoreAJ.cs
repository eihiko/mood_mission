using UnityEngine;
using System.Collections;

namespace ScaredScene
{
    public class IgnoreAJ : IncorrectActionBase
    {
        protected override void DialogueAnimation()
        {
            base.DialogueAnimation();
//            anim.SetTrigger("Ignore");
        }
    }
}