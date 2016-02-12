using System.Collections;
using Globals;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBase : MonoBehaviour
{
    protected AudioSource buttonPushAudio;
    protected AudioSource coinsGainingAudio;
    protected AudioSource coinsLosingAudio;
    protected AudioSource noTouchSymbolAudio;
    protected AudioSource helpAudio;
    protected AudioSource quitAudio;
    protected AudioSource repeatAudio;
    protected AudioSource questionAudio;

    protected GameObject practiceDropContainer;
    protected GameObject practiceButton;
    protected GameObject buttonPush;
    protected Coin coins;
    protected GameObject coinsArrow;
    protected GameObject noTouchArrow;
    protected GameObject initCanvas;
    protected GameObject helpCanvas;
    protected GameObject disablePanel;
    protected GameObject noInputSymbol;
    protected Color originalColor;

    protected bool initialAudioPlayed = false;

    protected virtual void Start()
    {
        StartCoroutine(DelayPlayAudio());

        InitializeGameObjects();
        InitializeAudio();
    }

    private IEnumerator DelayPlayAudio()
    {
        yield return new WaitForSeconds(1f);
        Utilities.PlayAudio(GetComponent<AudioSource>());
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        if (GameFlags.MainTutorialHasRun) HelpExplanationComplete();
        else
        {
            EnableHelpGUI();
            Utilities.PlayAudio(questionAudio);
            yield return new WaitForSeconds(questionAudio.clip.length);
            EnablePracticeUI();
        }
    }

    public void ExplainCoins()
    {
        coinsArrow.SetActive(true);
        StartCoroutine(playCoinExplanation());
    }

    private IEnumerator playCoinExplanation()
    {
        DisableHelpGUI();
        Utilities.PlayAudio(coinsGainingAudio);
        coins.ShowAddCoinAnimation();
        yield return new WaitForSeconds(coinsGainingAudio.clip.length);
        Utilities.PlayAudio(coinsLosingAudio);
        coins.ShowRemoveCoinAnimation();
        yield return new WaitForSeconds(coinsLosingAudio.clip.length);
        coinsArrow.SetActive(false);
        yield return StartCoroutine(playNoTouchSymbolExplanation());
    }

    private IEnumerator playNoTouchSymbolExplanation()
    {
        noTouchArrow.SetActive(true);
        Utilities.PlayAudio(noTouchSymbolAudio);
        yield return new WaitForSeconds(noTouchSymbolAudio.clip.length);
        noTouchArrow.SetActive(false);
        yield return StartCoroutine(ExplainHelpButtons());
    }

    private IEnumerator ExplainHelpButtons()
    {
        helpCanvas.GetComponent<Canvas>().enabled = true;

        ExplainButton(helpCanvas, "Help", ref helpAudio);
        yield return new WaitForSeconds(helpAudio.clip.length);

        ResetHighlight(helpCanvas.transform.Find("Help"));
        ExplainButton(helpCanvas, "Quit", ref quitAudio);
        yield return new WaitForSeconds(quitAudio.clip.length);

        ResetHighlight(helpCanvas.transform.Find("Quit"));
        ExplainButton(helpCanvas, "Repeat", ref repeatAudio);
        yield return new WaitForSeconds(repeatAudio.clip.length);
        ResetHighlight(helpCanvas.transform.Find("Repeat"));

        HelpExplanationComplete();
    }

    protected virtual void HelpExplanationComplete()
    {
        GameFlags.MainTutorialHasRun = true;
        Timeout.StopTimers();
        Timeout.SetRepeatAudio(null);
    }

    protected void ExplainButton(GameObject helpCanvas, string name, ref AudioSource audio)
    {
        var buttonParent = helpCanvas.transform.Find(name).gameObject;
        var button = buttonParent.transform.Find(buttonParent.name + "Button");
        audio = buttonParent.GetComponent<AudioSource>();
        Utilities.PlayAudio(audio);

        originalColor = button.GetComponent<Image>().color;
        button.GetComponent<Animator>().ResetTrigger("Normal");
        button.GetComponent<Animator>().SetTrigger("Grow");
        button.GetComponent<Image>().color = Color.yellow;
        buttonParent.transform.Find("BackgroundGlow").GetComponent<Image>().enabled = true;
    }

    public void EnableHelpGUI()
    {
        disablePanel.SetActive(false);
    }

    public void DisableHelpGUI()
    {
        disablePanel.SetActive(true);
    }

    public void HideNoInputSymbol()
    {
        noInputSymbol.SetActive(false);
    }

    public void ShowNoInputSymbol()
    {
        noInputSymbol.SetActive(true);
    }

    protected void EnablePracticeUI()
    {
        initialAudioPlayed = false;
        practiceDropContainer.SetActive(true);
        practiceButton.SetActive(true);
        buttonPush.SetActive(true);
        Utilities.PlayAudio(buttonPushAudio);
        Timeout.StartTimers();
        Timeout.SetRepeatAudio(buttonPushAudio);
    }

    protected void ResetHighlight(Transform button)
    {
        button.transform.Find(button.name + "Button").gameObject.GetComponent<Animator>().SetTrigger("Normal");
        button.Find(button.gameObject.name + "Button").GetComponent<Image>().color = originalColor;
        button.Find("BackgroundGlow").GetComponent<Image>().enabled = false;
    }

    public virtual void InitializeGameObjects()
    {
        practiceDropContainer = transform.Find("DropContainer").gameObject;
        practiceButton = transform.Find("PracticeButton").gameObject;
        buttonPush = transform.Find("ButtonPush").gameObject;
        coins = GameObject.Find("ScoreCanvas").transform.FindChild("CoinAnimation").GetComponent<Coin>();
        coinsArrow = transform.Find("CoinsArrow").gameObject;
        noTouchArrow = transform.Find("NoTouchSymbolArrow").gameObject;
        helpCanvas = GameObject.Find("HelpCanvas");
        disablePanel = helpCanvas.transform.FindChild("DisablePanel").gameObject;
        noInputSymbol = disablePanel.transform.FindChild("NoInputSymbol").gameObject;
    }

    protected virtual void InitializeAudio()
    {
        buttonPushAudio = buttonPush.GetComponent<AudioSource>();
        coinsGainingAudio = coinsArrow.transform.FindChild("Gaining").gameObject.GetComponent<AudioSource>();
        coinsLosingAudio = coinsArrow.transform.FindChild("Losing").gameObject.GetComponent<AudioSource>();
        questionAudio = transform.Find("TutorialQuestion").gameObject.GetComponent<AudioSource>();
        noTouchSymbolAudio = noTouchArrow.GetComponent<AudioSource>();
    }
}
