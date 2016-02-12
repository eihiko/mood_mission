using UnityEngine;
using UnityEngine.UI;

public class Orientation : MonoBehaviour
{
    public Text orientationText;

    private void Update()
    {
        orientationText.text = Input.deviceOrientation.ToString();
    }
}
