using UnityEngine;
using System.Collections;
using Globals;
using UnityEngine.UI;

public class PracticeButton : ButtonDragDrop {
    private AudioSource buttonDragAudio;
    private AudioSource buttonPushAudio;
    private AudioSource practiceButtonAudio;

    private bool shouldPlayDragAudio = false;
    private bool answerSubmitted = false;
    private bool buttonPushed = false;

    protected override void Awake()
    {
        base.Awake();

        buttonPushAudio = transform.parent.Find("ButtonPush").gameObject.GetComponent<AudioSource>();
        buttonDragAudio = transform.parent.Find("ButtonDrag").gameObject.GetComponent<AudioSource>();
        practiceButtonAudio = transform.parent.Find("PracticeButton").gameObject.GetComponent<AudioSource>();
    }

    public override void ButtonDown()
    {
        base.ButtonDown();
        if (!buttonPushed) ShowDragging();
    }

    public override void ButtonRelease()
    {
        base.ButtonRelease();
        //ShowPushing();
    }

    public override void SubmitAnswer()
    {
        answerSubmitted = true;
        Timeout.StopTimers();
        transform.parent.GetComponent<TutorialBase>().ExplainCoins();
        HidePracticeUI();
    }

    private void Update()
    {
        if (shouldPlayDragAudio && !practiceButtonAudio.isPlaying)
        {
            Utilities.PlayAudio(buttonDragAudio);
            Timeout.SetRepeatAudio(buttonDragAudio);
            shouldPlayDragAudio = false;
        }
    }

    private void ShowDragging()
    {
        Utilities.StopAudio(buttonPushAudio);
        buttonPushed = true;
        StartCoroutine(DelayShowDragging());
    }

    private IEnumerator DelayShowDragging()
    {
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        transform.parent.Find("ButtonPush").gameObject.SetActive(false);
        transform.parent.Find("ButtonDrag").gameObject.SetActive(true);
        transform.parent.Find("PracticeButtonDrag").gameObject.SetActive(true);
        shouldPlayDragAudio = true;
    }

    private void HidePracticeUI()
    {
        transform.parent.Find("ButtonPush").gameObject.SetActive(false);
        transform.parent.Find("ButtonDrag").gameObject.SetActive(false);
        transform.parent.Find("DropContainer").gameObject.SetActive(false);
        transform.parent.Find("PracticeButton").gameObject.SetActive(false);
        transform.parent.Find("PracticeButtonDrag").gameObject.SetActive(false);
    }

    private void ShowPushing()
    {
        if (!answerSubmitted)
        {
            Utilities.StopAudio(buttonDragAudio);
            transform.parent.Find("ButtonPush").gameObject.SetActive(true);
            transform.parent.Find("ButtonDrag").gameObject.SetActive(false);
            Utilities.PlayAudio(buttonPushAudio);
        }
    }
}
