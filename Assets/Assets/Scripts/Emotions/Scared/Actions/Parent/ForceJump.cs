using UnityEngine;
using System.Collections;

namespace ScaredScene
{
    public class ForceJump : IncorrectActionBase
    {
        protected override void DialogueAnimation()
        {
            anim.SetTrigger("Yell");
        }
    }
}