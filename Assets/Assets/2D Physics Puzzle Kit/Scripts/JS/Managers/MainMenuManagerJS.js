#pragma strict

public class MainMenuManagerJS extends MonoBehaviour 
{
    enum MenuState { WaitingForInput, InTransit };
    private var menuState		: MenuState = MenuState.WaitingForInput;

    public var levelSelect		: GameObject;                      	//The level select parent object
    public var mask				: LayerMask = -1;					//Input layer mask
	public var useTouch			: boolean;
	
    private var hit				: RaycastHit2D;						//The raycast to detect the target item
    private var inputPos		: Vector3;

    //Called at every frame
    function Update()
    {
        if (useTouch)
            TouchControls();
        else
            MouseControls();
    }
    //Mouse controls
    private function MouseControls()
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
    private function TouchControls()
    {
        for (var touch : Touch in Input.touches)
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
    private function ScanForInput()
    {
        if (HasInput() && hit.collider != null)
        {
            if (hit.transform.name == "Play")
                StartCoroutine(PlayPressed(hit.transform));
            else if (hit.transform.name == "backButton")
            	StartCoroutine(BackPressed(hit.transform));
            else
                hit.transform.GetComponent(LevelSelectButtonJS).ClickEvent();
        }
    }
    //Called when the play button is pressed
    private function PlayPressed(button : Transform)
    {
        StartCoroutine(Animate(button, 0.1f, 0.2f));
        yield WaitForSeconds(0.3f);
        levelSelect.SetActive(true);
    }
	
	//Called when the back button is pressed
	private function BackPressed(button : Transform)
	{
		StartCoroutine(Animate(button, 0.1f, 0.2f));
        yield WaitForSeconds(0.3f);
        levelSelect.SetActive(false);
	}
	
    //Returns true if there is an active input
    private function HasInput()
    {
        if (useTouch)
            return Input.touchCount > 0;
        else
            return Input.GetMouseButtonDown(0);
    }

    //Animates a button
    private function Animate(button : Transform, scaleFactor : float, time : float)
    {
        var originalScale : Vector3 = button.localScale;

        var rate : float = 1.0f / time;
        var t : float = 0.0f;

        var d : float = 0;

        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            button.localScale = originalScale + (originalScale * (scaleFactor * Mathf.Sin(d * Mathf.Deg2Rad)));

            d = 180 * t;
            yield WaitForEndOfFrame();
        }

        button.localScale = originalScale;
    }
}
