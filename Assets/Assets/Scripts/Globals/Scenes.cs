using System.Collections.Generic;
using System.Linq;

namespace Globals
{
    public static class Scenes
    {
        public static bool LoadingSceneThroughDebugging = false;
        public static int CurrentMiniGameIndex = 0;
        public static string NextSceneToLoad;
        public static string SceneForTestingParentPresent = "SadSceneSmallCity";
        public static string SceneForTestingMinigames = "AngrySceneSmallCity";

        public static IList<string> CompletedScenes = new List<string>();

        public static IList<string> MiniGames = new List<string>
        {
            "EggDropMiniGame", "PuzzleMiniGame"
        };

        public static string GetNextMiniGame()
        {
            if (MiniGames.Count <= CurrentMiniGameIndex) return string.Empty;
            var gameToLoad = MiniGames[CurrentMiniGameIndex++];
            if (CurrentMiniGameIndex == MiniGames.Count) CurrentMiniGameIndex = 0;
            return gameToLoad;
        }

        public static void ResetValues()
        {
            CompletedScenes = new List<string>();
            NextSceneToLoad = "";
        }

        public static string GetLastSceneCompleted()
        {
            return CompletedScenes.Count == 0 ? SceneForTestingMinigames : CompletedScenes.Last();
        }

        public static string GetNextSceneToLoadForParentPresent()
        {
            return string.IsNullOrEmpty(NextSceneToLoad) ? SceneForTestingParentPresent : NextSceneToLoad;
        }
    }
}

