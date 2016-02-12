using ScaredScene;
using UnityEngine;

namespace ScaredScene
{
    public class EdgeSlipTrigger : MonoBehaviour
    {
        public bool AlwaysStumble = false;
        public bool shouldJump = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!shouldJump && other.gameObject.GetComponent<FearfulMovement>() != null)
            {
                other.gameObject.GetComponent<CharacterMovement>().EdgeSlip("Stumble");
                shouldJump = true;
            }
            else if (other.gameObject.GetComponent<FearfulMovement>() != null)
            {
                other.gameObject.GetComponent<FearfulMovement>().RunJumpWithClapping();
            }
            else
            {
                other.gameObject.GetComponent<CharacterMovement>().RunJump();
            }
        }
    }
}