using UnityEngine;
using System.Collections;

public class DropDownHandler : MonoBehaviour
{
    public void HideDropDownList()
    {
        var dropdown = transform.Find("Dropdown List");
        if (dropdown != null)
        {
            var canvasGroup = dropdown.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void EnableDropDownList()
    {
        var dropdown = transform.Find("Dropdown List");
        if (dropdown != null)
        {
            var canvasGroup = transform.Find("Dropdown List").GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
