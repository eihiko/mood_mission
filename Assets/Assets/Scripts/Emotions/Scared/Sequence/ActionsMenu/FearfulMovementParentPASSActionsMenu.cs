using System.Collections;
using System.Linq;
using Globals;
using UnityEngine;

namespace ScaredScene
{
    public class FearfulMovementParentPASSActionsMenu : FearfulMovementActionsMenu
    {
        public GameObject[] PASSLetters;

        protected override void Start()
        {
            base.Start();
            PASSLetters.ToList().ForEach(x => x.SetActive(true));
        }
    }
}