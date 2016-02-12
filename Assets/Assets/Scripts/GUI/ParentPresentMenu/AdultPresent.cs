using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Globals;

namespace ParentPresentMenu
{
    public class AdultPresent : MonoBehaviour
    {
        public IEnumerator DelayShowCanvas()
        {
            yield return new WaitForSeconds(1f);
            GUIHelper.NextGUI(null, GUIHelper.GetCurrentGUI());
        }

        public void Yes()
        {
            Timeout.StopTimers();
            GameFlags.AdultIsPresent = true;
            GUIHelper.NextGUI();
        }

        public void No()
        {
            GameFlags.AdultIsPresent = false;
            GameFlags.ParentGender = new List<string> {"Dad", "Mom"}[Random.Range(0, 2)];
            Timeout.StopTimers();
            Utilities.LoadEmotionScene(Scenes.GetNextSceneToLoadForParentPresent());
        }
    }
}