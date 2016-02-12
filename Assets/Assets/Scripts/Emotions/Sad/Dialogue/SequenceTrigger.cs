using UnityEngine;
using System.Collections;

namespace SadScene
{
    public class SequenceTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<OutsideGroupSoccerBallMovement>() != null)
            {
                other.GetComponent<OutsideGroupSoccerBallMovement>().SetDialogueFlag(true);
            }
        }
    }
}