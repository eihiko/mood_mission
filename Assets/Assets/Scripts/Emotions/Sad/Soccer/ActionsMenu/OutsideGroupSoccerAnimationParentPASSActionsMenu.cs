using System.Linq;
using Globals;
using UnityEngine;

namespace SadScene
{
    public class OutsideGroupSoccerAnimationParentPASSActionsMenu : OutsideGroupSoccerAnimationActionsMenu
    {
        public GameObject[] PASSLetters;

        protected override void Start()
        {
            base.Start();
            PASSLetters.ToList().ForEach(x => x.SetActive(true));
        }
    }
}