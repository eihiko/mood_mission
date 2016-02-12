using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleMiniGame
{
    public class CreatePuzzlePieces : MonoBehaviour
    {
        public TakenImage image;
        public GameObject piecePrefab;
        public SceneReset sceneReset;
        public string sceneToLoadOnComplete;
        public RunTutorial tutorialCanvas;

        public List<GameObject> GeneratePuzzlePieces(List<GameObject> gridPanels, int dimensions, string panelBase,
            int width, int height)
        {
            var photo = image.Image;
            TextureScale.Bilinear(photo, width*dimensions, height*dimensions);
            photo.Apply();

            Color[] imageData;
            List<GameObject> pieces = new List<GameObject>();
            GameObject piece;
            int panelNumber = 1;

            for (int y = 0; y < dimensions; ++y)
            {
                for (int x = 0; x < dimensions; ++x)
                {
                    piece = (GameObject) Instantiate(piecePrefab);
                    piece.transform.SetParent(transform);
                    imageData = photo.GetPixels(x * width, y * height, width, height);
                    var texture = new Texture2D(width, height);
                    texture.SetPixels(imageData);
                    texture.Apply();

                    piece.GetComponent<RawImage>().texture = texture;
                    piece.GetComponent<RawImage>().SetNativeSize();
                    piece.GetComponent<PuzzleDragDrop>().correctContainer =
                        GetGridPanelByName(gridPanels, panelBase + panelNumber++).transform;
                    pieces.Add(piece);
                }
            }
            return pieces;
        }

        private GameObject GetGridPanelByName(List<GameObject> gridPanels, string name)
        {
            return gridPanels.FirstOrDefault(gridPanel => gridPanel.name.Equals(name));
        }

        private void ArrangePiecesWithGrids(List<GameObject> pieces)
        {
            foreach (var piece in pieces)
            {
                piece.transform.localPosition = piece.GetComponent<PuzzleDragDrop>().correctContainer.localPosition;
            }
        }

        public void RandomizePiecePositions(List<GameObject> pieces, AudioSource shuffleSound)
        {
            ArrangePiecesWithGrids(pieces);
            StartCoroutine(ShufflePositions(pieces, shuffleSound));
        }

        private void PieceAnimation(List<GameObject> pieces, bool playAnimation)
        {
            foreach (var piece in pieces)
            {
                if (playAnimation) piece.GetComponent<Animator>().SetTrigger("ShrinkPuzzle");
                piece.GetComponent<Animator>().enabled = playAnimation;
                if (!playAnimation) piece.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
            }
        }

        private IEnumerator ShufflePositions(List<GameObject> pieces, AudioSource shuffleSound)
        {
            yield return new WaitForSeconds(3.0f);
            PieceAnimation(pieces, true);
            yield return new WaitForSeconds(1.0f);
            PieceAnimation(pieces, false);
            var piecesMutable = new List<GameObject>(pieces);
            while (piecesMutable.Count > 0)
            {
                var first = piecesMutable[Random.Range(0, piecesMutable.Count)];
                piecesMutable.Remove(first);
                if (piecesMutable.Count == 0)
                {
                    first.GetComponent<PuzzleDragDrop>().correctContainer.GetComponent<GridPanel>().CurrentPuzzlePiece = first;
                    first.transform.localPosition = first.GetComponent<PuzzleDragDrop>().correctContainer.localPosition;
                    continue;
                };
                var second = piecesMutable[Random.Range(0, piecesMutable.Count)];
                piecesMutable.Remove(second);

                first.transform.localPosition = second.GetComponent<PuzzleDragDrop>().correctContainer.localPosition;
                second.transform.localPosition = first.GetComponent<PuzzleDragDrop>().correctContainer.localPosition;

                first.GetComponent<PuzzleDragDrop>().correctContainer.GetComponent<GridPanel>().CurrentPuzzlePiece = second;
                second.GetComponent<PuzzleDragDrop>().correctContainer.GetComponent<GridPanel>().CurrentPuzzlePiece = first;
                Utilities.PlayAudio(shuffleSound);
                yield return new WaitForSeconds(shuffleSound.clip.length);
            }

            yield return new WaitForSeconds(shuffleSound.clip.length);
            Utilities.PlayAudio(transform.parent.GetComponent<AudioSource>());
            Timeout.SetRepeatAudio(transform.parent.GetComponent<AudioSource>());
            yield return new WaitForSeconds(transform.parent.GetComponent<AudioSource>().clip.length);
            if (!GameFlags.PuzzleTutorialHasRun) tutorialCanvas.PlayTutorial(pieces);
            else
            {
                DisableCorrectlyPlacedPieces(pieces);
                GameObject.Find("DisablePanel").SetActive(false);
                Timeout.StartTimers();
            }
        }

        public void DisableCorrectlyPlacedPieces(List<GameObject> pieces)
        {
            foreach (var piece in pieces)
            {
                piece.GetComponent<PuzzleDragDrop>().CheckPieceCorrect();
            }
        }
    }
}