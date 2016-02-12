using System;
using System.Collections;
using UnityEngine;

public class CoinPile : MonoBehaviour
{
    public RectTransform backgroundGlow;
    private static Vector3 currentSize = Vector3.zero;
    private static Vector3 backgroundSize = Vector3.zero;

    private void Start()
    {
        if (currentSize != Vector3.zero)
        {
            transform.localScale = currentSize;
            backgroundGlow.localScale = backgroundSize;
        }
    }

    public void ShowFlashing()
    {
        StartCoroutine(showFlashing());
    }

    public void IncreaseScale(Vector3 value)
    {
        transform.localScale += value;
        backgroundGlow.localScale += value;
    }

    public void DecreaseScale(Vector3 value)
    {
        if (transform.localScale.x - value.x < 0 || backgroundGlow.localScale.x - value.x < 0) return;
        transform.localScale -= value;
        backgroundGlow.localScale -= value;
    }

    private IEnumerator showFlashing()
    {
        backgroundGlow.GetComponent<Animator>().SetTrigger("Flash");
        yield return new WaitForSeconds(1.5f);
        backgroundGlow.GetComponent<Animator>().SetTrigger("Off");
    }

    public void UpdateCurrentSize()
    {
        currentSize = transform.localScale;
        backgroundSize = backgroundGlow.localScale;
    }

    public static void ResetCoinPileSize()
    {
        currentSize = new Vector3(0.8f, 0.8f, 0.8f);
        backgroundSize = new Vector3(0.8f, 0.8f, 0.8f);
    }
}
