using UnityEngine;
using System.Collections;

namespace EggCatch
{
    public class EggScript : MonoBehaviour
    {
        private void Awake()
        {
            //rigidbody.AddForce(new Vector3(0, -100, 0), ForceMode.Force);
        }

        //Update is called by Unity every frame
        private void Update()
        {
            float fallSpeed = 1.5f*Time.deltaTime;
            transform.position -= new Vector3(0, fallSpeed, 0);
        }
    }
}