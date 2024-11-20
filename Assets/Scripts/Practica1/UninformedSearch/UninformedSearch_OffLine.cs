// This script implements a concrete version of UninformedSearch for an offline environment. 
// Defines how the next move is found to reach a target on a board.
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;

public class UninformedSearch_OffLine : UninformedSearch
{
    // Method that implements how the following movement is found 
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        // Check whether the path has been found and the search has been started 
        if (!pathFound && startSearching)
        {
            // If the path has not been found and the search has been started, find the path. 
            FindPath(boardInfo, currentPos, goals);

        }
        else
        {
            // If the path has been found, navigate through the path. 
            if (pathMoves.Count > 1)
            {
                // Get the direction of the next move based on the positions of consecutive cells.
                if ((pathMoves[1] - pathMoves[0]) == new Vector2(1, 0))
                {
                    currentMove = "Right";
                    pathMoves.RemoveAt(0);
                    return Locomotion.MoveDirection.Right;
                }
                else if ((pathMoves[1] - pathMoves[0]) == new Vector2(-1, 0))
                {
                    currentMove = "Left";
                    pathMoves.RemoveAt(0);
                    return Locomotion.MoveDirection.Left;
                }
                else if ((pathMoves[1] - pathMoves[0]) == new Vector2(0, 1))
                {
                    currentMove = "Up";
                    pathMoves.RemoveAt(0);
                    return Locomotion.MoveDirection.Up;
                }
                else if ((pathMoves[1] - pathMoves[0]) == new Vector2(0, -1))
                {
                    currentMove = "Down";
                    pathMoves.RemoveAt(0);
                    return Locomotion.MoveDirection.Down;
                }
            }
        }

        // If there is no next move, return None
        return Locomotion.MoveDirection.None;
    }

    // Method for finding the path using the uninformed search algorithm. 
    protected override void FindPath(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        // Increase search time 
        timeToFindPath += Time.deltaTime;

        // Clean the data structures of the algorithm 
        nodeQueue.Clear();
        pathMoves.Clear();
        visitedNodes.Clear();

        // Target cell position 
        Vector2 goal = new Vector2(goals[0].ColumnId, goals[0].RowId);

        // Create the first node with position in the cell we are in and add it to the node queue 
        firstNode = new Node(new Vector2(currentPos.ColumnId, currentPos.RowId), null, nullVector2, false);
        nodeQueue.Enqueue(firstNode);

        // Main loop of the search algorithm 
        while (nodeQueue.Count > 0 && !pathFound)
        {
            // Extract the node from the queue
            currentNode = nodeQueue.Dequeue();
            currentCell = boardInfo.CellInfos[(int)currentNode.position.x, (int)currentNode.position.y];

            // Mark current node as visited 
            visitedNodes.Add(currentNode.position);

            // Check if we reach the goal
            if (currentNode.position == goal)
            {
                // If the objective has been reached, reconstruct the final path and end the search. 
                Debug.Log("Goal reached.");
                Debug.Log("Time to find the Path: " + timeToFindPath.ToString("F3"));
                FinalPath(currentNode);
                pathFound = true;
                startSearching = false;
                return;
            }

            // Explore the neighbors of the current node 
            foreach (CellInfo neighbour in currentCell.WalkableNeighbours(boardInfo))
            {
                if (neighbour != null)
                {
                    Vector2 neighbourPosition = new Vector2(neighbour.ColumnId, neighbour.RowId);
                    if (!visitedNodes.Contains(neighbourPosition))
                    {
                        Node neighbourNode = new Node(new Vector2(neighbour.ColumnId, neighbour.RowId), currentNode, nullVector2, false);
                        nodeQueue.Enqueue(neighbourNode);
                    }
                }
            }
        }

        // If a path is not found, print a message and restart the search time. 
        if (!pathFound)
        {
            Debug.Log("Path not found");
            timeToFindPath = 0;
            startSearching = false;
            return;
        }
    }
}
