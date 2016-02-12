using System.Collections;
using UnityEngine;

namespace SadScene
{
    public class OutsideGroupSoccerAnimationActionsMenu : OutsideGroupSoccerAnimation
    {
        protected override void Start()
        {
            base.Start();
            global::GUIInitialization.Initialize();
            GetComponent<CharacterMovement>().EnableParent();
            GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(ShowActionsMenu());
        }

        protected IEnumerator ShowActionsMenu()
        {
            yield return new WaitForSeconds(2f);
            GUIHelper.NextGUI(null, GUIHelper.GetCurrentGUI());
        }
    }
}