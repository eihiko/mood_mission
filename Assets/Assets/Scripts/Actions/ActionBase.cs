using UnityEngine;
using System.Collections;
using Globals;

public class ActionBase : MonoBehaviour
{
    public Animator anim;
    public SceneReset sceneReset;
    public AudioSource actionExplanation;

    protected void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    public virtual void StartAction()
    {
        Timeout.StopTimers();
    }

    protected void TriggerCorrect()
    {
        sceneReset.TriggerCorrect(actionExplanation, Scenes.GetNextMiniGame(), true);
    }

    protected void TriggerIncorrect()
    {
        sceneReset.TriggerSceneReset(actionExplanation, true);
    }

    protected void ShowCorrect(bool show)
    {
        sceneReset.ShowCorrectSymbol(show);
    }

    protected void ShowIncorrect(bool show)
    {
        sceneReset.ShowIncorrectSymbol(show);
    }
}