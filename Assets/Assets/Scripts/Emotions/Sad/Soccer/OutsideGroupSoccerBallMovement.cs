using UnityEngine;

namespace SadScene
{
    public class OutsideGroupSoccerBallMovement : MonoBehaviour
    {
        public bool isWatching = false;
        private Rigidbody rigidBody;
        private bool shouldStartDialogue = false;
        private Vector3 previousPosition;
        private Quaternion previousRotation;

        private void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        private void neutralizeForce()
        {
            rigidBody.velocity = Vector3.zero;   
        }

        public void ResetPosition()
        {
            transform.position = previousPosition;
            transform.rotation = previousRotation;
        }

        public void ApplyForce(Vector3 force)
        {
            rigidBody.AddForce(force);
        }

        public void KickBallUp()
        {
            neutralizeForce();
            rigidBody.AddForce(0f, 180f, 0f);
            rigidBody.AddTorque(0f, 0f, 100f);
        }

        public void SetPreviousPosition()
        {
            previousPosition = transform.position;
            previousRotation = transform.rotation;
        }

        public void KickBallForward(float multiplier)
        {
            neutralizeForce();
            SetPreviousPosition();
            rigidBody.constraints = RigidbodyConstraints.None;
            rigidBody.AddForce(250f*multiplier, 180f*multiplier, 0f);
            rigidBody.AddTorque(0f, 0f, -100f);
            rigidBody.angularDrag = 40f;
        }

        public void SetDialogueFlag(bool value)
        {
            shouldStartDialogue = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<OutsideGroupSoccerAnimation>() != null)
            {
                other.GetComponent<CapsuleCollider>().enabled = false;
                if (shouldStartDialogue)
                {
                    shouldStartDialogue = false;
                    other.GetComponent<OutsideGroupSoccerAnimation>().StartDialogue();
                }
                else if (isWatching)
                    other.GetComponent<SideLinesWatching>().StartWatchingFromSidelines();
                else
                    other.GetComponent<OutsideGroupSoccerAnimation>().KickForward(true);
            }
        }
    }
}