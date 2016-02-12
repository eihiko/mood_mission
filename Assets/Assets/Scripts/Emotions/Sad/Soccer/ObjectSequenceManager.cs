using UnityEngine;

namespace SadScene
{
    public class ObjectSequenceManager : MonoBehaviour
    {
        public GameObject[] SequenceObjects;
        public int currentIndex = 0;

        public virtual void NextInSequence()
        {
            if (currentIndex >= SequenceObjects.Length) return;
            SequenceObjects[currentIndex++].SetActive(true);
        }
    }
}
