using System;
using UnityEngine;
using System.Collections;
using Globals;

public class SceneReset : MonoBehaviour {
    public string sceneToLoadIncorrect;
    public Canvas noSymbol;
    public Canvas correctSymbol;
    bool startedPlaying = false;
    private Animator noSymbolAnimator;
    private Animator correctSymbolAnimator;
    private Coin coin;

    void Awake() {
        if (noSymbol != null) noSymbolAnimator = noSymbol.GetComponent<Animator>();
        if (correctSymbol != null) correctSymbolAnimator = correctSymbol.GetComponent<Animator>();
        if (GameObject.Find("ScoreCanvas") != null)
        {
            var coinTransform = GameObject.Find("ScoreCanvas").transform.FindChild("CoinAnimation");
            if (coinTransform != null)
                coin = coinTransform.GetComponent<Coin>();
        }
    }

    public void TriggerSceneReset(AudioSource audioSource, bool showSymbol)
    {
        StartCoroutine(DelayLoadingScene(audioSource, sceneToLoadIncorrect, () => { ShowIncorrectSymbol(showSymbol);}));
    }

    public void TriggerCorrect(AudioSource audioSource, string sceneToLoadCorrect, bool showSymbol)
    {
        StartCoroutine(DelayLoadingScene(audioSource, sceneToLoadCorrect, () => { ShowCorrectSymbol(showSymbol); }));
    }

    public void ShowCorrectSymbol(bool show)
    {
        if (coin != null && show) coin.ShowAddCoinAnimation();
        showSymbol(correctSymbol, correctSymbolAnimator, show);
    }

    public void ShowIncorrectSymbol(bool show)
    {
        if (coin != null && show) coin.ShowRemoveCoinAnimation();
        showSymbol(noSymbol, noSymbolAnimator, show);
    }

    private void showSymbol(Canvas symbol, Animator animator, bool show)
    {
        symbol.enabled = show;
        animator.SetTrigger(show ? "ShowCanvas" : "ResetCanvas");
    }

    private IEnumerator DelayLoadingScene(AudioSource audioSource, string sceneToLoad, Action displaySymbol)
    {
        Utilities.PlayAudio(audioSource);
        displaySymbol();
        if (audioSource != null) yield return new WaitForSeconds(audioSource.clip.length);
        Timeout.StopTimers();
        Utilities.LoadScene(sceneToLoad);
    }
}
