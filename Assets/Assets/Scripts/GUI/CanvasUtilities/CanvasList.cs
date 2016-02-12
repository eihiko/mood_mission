using UnityEngine;

public class CanvasList : MonoBehaviour
{
    public GameObject[] Canvases;
    public GameObject[] AudioIgnoreList;
    public GameObject[] HelpCanvasIgnoreList;
    public GameObject[] DisableCanvasIgnoreList;

    private static int currentIndex = 0;

    public static int IncrementIndex()
    {
        return ++currentIndex;
    }

    public static int GetIndex()
    {
        return currentIndex;
    }

    public static void ResetIndex()
    {
        currentIndex = 0;
    }
}
