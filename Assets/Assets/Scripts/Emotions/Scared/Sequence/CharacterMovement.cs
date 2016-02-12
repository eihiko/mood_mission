using System.Collections;
using UnityEngine;

namespace ScaredScene
{
    public class CharacterMovement : ControllerMovement
    {
        protected Animator anim;
        protected bool shouldRunBase = true;

        protected override void Start()
        {
            if (shouldRunBase) base.Start();
            anim = GetComponent<Animator>();
            multiplierSpeed = 1f;
        }

        public virtual void StepForward()
        {
            multiplierDirection = 1f;
            multiplierSpeed = 0f;
            StartWalking();
        }

        public void FreezeMovement()
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            anim.SetTrigger("Idle");
        }

        public virtual void StartSequence()
        {
            anim.SetBool("IsIdle", false);
            anim.SetTrigger("TurnAround");
        }

        public virtual void StartWalking()
        {
            anim.SetBool("Walking", true);
            isWalking = true;
        }

        public virtual void Run()
        {
            anim.SetBool("Walking", false);
            anim.SetTrigger("Run");
            multiplierSpeed = 3f;
        }

        public virtual void RunJump()
        {
            if (!anim.GetBool("RunJump"))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                anim.SetBool("Run", false);
                anim.SetBool("RunJump", true);
            }
        }

        public virtual void JumpToRun()
        {
            if (!anim.GetBool("Run"))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                anim.SetBool("RunJump", false);
                anim.SetBool("Run", true);
            }
        }

        public virtual void RunToWalk()
        {
            multiplierSpeed = 2.5f;
            anim.SetBool("Run", false);
            anim.SetBool("Walking", true);
        }

        public virtual void Walk()
        {
            multiplierSpeed = 1f;
        }

        public virtual void TurnAround()
        {
            isWalking = false;
            anim.SetBool("Walking", false);
            anim.SetTrigger("TurnAround");
        }

        public virtual void ShiftIdle()
        {
            anim.SetTrigger("Idle");
        }

        public virtual void EdgeSlip(string stumbleTrigger)
        {
            anim.SetBool("Run", false);
            anim.SetTrigger(stumbleTrigger);
            isWalking = false;
        }
    }
}

