using UnityEngine;
using System.Collections;

namespace Globals
{
    public class Timeout : MonoBehaviour
    {
        public float TimeUntilReset;
        public float TimeUntilRepeat;

        public static Timeout Instance;
        public static float ResetTime;
        public static float RepeatTime;
        public static AudioSource WarningAudio;
        public bool shouldDisableAfterAwake = false;

        private static AudioSource RepeatAudio;
        private static float repeatTimer = 0f;
        private static float resetTimer = 0f;
        private static bool shouldRunTimers = false;
        private static bool shouldReset = false;

        private void Awake()
        {
            WarningAudio = transform.FindChild("WarningAudio").GetComponent<AudioSource>();
            ResetTime = TimeUntilReset;
            RepeatTime = TimeUntilRepeat;
            Instance = gameObject.GetComponent<Timeout>();
            if (shouldDisableAfterAwake) enabled = false;
        }

        private void Update()
        {
            if (!shouldRunTimers) return;
            repeatTimer += Time.deltaTime;
            if (resetTimer >= ResetTime && !shouldReset)
            {
                Utilities.PlayAudio(WarningAudio);
                shouldReset = true;
                resetTimer = 0f;
            }
            else if (repeatTimer >= RepeatTime)
            {
                if (shouldReset) Application.LoadLevel("TitleScreen");
                resetTimer += repeatTimer;
                repeatTimer = 0f;
                StartCoroutine(PlayAudio());
            }
        }

        private IEnumerator PlayAudio()
        {
            shouldRunTimers = false;
            Utilities.PlayAudio(RepeatAudio);
            if (RepeatAudio != null) 
                yield return new WaitForSeconds(RepeatAudio.clip.length);
            StartTimers();
        }

        public static void StartTimers()
        {
            shouldRunTimers = true;
        }

        public static void StopTimers()
        {
            shouldRunTimers = false;
            shouldReset = false;
            repeatTimer = 0f;
            resetTimer = 0f;
        }

        public static void ResetValues()
        {
            RepeatTime = 15f;
            ResetTime = 60f;
            StopTimers();
        }

        public static void SetRepeatAudio(AudioSource audioToRepeat)
        {
            RepeatAudio = audioToRepeat;
        }

        public static AudioSource GetRepeatAudio()
        {
            return RepeatAudio;
        }
    }
}