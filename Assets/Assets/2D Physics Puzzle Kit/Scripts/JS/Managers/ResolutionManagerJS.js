#pragma strict

public class ResolutionManagerJS extends MonoBehaviour 
{
    public var toReposition				: Transform[];
    public var toolbox 					: Transform;

    private var scaleFactor				: float;				//The current scale factor

    // Use this for initialization
    function Start()
    {
        scaleFactor = Camera.main.aspect / 1.33f;

        for (var item : Transform in toReposition)
        	item.position = new Vector3(item.position.x * scaleFactor, item.position.y, item.position.z);
         	
		toolbox.position = new Vector3((scaleFactor - 1) * 4, toolbox.position.y, toolbox.position.z);
    }
}
