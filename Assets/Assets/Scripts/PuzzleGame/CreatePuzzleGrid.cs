using System;
using System.Collections.Generic;
using Globals;
using UnityEngine;

namespace PuzzleMiniGame
{
    public class CreatePuzzleGrid : MonoBehaviour
    {
        public GameObject gridPrefab;
        public CreatePuzzlePieces puzzlePieceGenerator;
        public RectTransform puzzlePieceParent;
        public GameObject disablePanel;
        public const string PANEL_BASE = "GridPanel";
        public int DIMENSIONS;

        private float X_LOWER_BOUND;
        private float Y_LOWER_BOUND;
        private float MAX_WIDTH;
        private float MAX_HEIGHT;

        public void StartGeneratingGrid()
        {
            Timeout.StopTimers();
            disablePanel.SetActive(true);

            // the game is only allowed in landscape mode so height < width always
            MAX_WIDTH = Math.Abs(puzzlePieceParent.rect.height);
            MAX_HEIGHT = Math.Abs(puzzlePieceParent.rect.height);
            X_LOWER_BOUND = puzzlePieceParent.rect.xMin;
            Y_LOWER_BOUND = puzzlePieceParent.rect.yMin;

            var gridPanels = GenerateGridPanels(DIMENSIONS, PANEL_BASE);
            puzzlePieceGenerator.RandomizePiecePositions(puzzlePieceGenerator.GeneratePuzzlePieces(gridPanels,
                DIMENSIONS, PANEL_BASE, (int)(MAX_WIDTH / DIMENSIONS), (int)(MAX_HEIGHT / DIMENSIONS)), GetComponent<AudioSource>());
        }

        private List<GameObject> GenerateGridPanels(int dimensions, string panelBase)
        {
            var gridList = new List<GameObject>();
            int counter = 1;
            var newWidth = MAX_WIDTH/dimensions;
            var newHeight = MAX_HEIGHT/dimensions;
            var scale = new Vector3(newWidth/gridPrefab.GetComponent<RectTransform>().rect.width,
                newHeight/gridPrefab.GetComponent<RectTransform>().rect.height);

            for (int y = 1; y <= dimensions; ++y)
            {
                for (int x = 1; x <= dimensions; ++x)
                {
                    var gridPanel = (GameObject) Instantiate(gridPrefab);
                    gridPanel.transform.SetParent(transform);
                    gridPanel.name = panelBase + counter;

                    gridPanel.transform.localScale = scale;

                    gridPanel.transform.localPosition = new Vector3(X_LOWER_BOUND + ((newWidth - 3)*x),
                        Y_LOWER_BOUND + (y*(newHeight - 3)), 0);

                    gridList.Add(gridPanel);
                    ++counter;
                }
            }
            return gridList;
        }
    }
}