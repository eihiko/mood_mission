using UnityEngine;
using System.Collections;

namespace AngryScene
{
    public class AnimationActions : MonoBehaviour
    {
        private Animator anim;
        
        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void TriggerStandToSit()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            anim.SetTrigger("IsSharing");
        }

        public void TriggerSitIdle()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            anim.SetTrigger("IsSharing");
        }

        public void TriggerAngryFoldedArmsEvent()
        {
            anim.SetTrigger("IsAngry");
        }

        public void MoveIpadUnderArm()
        {
            var ipad = GameObject.Find("iPad");
            ipad.transform.localPosition = new Vector3(0.04f, -0.073f, 0.049f);
            ipad.transform.localRotation = Quaternion.Euler(352.4399f, 357.2535f, 10.56083f);
        }

        public void MoveIpadToLap()
        {
            var ipad = GameObject.Find("iPad");
            ipad.transform.localPosition = new Vector3(0.224f, 0.145f, 0.043f);
            ipad.transform.localRotation = Quaternion.Euler(15.41914f, 158.8406f, 1.443392f);
        }
    }
}

