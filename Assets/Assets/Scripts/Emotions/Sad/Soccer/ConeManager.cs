using System.Linq;
using UnityEngine;

namespace SadScene
{
    public class ConeManager : ObjectSequenceManager
    {
        public ObjectSequenceManager ballMissManager;
        public float[] conePositions;
        public LaneColor[] lanes;

        public float RandomizePositionZ()
        {
            if (currentIndex >= SequenceObjects.Length)
            {
                // middle lane is the correct lane for the final cone
                adjustLaneColors(conePositions[2]);
                return 80.52f;
            }
            var index = Random.Range(0, conePositions.Length);
            var objectPosition = SequenceObjects[currentIndex - 1].transform.localPosition;
            SequenceObjects[currentIndex - 1].transform.localPosition = new Vector3(objectPosition.x, objectPosition.y, conePositions[index]);
            adjustLaneColors(conePositions[index]);
            return SequenceObjects[currentIndex - 1].transform.position.z;
        }

        private void adjustLaneColors(float zPosition)
        {
            var positionIndex = conePositions.ToList().IndexOf(zPosition);
            lanes.ToList().ForEach(lane =>
            {
                lane.SetColor(lanes.ToList().IndexOf(lane) == positionIndex ? LaneColor.CORRECT_LANE_COLOR : LaneColor.WRONG_LANE_COLOR);
            });
            LaneAppear.shouldShowLanes = true;
        }
    }
}
