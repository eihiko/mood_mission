using UnityEngine;
using System.Collections;
using ScaredScene;

namespace ScaredScene
{
    public class RunTrigger : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<CharacterMovement>().Run();
        }
    }
}