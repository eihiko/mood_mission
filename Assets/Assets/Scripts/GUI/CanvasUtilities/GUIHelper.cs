using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Globals;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIHelper : MonoBehaviour
{
    public static GameObject Canvases;

    public static GameObject GetCurrentGUI()
    {
        return GetAllGUI()[CanvasList.GetIndex()];
    }

    public static GameObject GetNextGUI()
    {
        return GetAllGUI()[CanvasList.GetIndex() + 1];
    }

    public static GameObject[] GetAllGUI()
    {
        return Canvases.GetComponent<CanvasList>().Canvases;
    }

    public static GameObject[] GetAudioIgnoreList()
    {
        return Canvases.GetComponent<CanvasList>().AudioIgnoreList;
    }

    public static GameObject[] GetHelpCanvasIgnoreList()
    {
        return Canvases.GetComponent<CanvasList>().HelpCanvasIgnoreList;
    }

    public static GameObject[] GetDisableCanvasIgnoreList()
    {
        return Canvases.GetComponent<CanvasList>().DisableCanvasIgnoreList;
    }

    public static GameObject GetGUIByName(string name)
    {
        return GetAllGUI().ToList().First(x => x.name.Equals(name));
    }

    public static void NextGUI()
    {
        NextGUI(GetCurrentGUI(), GetNextGUI());
    }

    public static void NextGUI(GameObject current, GameObject next)
    {
        Timeout.StopTimers();
        showHelpUI(next);
        next.SetActive(true);
        if (current != null)
        {
            if (!GetDisableCanvasIgnoreList().Contains(current)) current.SetActive(false);
            else current.GetComponent<Canvas>().enabled = false;
            CanvasList.IncrementIndex();
        }
        Timeout.Instance.StartCoroutine(playCanvasAudio(next));
    }

    private static void showHelpUI(GameObject guiCanvas)
    {
        if (!GetHelpCanvasIgnoreList().Contains(guiCanvas))
        {
            var helpCanvas = GameObject.Find("HelpCanvas");
            helpCanvas.GetComponent<Canvas>().enabled = true;
            helpCanvas.transform.FindChild("DisablePanel").gameObject.SetActive(true);
        }
    }

    private static void enableUI()
    {
        var helpCanvas = GameObject.Find("HelpCanvas");
        if (helpCanvas != null)
            helpCanvas.transform.FindChild("DisablePanel").gameObject.SetActive(false);
    }

    private static IEnumerator playCanvasAudio(GameObject guiCanvas)
    {
        if (!GetAudioIgnoreList().Contains(guiCanvas))
        {
            var passReminder = guiCanvas.transform.FindChild("PASSReminder");
            if (passReminder != null && GameFlags.AdultIsPresent && GameFlags.HasSeenPASS)
            {
                var passLetters = passReminder.GetComponentsInChildren<Transform>().ToList();
                passLetters.Remove(passLetters.First(x => x.name.Equals(passReminder.name)));
                var passCanvas = GameObject.Find("PASSCanvas").transform;
                passLetters.ForEach(x => passCanvas.FindChild(x.name).gameObject.SetActive(true));
                Utilities.PlayAudio(passReminder.GetComponent<AudioSource>());
                Timeout.SetRepeatAudio(passReminder.GetComponent<AudioSource>());
                yield return new WaitForSeconds(passReminder.GetComponent<AudioSource>().clip.length);
            }
            else
            {
                Utilities.PlayAudio(guiCanvas.GetComponent<AudioSource>());
                Timeout.SetRepeatAudio(guiCanvas.GetComponent<AudioSource>());
                yield return new WaitForSeconds(guiCanvas.GetComponent<AudioSource>().clip.length);
            }

            var tiles = guiCanvas.transform.GetComponentsInChildren<ButtonDragDrop>().ToList();
            foreach (var child in tiles)
            {
                yield return Timeout.Instance.StartCoroutine(playTileAudio(child.transform));
            }
            enableUI();
            Timeout.StartTimers();
        }
    }

    private static IEnumerator playTileAudio(Transform tile)
    {
        var tileAudio = tile.GetComponent<AudioSource>();
        if (tileAudio == null) yield break;
        tile.GetComponent<Animator>().ResetTrigger("Normal");
        tile.GetComponent<Animator>().SetTrigger("ButtonGrow");
        Utilities.PlayAudio(tileAudio);
        yield return new WaitForSeconds(tileAudio.clip.length);
        tile.GetComponent<Animator>().SetTrigger("Normal");
    }
}
