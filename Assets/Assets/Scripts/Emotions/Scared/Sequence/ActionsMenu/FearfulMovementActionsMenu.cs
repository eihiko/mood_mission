using System.Collections;
using System.Linq;
using Globals;
using UnityEngine;

namespace ScaredScene
{
    public class FearfulMovementActionsMenu : FearfulMovement
    {
        protected override void Start()
        {
            base.Start();
            global::GUIInitialization.Initialize();
            if (GameFlags.AdultIsPresent)
            {
                parentCharacters.ToList()
                    .First(x => x.name.ToLower().Contains(GameFlags.ParentGender.ToLower()))
                    .transform.parent.gameObject
                    .SetActive(true);
            }
            waitingForScarlet = false;
            StartCoroutine(ShowActionsMenu());
        }

        private IEnumerator ShowActionsMenu()
        {
            yield return new WaitForSeconds(2f);
            GUIHelper.NextGUI(null, GUIHelper.GetCurrentGUI());
        }
    }
}