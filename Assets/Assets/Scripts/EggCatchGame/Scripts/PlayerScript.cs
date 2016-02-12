using Globals;
using UnityEngine;

namespace EggCatch
{
    public class PlayerScript : MonoBehaviour
    {

        public int theScore = 0;
        public GameObject[] stars;
        public bool shouldDropEggs = false;
        public bool shouldKeepScore = true;
        public Animator Lily;
        public int MAX_SCORE = 5;
        public float inputSensitivity;

        private float lastInput;
        private float animationDelay = 0.0f;

        private void Start()
        {
            Timeout.StartTimers();
        }

        private void Update()
        {
            float moveInput = (Input.mousePosition.x / Screen.width) * 5f;
            if (shouldKeepScore)
            {
                if (moveInput == lastInput) Timeout.StartTimers();
                else Timeout.StopTimers();
            }
            else
            {
                animationDelay += Time.deltaTime;
                if (animationDelay >= 1)
                {
                    Lily.ResetTrigger("SwipeRight");
                    Lily.ResetTrigger("SwipeLeft");
                    Lily.SetTrigger(moveInput > 0 ? "SwipeRight" : "SwipeLeft");
                    animationDelay = 0.0f;
                }
            }
            
            transform.position = new Vector3(Mathf.Clamp(moveInput - 2.5f, -2.5f, 2.5f), transform.position.y, transform.position.z);

            lastInput = moveInput;
        }

        public void UpdateScore(int value)
        {
            if (!shouldKeepScore) return;
            if (theScore < MAX_SCORE) theScore += value;
            HideAllStars();
            ShowStars();
        }

        private void HideAllStars()
        {
            foreach (var star in stars)
            {
                star.SetActive(false);
            }
        }

        private void ShowStars()
        {
            for (var i = 0; i < theScore; ++i)
            {
                stars[i].SetActive(true);
            }
        }
    }
}