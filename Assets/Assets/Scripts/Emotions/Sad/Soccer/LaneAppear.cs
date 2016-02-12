using System.Collections;
using System.Linq;
using UnityEngine;

namespace SadScene
{
    public class LaneAppear : MonoBehaviour
    {
        [HideInInspector] public static bool shouldShowLanes;
        private OutsideGroupSoccerAnimation soccerAnimation;

        private bool isIntersectingPlayer;

        private void Start()
        {
            soccerAnimation = GameObject.Find("Luis").GetComponent<OutsideGroupSoccerAnimation>();
        }

        public static void HideAllLanes()
        {
            GameObject.Find("Lanes")
                .GetComponentsInChildren<LaneColor>()
                .ToList()
                .ForEach(lane => lane.GetComponent<MeshRenderer>().enabled = false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<OutsideGroupSoccerAnimation>() != null)
            {
                if (!shouldShowLanes)
                {
                    isIntersectingPlayer = true;
                    return;
                }
                transform.parent.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        private bool laneIsCorrectLane()
        {
            return colorsMatch(transform.parent.GetComponent<Renderer>().material.color, LaneColor.CORRECT_LANE_COLOR);
        }

        private bool colorsMatch(Color color1 , Color color2)
        {
            return color1.r == color2.r && color1.g == color2.g && color1.b == color2.b;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<OutsideGroupSoccerAnimation>() != null && shouldShowLanes)
            {
                transform.parent.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        private void Update()
        {
            if (isIntersectingPlayer && shouldShowLanes)
            {
                isIntersectingPlayer = false;
                transform.parent.GetComponent<MeshRenderer>().enabled = true;
                if (laneIsCorrectLane()) soccerAnimation.IgnoreLateralMovement();
            }
        }
    }
}