using Assets.Scripts.DataStructures;
using Assets.Scripts;
using UnityEngine;

public class UninformedSearch_OnLine : UninformedSearch
{
    // Variable to store the position of the goal 
    private Vector2 goal;

    // Method to obtain the following movement 
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        // Starts search if enabled 
        if (startSearching)
        {
            FindPath(boardInfo, currentPos, goals);
        }

        // Checks if there are more moves on the way and returns the next move. 
        if (pathMoves.Count > 1)
        {
            if ((pathMoves[1] - pathMoves[0]) == new Vector2(1, 0))
            {
                currentMove = "Right";
                pathMoves.Clear();
                return Locomotion.MoveDirection.Right;
            }
            else if ((pathMoves[1] - pathMoves[0]) == new Vector2(-1, 0))
            {
                currentMove = "Left";
                pathMoves.Clear();
                return Locomotion.MoveDirection.Left;
            }
            else if ((pathMoves[1] - pathMoves[0]) == new Vector2(0, 1))
            {
                currentMove = "Up";
                pathMoves.Clear();
                return Locomotion.MoveDirection.Up;
            }
            else if ((pathMoves[1] - pathMoves[0]) == new Vector2(0, -1))
            {
                currentMove = "Down";
                pathMoves.Clear();
                return Locomotion.MoveDirection.Down;
            }
        }

        // If there are no more movements, return none. 
        return Locomotion.MoveDirection.None;
    }

    // Method for finding the way using Uninformed Search Online 
    protected override void FindPath(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        // Increase search time 
        timeToFindPath += Time.deltaTime;

        // Clear lists
        nodeQueue.Clear();
        pathMoves.Clear();
        visitedNodes.Clear();

        // Sets the target at the position of the first enemy if there are enemies, otherwise at the default target. 
        if (boardInfo.Enemies.Count > 0)
        {
            goal = new Vector2(boardInfo.Enemies[0].CurrentPosition().ColumnId, boardInfo.Enemies[0].CurrentPosition().RowId);
        }
        else
        {
            goal = new Vector2(goals[0].ColumnId, goals[0].RowId);
        }

        // Creates the first node at the current position and adds it to the queue 
        firstNode = new Node(new Vector2(currentPos.ColumnId, currentPos.RowId), null, nullVector2, false);
        nodeQueue.Enqueue(firstNode);

        while(nodeQueue.Count > 0)
        {
            currentNode = nodeQueue.Dequeue();
            currentCell = boardInfo.CellInfos[(int)currentNode.position.x, (int)currentNode.position.y];

            visitedNodes.Add(currentNode.position);

            if(currentNode.position == goal)
            {
                FinalPath(currentNode);
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

            if(nodeQueue.Count <= 0)
            {
                Debug.Log("Path not found.");
                return;
            }
        }
    }
}
