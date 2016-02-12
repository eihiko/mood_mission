using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour
{
    public float startDelay = 3;
    public float totalTime = 30;
    public Transform lastItem;

    private bool shouldScroll;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(delayScroll());
    }

    private IEnumerator delayScroll()
    {
        yield return new WaitForSeconds(startDelay);
        shouldScroll = true;
    }

    private void Update()
    {
        if (shouldScroll) transform.position += new Vector3(0f, 1f, 0f);
        if (lastItem.position.y > transform.GetComponent<RectTransform>().rect.height + lastItem.GetComponent<RectTransform>().rect.height)
            transform.position = originalPosition - new Vector3(0f, transform.parent.GetComponent<RectTransform>().rect.height, 0f);
    }
}
