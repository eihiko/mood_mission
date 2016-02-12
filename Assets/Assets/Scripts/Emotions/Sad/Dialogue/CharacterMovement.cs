using UnityEngine;
using System.Collections;
using System.Linq;
using Globals;

namespace SadScene
{
    public class CharacterMovement : MonoBehaviour
    {
        public GameObject[] parentCharacters;

        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void WalkAwayEvent()
        {
            anim.SetTrigger("WalkAway");
            GetComponent<OutsideGroupSoccerAnimation>().SetWalkAwaySpeed(true, -0.5f, 0f);
            EnableParent();
            StartCoroutine(enableCollider());
        }

        private IEnumerator enableCollider()
        {
            yield return new WaitForSeconds(1f);
            GetComponent<CapsuleCollider>().enabled = true;
        }

        public void StopWalking(bool soccerFlag)
        {
            anim.SetTrigger("Idle");
            GetComponent<OutsideGroupSoccerAnimation>().SetWalkAwaySpeed(false, 0f, 0f);
            GetComponent<OutsideGroupSoccerAnimation>().SetSoccerBallFlag(soccerFlag);
        }

        public void EnableParent()
        {
            parentCharacters.ToList().First(x => x.name.ToLower().Contains(GameFlags.ParentGender.ToLower()))
                .transform.parent.gameObject.SetActive(true);
        }
    }
}

