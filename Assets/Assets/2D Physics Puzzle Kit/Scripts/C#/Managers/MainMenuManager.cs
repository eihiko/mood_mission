using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    enum MenuState { WaitingForInput, InTransit };
    MenuState menuState = MenuState.WaitingForInput;

    public GameObject levelSelect;                      //The level select parent object
    public LayerMask mask = -1;                         //Input layer mask
    public bool useTouch;                               //Use touch controls

    RaycastHit2D hit;                                   //The raycast to detect the target item
    Vector3 inputPos;

    //Called at every frame
    void Update()
    {
        if (useTouch)
            TouchControls();
        else
            MouseControls();
    }
    //Mouse controls
    void MouseControls()
    {
        //Get the position of the input
        inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inputPos.z = 0;

        //Cast a ray to detect objets
        hit = Physics2D.Raycast(inputPos, new Vector2(0, 0), 0.1f, mask);

        if (menuState == MenuState.WaitingForInput)
            ScanForInput();
    }
    //Touch controls
    void TouchControls()
    {
        foreach (Touch touch in Input.touches)
        {
            //Get the position of the input
            inputPos = Camera.main.ScreenToWorldPoint(touch.position);
            inputPos.z = 0;

            //Cast a ray to detect objets
            hit = Physics2D.Raycast(inputPos, new Vector2(0, 0), 0.1f, mask);

            if (menuState == MenuState.WaitingForInput)
                ScanForInput();
        }
    }

    //Scans for inputs
    void ScanForInput()
    {
        if (HasInput() && hit.collider != null)
        {
            if (hit.transform.name == "Play")
                StartCoroutine(PlayPressed(hit.transform));
			else if (hit.transform.name == "backButton")
				StartCoroutine(BackPressed(hit.transform));
            else
                hit.transform.GetComponent<LevelSelectButton>().ClickEvent();
        }
    }
    //Called when the play button is pressed
    IEnumerator PlayPressed(Transform button)
    {
        menuState = MenuState.InTransit;

        StartCoroutine(Animate(button, 0.1f, 0.2f));
        yield return new WaitForSeconds(0.3f);
        levelSelect.SetActive(true);

        menuState = MenuState.WaitingForInput;
    }

	//Called when the back button is pressed
	IEnumerator BackPressed(Transform button)
	{
        menuState = MenuState.InTransit;

		StartCoroutine(Animate(button, 0.1f, 0.2f));
		yield return new WaitForSeconds(0.3f);
		levelSelect.SetActive(false);

        menuState = MenuState.WaitingForInput;
	}

    //Returns true if there is an active input
    bool HasInput()
    {
        if (useTouch)
            return Input.touchCount > 0;
        else
            return Input.GetMouseButtonDown(0);
    }

    //Animates a button
    IEnumerator Animate(Transform button, float scaleFactor, float time)
    {
        Vector3 originalScale = button.localScale;

        float rate = 1.0f / time;
        float t = 0.0f;

        float d = 0;

        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            button.localScale = originalScale + (originalScale * (scaleFactor * Mathf.Sin(d * Mathf.Deg2Rad)));

            d = 180 * t;
            yield return new WaitForEndOfFrame();
        }

        button.localScale = originalScale;
    }
}
