using UnityEngine;
using System.Collections;
using ScaredScene;

namespace ScaredScene
{
    public class JumpTrigger : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<CharacterMovement>().RunJump();
        }
    }
}