using UnityEngine;
using System.Collections;
using ScaredScene;

namespace ScaredScene
{
    public class FearlessMovement : CharacterMovement
    {
        private GameObject otherCharacter;

        protected override void Start()
        {
            shouldRunBase = false;
            base.Start();
            otherCharacter = GameObject.Find("Aj");
        }

        public void StartClapping()
        {
            anim.SetTrigger("Clap");
        }

        public override void StartWalking()
        {
            base.StartWalking();
            otherCharacter.GetComponent<CharacterMovement>().StartWalking();
        }

        public override void TurnAround()
        {
            base.TurnAround();
            otherCharacter.GetComponent<CharacterMovement>().Run();
        }

        public override void Run()
        {
            base.Run();
            otherCharacter.GetComponent<CharacterMovement>().StartWalking();
        }
    }
}