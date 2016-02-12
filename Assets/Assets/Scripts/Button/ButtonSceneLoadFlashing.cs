using UnityEngine;
using System.Collections;

public class ButtonSceneLoadFlashing : ButtonSelect {

    public string sceneToLoad;

    protected override void DoubleClickAction()
    {
        Utilities.LoadScene(sceneToLoad);
    }
}
