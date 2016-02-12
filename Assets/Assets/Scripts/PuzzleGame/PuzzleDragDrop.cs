using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Globals;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleMiniGame
{
    public class PuzzleDragDrop : MonoBehaviour
    {
        public Vector3 originalPosition;
        public bool disabled = false;
        public Transform correctContainer;

        private GameObject highlight;
        private GameObject[] gridPanels;
        private GameObject intersectingPanel;
        private static Transform currentlyDraggingPiece;

        private void Awake()
        {
            gridPanels = GameObject.FindGameObjectsWithTag("GUI");
        }

        private void Start()
        {
            highlight = correctContainer.FindChild("Highlight").gameObject;
        }

        private void Update()
        {
            if (Input.touchCount > 1 && currentlyDraggingPiece == transform)
            {
                ReturnToOriginalPosition();
                currentlyDraggingPiece = null;
            }
        }

        public void ClickPanel()
        {
            if (Input.touchCount > 1) return;
            currentlyDraggingPiece = transform;
            Timeout.StartTimers();
            originalPosition = transform.localPosition;
            Timeout.StartTimers();
        }

        public void MovePanel()
        {
            if (currentlyDraggingPiece != transform) return;
            if (disabled) return;
            Timeout.StopTimers();
            MoveToHierarchyBottom();
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // check if in range of container and highlight the container
            intersectingPanel = FindIntersectingPanel(gridPanels, gameObject);
            if (intersectingPanel != null)
            {
                ResetAllPanelColors(gridPanels);
                intersectingPanel.transform.FindChild("Highlight").gameObject.SetActive(true);
            }
            else
            {
                ResetAllPanelColors(gridPanels);
            }
        }

        public void PanelRelease()
        {
            if (currentlyDraggingPiece != transform) return;
            currentlyDraggingPiece = null;
            if (disabled) return;
            highlight.SetActive(false);
            if (intersectingPanel == null)
            {
                ReturnToOriginalPosition();
            }
            else
            {
                StartCoroutine(SubmitAnswer(intersectingPanel, intersectingPanel == correctContainer.gameObject));
            }
            Timeout.StartTimers();
        }

        private void ReturnToOriginalPosition()
        {
            transform.localPosition = originalPosition;
        }

        public void CheckPieceCorrect()
        {
            if (RectsOverlap(correctContainer.GetComponent<RectTransform>(), gameObject.GetComponent<RectTransform>()))
            {
                correctContainer.GetComponent<GridPanel>().CurrentPuzzlePiece = gameObject;
                disabled = true;
            }
            else
            {
                var panel = FindIntersectingPanel(gridPanels, gameObject);
                if (panel != null) panel.GetComponent<GridPanel>().CurrentPuzzlePiece = gameObject;
            }
        }

        private IEnumerator SubmitAnswer(GameObject intersectingPanel, bool isCorrectContainer)
        {
            if (intersectingPanel == null) yield break;
            SwapPieces(intersectingPanel);
            if (!isCorrectContainer) yield break;
            disabled = true;
            yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
            if (AllPiecesInCorrectPlaces())
            {
                GrowPieces();
                yield return new WaitForSeconds(1.0f);
                var parent = transform.parent;
                parent.GetComponent<CreatePuzzlePieces>().sceneReset.TriggerCorrect(parent.GetComponent<AudioSource>(), parent.GetComponent<CreatePuzzlePieces>().sceneToLoadOnComplete, true);
            }
        }

        private void GrowPieces()
        {
            foreach (var anim in gridPanels.Select(gridPanel => gridPanel.GetComponent<GridPanel>().CurrentPuzzlePiece.GetComponent<Animator>()))
            {
                anim.enabled = true;
                anim.SetTrigger("GrowPuzzle");
            }
        }

        public void SwapPieces(GameObject intersectingPanel)
        {
            transform.localPosition = intersectingPanel.transform.localPosition;
            if (intersectingPanel.GetComponent<GridPanel>().CurrentPuzzlePiece.GetComponent<PuzzleDragDrop>().disabled)
            {
                ReturnToOriginalPosition();
                return;
            }
            intersectingPanel.GetComponent<GridPanel>().CurrentPuzzlePiece.transform.localPosition = originalPosition;
            intersectingPanel.GetComponent<GridPanel>().CurrentPuzzlePiece.GetComponent<PuzzleDragDrop>().CheckPieceCorrect();
            intersectingPanel.GetComponent<GridPanel>().CurrentPuzzlePiece = gameObject;
            Utilities.PlayAudio(GetComponent<AudioSource>());
        }

        private bool AllPiecesInCorrectPlaces()
        {
            return gridPanels.All(gridPanel => gridPanel.GetComponent<GridPanel>().CurrentPuzzlePiece.GetComponent<PuzzleDragDrop>().disabled);
        }

        private bool RectsOverlap(RectTransform r1, RectTransform r2)
        {
            bool widthOverlap = (r1.position.x >= r2.position.x && r1.position.x <= r2.position.x + r2.rect.width * 0.4 * r2.localScale.x) ||
                                (r2.position.x >= r1.position.x && r2.position.x <= r1.position.x + r1.rect.width * 0.4 * r1.localScale.x);

            bool heightOverlap = (r1.position.y >= r2.position.y && r1.position.y <= r2.position.y + r2.rect.height * 0.4 * r2.localScale.y) ||
                                (r2.position.y >= r1.position.y && r2.position.y <= r1.position.y + r1.rect.height * 0.4 * r1.localScale.y);

            return (widthOverlap && heightOverlap);
        }

        private GameObject FindIntersectingPanel(GameObject[] panels, GameObject current)
        {
            return panels.FirstOrDefault(panel => RectsOverlap(current.GetComponent<RectTransform>(), panel.GetComponent<RectTransform>()));
        }

        private void ResetAllPanelColors(GameObject[] panels)
        {
            foreach (var panel in panels)
            {
                panel.transform.FindChild("Highlight").gameObject.SetActive(false);
            }
        }

        private void MoveToHierarchyBottom()
        {
            var parent = transform.parent;
            transform.SetParent(null);
            transform.SetParent(parent);
        }
    }
}