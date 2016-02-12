using System;
using UnityEngine;
using System.Collections;
using Globals;

public class ControllerMovement : MonoBehaviour 
{
    public MovementHandler movementHandler;
    public GameObject joystickCanvas;
    public Camera mainCamera;
    public float zMax = 80.767f;
    public float zMin = 80.366f;
    public GameObject[] joystickAnimations;
    public TutorialBase tutorial;
    public AudioSource initialInstructions;

    protected bool isWalking = false;
    protected float multiplierSpeed = 2f;
    protected float multiplierDirection = 0f;
    protected bool trackJoystick = false;
    protected bool shouldIgnoreLateral = false;
    protected bool shouldIgnoreForward = false;
    
    private bool initialInstructionsPlayed = false;
    private AudioSource joystickInstructions;
    private GameObject disableJoystickPanel;
    private Joystick joystickScript;

    protected virtual void Start()
    {
        joystickInstructions = joystickCanvas.GetComponent<AudioSource>();
        disableJoystickPanel = joystickCanvas.transform.FindChild("DisablePanel").gameObject;
        joystickScript = joystickCanvas.transform.FindChild("Base").FindChild("Stick").GetComponent<Joystick>();
        tutorial.InitializeGameObjects();
    }

    protected virtual void Update()
    {
        if (isWalking)
        {
            if (trackJoystick)
                movementHandler.HandleMovement(transform, joystickScript);
            else
                movementHandler.OverrideMovement(transform, Time.deltaTime * multiplierSpeed, Time.deltaTime * multiplierDirection);
        }
        trackJoystickMovement();
    }

    private void trackJoystickMovement()
    {
        if (trackJoystick)
        {
            if (joystickScript.CurrentSpeedAndDirection.y > 0) StartRunningAnimation();
            multiplierSpeed = joystickScript.CurrentSpeedAndDirection.y;
            multiplierDirection = joystickScript.CurrentSpeedAndDirection.x;
            // limit character's position laterally (z-direction)
            if (transform.position.z > zMax)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, zMax);
            }
            else if (transform.position.z < zMin)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, zMin);
            }
            // limit character's position so it can't move behind the camera
            if (Math.Abs(mainCamera.transform.position.x - transform.position.x) < 1.0f)
            {
                transform.position = new Vector3(mainCamera.transform.position.x + 1.0f, transform.position.y, transform.position.z);
            }
        }
    }

    protected virtual void StartRunningAnimation() {}

    protected virtual void AdjustCamera()
    {
        if (!joystickCanvas.activeInHierarchy) GUIHelper.NextGUI();
        joystickCanvas.GetComponent<Canvas>().enabled = true;
        mainCamera.transform.position = new Vector3(transform.position.x - 1.0f, transform.position.y + 3.0f, transform.position.z + 0.3f);
        mainCamera.transform.localRotation = Quaternion.Euler(33.56473f, 98.39697f, 5.486476f);
    }

    protected virtual void StartJoystickTutorial()
    {
        StartCoroutine(playJoystickInstructions());
    }

    private IEnumerator playJoystickInstructions()
    {
        tutorial.DisableHelpGUI();
        tutorial.ShowNoInputSymbol();
        if (!initialInstructionsPlayed)
        {
            Utilities.PlayAudio(initialInstructions);
            if (initialInstructions != null)
            {
                yield return new WaitForSeconds(initialInstructions.clip.length);
            }
            initialInstructionsPlayed = true;
        }
        if (!GameFlags.JoyStickTutorialHasRun)
        {
            foreach (var joystickAnimation in joystickAnimations)
            {
                joystickAnimation.SetActive(true);
                var audio = joystickAnimation.GetComponent<AudioSource>();
                Utilities.PlayAudio(audio);
                yield return new WaitForSeconds(audio.clip.length);
                joystickAnimation.SetActive(false);
            }
            GameFlags.JoyStickTutorialHasRun = true;
        }
        EnableJoystick();
    }

    protected virtual void EnableJoystick()
    {
        disableJoystickPanel.SetActive(false);
        trackJoystick = true;
        tutorial.EnableHelpGUI();
        EnableHelpGUI();
        Timeout.SetRepeatAudio(joystickInstructions);
        Timeout.StartTimers();
        multiplierSpeed = 0f;
        multiplierDirection = 0f;
        isWalking = true;
    }

    protected void HideJoystick(bool shouldStartTimers)
    {
        joystickCanvas.GetComponent<Canvas>().enabled = false;
        joystickScript.shouldStartTimers = shouldStartTimers;
    }

    protected void DisableHelpGUI()
    {
        tutorial.DisableHelpGUI();
    }

    protected void EnableHelpGUI()
    {
        GameObject.Find("HelpCanvas").GetComponent<Canvas>().enabled = true;
        tutorial.EnableHelpGUI();
    }

    protected void ResetAndDisableJoystick()
    {
        trackJoystick = false;
        joystickScript.ButtonRelease();
        disableJoystickPanel.SetActive(true);
    }
}
