using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.UI;

namespace EggCatch
{
    public class Tutorial : MonoBehaviour
    {

        public GameObject[] instructions;
        private string lastSceneCompleted;
        public PlayerScript player;
        public bool shouldKeepScore = true;
        private float lastInput;
        private float animationDelay = 0.0f;

        private GameObject buttonDragDrop;

        public RawImage[] images;
        public GameObject[] badCatchIndicators;
        public GameObject goodCatchIndicator;

        private List<GameObject> stars;

        private void Start()
        {
            Timeout.StopTimers();
            lastSceneCompleted = Scenes.GetLastSceneCompleted();
            initializeObjects();
            StartCoroutine(DelayedPlayAudio());
        }

        private void initializeObjects()
        {
            buttonDragDrop = transform.FindChild("ButtonDrag").gameObject;
            stars = player.stars.ToList();

            images.ToList().ForEach(image =>
            {
                if (lastSceneCompleted.ToLower().Contains(image.name.ToLower()))
                {
                    goodCatchIndicator.GetComponent<RawImage>().texture = image.texture;
                }
                else
                {
                    var badCatch =
                        badCatchIndicators.ToList().FirstOrDefault(x => x.GetComponent<RawImage>().texture == null);
                    if (badCatch != null) badCatch.GetComponent<RawImage>().texture = image.texture;
                }
            });
        }

        private IEnumerator DelayedPlayAudio()
        {
            yield return new WaitForSeconds(1.0f);
            if (shouldKeepScore)
            {
                var instructions = GetAudioInstructions();
                Utilities.PlayAudio(instructions);
                stars.ForEach(x => x.SetActive(true));
                goodCatchIndicator.transform.parent.gameObject.SetActive(true);
                yield return new WaitForSeconds(instructions.clip.length);
                stars.ForEach(x => x.SetActive(false));

                var avoidInstructions = GetAudioInstructions("avoid");
                Utilities.PlayAudio(avoidInstructions);
                yield return new WaitForSeconds(avoidInstructions.clip.length);
                goodCatchIndicator.transform.parent.gameObject.SetActive(false);

                if (!GameFlags.BucketTutorialHasRun)
                {
                    var controlInstructions = GetControlInstructions();
                    ShowDraggingAnimation();
                    Utilities.PlayAudio(controlInstructions);
                    yield return new WaitForSeconds(controlInstructions.clip.length);
                    HideDraggingAnimation();
                    GameFlags.BucketTutorialHasRun = true;
                }
            }

            player.shouldDropEggs = true;
        }

        private void ShowDraggingAnimation()
        {
            transform.FindChild("ButtonDrag").gameObject.SetActive(true);
        }

        private void HideDraggingAnimation()
        {
            transform.FindChild("ButtonDrag").gameObject.SetActive(false);
        }

        private AudioSource GetAudioInstructions()
        {
            return (from instruction in instructions
                where lastSceneCompleted.Contains(instruction.name)
                select instruction.GetComponent<AudioSource>()).FirstOrDefault();
        }

        private AudioSource GetAudioInstructions(string name)
        {
            return (from instruction in instructions
                where instruction.name.ToLower().Equals(name)
                select instruction.GetComponent<AudioSource>()).FirstOrDefault();
        }

        private AudioSource GetControlInstructions()
        {
            return buttonDragDrop.GetComponent<AudioSource>();
        }
    }
}