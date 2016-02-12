using UnityEngine;
using System.Collections;

namespace AngryScene
{
    public class RunAway : ActionBase
    {
        private bool run = false;
        private float rotation;
        private bool rotationHasCycledBack = false;

        protected void Update() {
            if (run)
            {
                float move = Time.deltaTime * 1.5f;
                transform.position = new Vector3(transform.position.x - move, transform.position.y, transform.position.z);
            }
            if (rotationHasCycledBack && transform.rotation.eulerAngles.y <= 265f && !run)
            {
                anim.SetBool("IsTurning", false);
                anim.SetTrigger("IsHiding");
                run = true;
                StartCoroutine(triggerReset());
            }
            if (transform.rotation.eulerAngles.y >= 350f)
            {
                rotationHasCycledBack = true;
            }
        }

        public void StartRunningAway()
        {
            anim.SetTrigger("IsHiding");
        }

        private IEnumerator triggerReset()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            yield return new WaitForSeconds(3.5f);
            sceneReset.TriggerSceneReset(actionExplanation, true);
        }

        public override void StartAction()
        {
            base.StartAction();
            StartRunningAway();
        }
    }
}