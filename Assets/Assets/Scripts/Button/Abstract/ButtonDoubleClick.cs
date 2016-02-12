using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ButtonDoubleClick : MonoBehaviour
{
    private int count = 0;
    private float timer = 0;
    private bool toggle = false;
    protected Color originalColor;
    protected Transform backgroundGlow;

    protected virtual void Awake()
    {
        originalColor = GetComponent<Image>().color;
        backgroundGlow = transform.parent.Find("BackgroundGlow");
    }

    public virtual void ButtonClicked()
    {
        ++count;
        if (count == 1)
        {
            GetComponent<Image>().color = Color.yellow;
            backgroundGlow.GetComponent<Image>().enabled = true;
        }
        if (count > 1)
        {
            count = 0;
            DoubleClickAction();
        }
        else
        {
            SingleClickAction();
        }
    }

    protected abstract void DoubleClickAction();
    protected abstract void SingleClickAction();

    //override Update to prevent button flashing
    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (count < 1)
        {
            if (timer > 0.5f)
            {
                ToggleBackgroundColor();
                timer = 0;
            }
        }
    }

    private void ToggleBackgroundColor()
    {
        if (toggle)
        {
            GetComponent<Image>().color = originalColor;
            backgroundGlow.GetComponent<Image>().enabled = false;
            toggle = false;
        }
        else
        {
            GetComponent<Image>().color = Color.yellow;
            backgroundGlow.GetComponent<Image>().enabled = true;
            toggle = true;
        }
    }

    protected void ResetCount()
    {
        count = 0;
    }
}
