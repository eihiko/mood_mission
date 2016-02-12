using UnityEngine;
using System.Collections;

namespace ScaredScene
{
    public class TemperTantrum : IncorrectActionBase
    {
        protected override void DialogueAnimation()
        {
            base.DialogueAnimation();
            transform.Find("CameraFollow").gameObject.SetActive(false);
            anim.SetTrigger("Tantrum");
        }

        public void ScaredIdleEvent()
        {
            anim.SetTrigger("Idle");
        }
    }
}