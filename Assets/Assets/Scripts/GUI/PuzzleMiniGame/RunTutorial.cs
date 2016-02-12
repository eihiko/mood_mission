using System;
using Globals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleMiniGame
{
    public class RunTutorial : MonoBehaviour
    {
        public Transform buttonDrag;
        private bool showDragging = false;
        private Transform piece;
        private Transform gridPanel;
        private GameObject puzzlePieces;
        private GameObject gridPanels;
        private Vector3 initialPosition;

        public void PlayTutorial(List<GameObject> pieces)
        {
            puzzlePieces = GameObject.Find("PuzzlePieces");
            gridPanels = GameObject.Find("GridPanels");

            piece = gridPanels.transform.GetChild(0).GetComponent<GridPanel>().CurrentPuzzlePiece.transform;
            piece.GetComponent<PuzzleDragDrop>().originalPosition = piece.localPosition;
            gridPanel = gridPanels.transform.GetChild(2);

            StartCoroutine(ShowDragging(pieces));
        }

        private IEnumerator ShowDragging(List<GameObject> pieces)
        {
            piece.SetParent(buttonDrag.parent);
            // force buttonDrag to be on top of the puzzle piece
            buttonDrag.SetParent(null);
            buttonDrag.SetParent(piece.parent);
            initialPosition = new Vector3(piece.localPosition.x, buttonDrag.localPosition.y);
            buttonDrag.localPosition = initialPosition;
            buttonDrag.gameObject.SetActive(true);

            showDragging = true;
            Utilities.PlayAudio(GetComponent<AudioSource>());
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
            showDragging = false;

            piece.SetParent(puzzlePieces.transform);
            piece.localPosition = gridPanel.localPosition;
            piece.GetComponent<PuzzleDragDrop>().SwapPieces(gridPanel.gameObject);
            puzzlePieces.GetComponent<CreatePuzzlePieces>().DisableCorrectlyPlacedPieces(pieces);

            GameObject.Find("DisablePanel").SetActive(false);
            gameObject.SetActive(false);
            Timeout.StartTimers();
            GameFlags.PuzzleTutorialHasRun = true;
        }

        private void Update()
        {
            if (showDragging)
            {
                buttonDrag.localPosition = new Vector3(buttonDrag.localPosition.x + Time.deltaTime * (Screen.width / 300f) * 100f,
                    buttonDrag.localPosition.y);
                if (buttonDrag.localPosition.x >=
                    gridPanel.GetComponent<GridPanel>().CurrentPuzzlePiece.transform.localPosition.x / 2.0f)
                {
                    buttonDrag.localPosition = initialPosition;
                }
                piece.localPosition = new Vector3(buttonDrag.localPosition.x, piece.localPosition.y);
            }
        }
    }
}