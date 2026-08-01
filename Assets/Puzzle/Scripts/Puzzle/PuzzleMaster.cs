using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace PuzzlePipes
{
    /// <summary>
    /// RotatingPiecesPuzzle.cs checks for the puzzle complete
    /// </summary>
    public class PuzzleMaster : MonoBehaviour
    {
        [Tooltip("If checked on, you'll be able to rotate pieces even when the puzzle is completed")]
        public bool RotatePiecesAfterCompleted;
        [Tooltip("Puzzle completed")]
        public bool completed;

        public Color DefaultPipeColour = Color.grey;
        public Color DefaultPieceColour = Color.grey;
        public Color ConnectedPipeColour = Color.blue;
        public Color SelectedPieceColour = Color.yellow;

        [HideInInspector]
        [Tooltip("The final connector(s) that must be reached")]
        public List<PuzzleConnect> endConnectors;
        public UnityEvent onPuzzleComplete;
        // private variables
        private List<GameObject> endConnectorsGO;
       

        private void Start()
        {
            endConnectorsGO = new List<GameObject>(GameObject.FindGameObjectsWithTag("EndConnector"));
            endConnectors = GetPuzzleConnectFromGOList(endConnectorsGO);
        }

        // check for puzzle complete
        private void Update()
        {
            CheckForPuzzleComplete();
        }

        /// <summary>
        /// Checks whether the puzzle is completed and invokes onPuzzleComplete event
        /// </summary>
        private void CheckForPuzzleComplete()
        {
            completed = true;
            // if all end connectors are receivers it means the puzzle is solved
            foreach (var endConnector in endConnectors)
            {
                if (!endConnector.IsReceiver)
                {
                    // if at least one connector is not a receiver then it means the puzzle is not completed and break 
                    completed = false;
                    return;
                }
            }

            if(completed)
            {
                // invoke onPuzzleComplete event on complete
                onPuzzleComplete.Invoke();
                // do whatever you wish here
                // you can put anything in "OnPuzzleComplete() on PuzzleMaster
                Debug.Log("Completed");
            }

        }

        /// <summary>
        /// Gets PuzzleConnect component from each list member and returns them as a list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<PuzzleConnect> GetPuzzleConnectFromGOList(List<GameObject> list)
        {
            int length = list.Count;
            if (length <= 0) return new();

            List<PuzzleConnect> newList = new();

            for (int i = 0; i < length; i++)
            {
                newList.Add(list[i].GetComponent<PuzzleConnect>());
            }
            return newList;
        }
    }
}