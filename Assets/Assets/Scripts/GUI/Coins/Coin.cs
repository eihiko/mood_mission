using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public CoinPile coinPile;
    public AudioSource correctSound;
    public AudioSource wrongSound;

    public void AddCoins()
    {
        coinPile.IncreaseScale(new Vector3(0.2f, 0.2f));
        HideCoin();
        resetState();
    }

    public void RemoveCoins()
    {
        coinPile.DecreaseScale(new Vector3(0.2f, 0.2f));
        HideCoin();
        resetState();
    }

    public void ShowCoin()
    {
        GetComponent<RawImage>().enabled = true;
    }

    public void HideCoin()
    {
        GetComponent<RawImage>().enabled = false;
    }

    public void ShowAddCoinAnimation()
    {
        showAnimation("Add");
        Utilities.PlayAudioUnBlockable(correctSound);
    }

    public void ShowRemoveCoinAnimation()
    {
        showAnimation("Remove");
        Utilities.PlayAudioUnBlockable(wrongSound);
    }

    private void showAnimation(string trigger)
    {
        coinPile.ShowFlashing();
        GetComponent<Animator>().gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger(trigger);
    }

    private void resetState()
    {
        gameObject.SetActive(false);
        GetComponent<Animator>().SetTrigger("Normal");
        coinPile.UpdateCurrentSize();
    }
}
