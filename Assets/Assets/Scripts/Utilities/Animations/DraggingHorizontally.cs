using UnityEngine;

public class DraggingHorizontally : MonoBehaviour
{
    public Transform target;
    public Transform following;

    public float speed;
    // this is speedSmallScreen / (screenWidth / speedSmallScreen)
    // test on small screen first, determine desired speed, then 
    // compute constant using above
    public float constant;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + Time.deltaTime * (Screen.width / speed) * constant,
                transform.localPosition.y);
        if (transform.localPosition.x >= target.localPosition.x / 2.0f)
        {
            transform.localPosition = initialPosition;
        }

        if (following != null) following.localPosition = new Vector2(transform.localPosition.x, following.localPosition.y);
    }
}
