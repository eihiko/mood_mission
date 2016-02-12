using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.UI;
using CharacterMovement = ScaredScene.CharacterMovement;

public class ScaredTutorial : TutorialBase
{
    public GameObject scarlet;
    public GameObject aj;
    public GameObject[] parentCharacters;

    protected override void HelpExplanationComplete()
    {
        base.HelpExplanationComplete();
        GUIInitialization.Initialize();
        if (GameFlags.AdultIsPresent)
        {
            parentCharacters.ToList()
                .First(x => x.name.ToLower().Contains(GameFlags.ParentGender.ToLower()))
                .transform.parent.gameObject
                .SetActive(true);
        }
        scarlet.GetComponent<CharacterMovement>().StartSequence();
        aj.GetComponent<CharacterMovement>().StartSequence();
    }
}