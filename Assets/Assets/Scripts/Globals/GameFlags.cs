namespace Globals
{
    public static class GameFlags
    {
        public static bool MainTutorialHasRun = true;
        public static bool BucketTutorialHasRun = true;
        public static bool PuzzleTutorialHasRun = true;
        public static bool CameraTutorialHasRun = true;
        public static bool JoyStickTutorialHasRun = true;
        public static bool HasSeenPASS = false;
        public static bool AdultIsPresent = true;
        public static string ParentGender = "Mom";

        public static void ResetValues()
        {
            MainTutorialHasRun = false;
            JoyStickTutorialHasRun = false;
            BucketTutorialHasRun = false;
            PuzzleTutorialHasRun = false;
            CameraTutorialHasRun = false;
            HasSeenPASS = false;
            AdultIsPresent = false;
            ParentGender = "";
        }
    }
}