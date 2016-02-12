using UnityEngine;
using System.Collections;

namespace ScaredScene
{
    public class RunAway : ActionBase
    {
        public override void StartAction()
        {
            base.StartAction();
            SadTurn();
        }

        private void SadTurn()
        {
            transform.Find("CameraFollow").gameObject.SetActive(false);
            anim.SetTrigger("SadTurn");
            StartCoroutine(SadRunAway());
        }

        public IEnumerator SadRunAway()
        {
            yield return new WaitForSeconds(2.5f);
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            anim.SetBool("RunAway", true);
        }

        public void TriggerRunningMovement()
        {
            GetComponent<FearfulMovement>().RunReverse();
            StartCoroutine(ResetScene());
        }

        private IEnumerator ResetScene()
        {
            yield return new WaitForSeconds(1.5f);
            sceneReset.TriggerSceneReset(actionExplanation, true);
            gameObject.SetActive(false);
        }
    }
}