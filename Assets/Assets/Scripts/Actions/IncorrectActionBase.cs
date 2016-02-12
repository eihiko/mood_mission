using UnityEngine;
using System.Collections;
using Globals;

public class IncorrectActionBase : ActionBase
{
    public AudioSource dialogue;

    public override void StartAction()
    {
        base.StartAction();
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        DialogueAnimation();
        Utilities.PlayAudio(dialogue);
        yield return new WaitForSeconds(dialogue.clip.length);
        AfterDialogue();
        TriggerIncorrect();
    }

    protected virtual void DialogueAnimation() {}

    protected virtual void AfterDialogue() {}
}