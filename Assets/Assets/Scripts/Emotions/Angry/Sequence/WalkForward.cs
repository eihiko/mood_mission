using UnityEngine;
using System.Collections;

namespace AngryScene
{
    public class WalkForward : MonoBehaviour
    {
        public Animator anim;
        public TutorialBase tutorial;
        private bool shouldStartWalking = false;
        private bool stepForward = false;

        public void StartWalking()
        {
            tutorial.DisableHelpGUI();
            shouldStartWalking = true;
            Utilities.PlayAudio(GetComponent<AudioSource>());
        }

        private void Update()
        {
            if (shouldStartWalking)
            {
                ChangePosition(1.0f);
                if (transform.position.x <= 204.639f)
                {
                    StopWalking();
                    transform.position = new Vector3(204.639f, transform.position.y, transform.position.z);
                }
            }
            if (stepForward)
            {
                ChangePosition(1.0f);
                if (transform.position.x <= 204.43f)
                {
                    TakeItem();
                    transform.position = new Vector3(204.43f, transform.position.y, transform.position.z);
                }
            }
        }

        public void StepForward()
        {
            stepForward = true;
            anim.SetBool("IsWalking", true);
        }

        private void ChangePosition(float speed)
        {
            float move = Time.deltaTime * speed;
            transform.position = new Vector3(transform.position.x - move, transform.position.y, transform.position.z);
        }

        private void TakeItem()
        {
            stepForward = false;
            anim.SetBool("IsWalking", false);
            tutorial.ShowNoInputSymbol();
            gameObject.GetComponent<TakeItem>().TakeIPad();
        }

        private void StopWalking()
        {
            shouldStartWalking = false;
            anim.SetBool("IsWalking", false);
            anim.SetTrigger("IsIdle");
            StartCoroutine(GetComponent<TakeItem>().StartTalking());
        }
    }
}