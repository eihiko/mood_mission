using System.Collections;
using AngryScene;
using UnityEngine;
using UnityEngine.UI;

public class AngryTutorial : TutorialBase
{
    private AudioSource helpLilyPlayAudio;
    private AudioSource whatLilyIsPlayingAudio;

    public GameObject otherCharacter;
    private GameObject fingerDrag;
    public GameObject ipadCamera;
    public GameObject miniGame;
    public GameObject ipadCanvas;
    private Transform ipadBucketTracker;
    public Transform bucket;
    private Transform maxX;
    private Transform minX;

    protected override void HelpExplanationComplete()
    {
        base.HelpExplanationComplete();
        GUIInitialization.Initialize();
        StartCoroutine(HelpLilyPlayAudio());
    }

    private IEnumerator HelpLilyPlayAudio()
    {
        HideNoInputSymbol();
        ipadCamera.SetActive(true);
        ipadCanvas.SetActive(true);
        miniGame.SetActive(true);
        Utilities.PlayAudio(whatLilyIsPlayingAudio);
        yield return new WaitForSeconds(whatLilyIsPlayingAudio.clip.length);

        Utilities.PlayAudio(helpLilyPlayAudio);
        fingerDrag.SetActive(true);
        yield return new WaitForSeconds(helpLilyPlayAudio.clip.length);
        fingerDrag.SetActive(false);
        otherCharacter.SetActive(true);
        otherCharacter.GetComponent<WalkForward>().StartWalking();
    }

    public override void InitializeGameObjects()
    {
        base.InitializeGameObjects();
        fingerDrag = transform.Find("FingerDrag").gameObject;
        ipadBucketTracker = ipadCanvas.transform.FindChild("Circle");
        maxX = ipadCanvas.transform.FindChild("Max");
        minX = ipadCanvas.transform.FindChild("Min");
    }

    protected override void InitializeAudio()
    {
        base.InitializeAudio();
        helpLilyPlayAudio = transform.Find("HelpLilyPlay").gameObject.GetComponent<AudioSource>();
        whatLilyIsPlayingAudio = transform.Find("WhatLilyIsPlaying").gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        var percentMoved = (bucket.position.x + 2.5f)/5.0f;
        var max = maxX.localPosition.x - (ipadBucketTracker.GetComponent<RectTransform>().rect.width / 2.0f);
        var min = minX.localPosition.x + (ipadBucketTracker.GetComponent<RectTransform>().rect.width / 2.0f);
        ipadBucketTracker.localPosition = new Vector2(min + ((max - min) * percentMoved), ipadBucketTracker.localPosition.y);
    }
}