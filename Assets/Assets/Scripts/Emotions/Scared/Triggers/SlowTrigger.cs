using UnityEngine;
using System.Collections;
using ScaredScene;

namespace ScaredScene
{
    public class SlowTrigger : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<CharacterMovement>().Walk();
        }
    }
}