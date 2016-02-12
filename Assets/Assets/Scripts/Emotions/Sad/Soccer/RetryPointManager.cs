using System.Linq;

namespace SadScene
{
    public class RetryPointManager : ObjectSequenceManager
    {
        public ObjectSequenceManager coneManager;
        
        public override void NextInSequence()
        {
            if (RetryPoint.PreviousRetryPoint != null && 
                SequenceObjects.ToList().IndexOf(RetryPoint.PreviousRetryPoint) + 1 == coneManager.currentIndex)
            {
                RetryPoint.PreviousRetryPoint.SetActive(true);
            }
            else
            {
                currentIndex = coneManager.currentIndex - 1;
                base.NextInSequence();
            }
        }
    }
}
