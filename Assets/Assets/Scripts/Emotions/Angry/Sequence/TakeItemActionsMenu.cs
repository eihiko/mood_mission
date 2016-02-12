using System.Collections;
using UnityEngine;

namespace AngryScene
{
    public class TakeItemActionsMenu : TakeItem
    {
        private void Start()
        {
            StartCoroutine(StartUsingIPad());
        }

        private IEnumerator StartUsingIPad()
        {
            yield return new WaitForSeconds(2f);
            anim.SetBool("IsUsingIPad", true);
            other.SetTrigger("IsAngry");
            GUIInitialization.Initialize();
            GUIHelper.NextGUI(null, GUIHelper.GetCurrentGUI());
        }
    }
}