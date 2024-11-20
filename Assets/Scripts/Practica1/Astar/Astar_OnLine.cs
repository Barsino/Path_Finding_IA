// This script implements a concrete version of AstarPathFinder for an online environment.
// Defines how to find the next move to reach a target on a board using the A* algorithm.
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UIElements;

public class Astar_OnLine : AstarPathFinder
{
    private int movesCount;

    private const int depth = 6;
    private int depthAux;

	// Method that implements how the next move is found
    public override Locomotion.MoveDirection GetNextMove(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
		// If the search has been started, find the path
        if(startSearching)
        {
            FindPath(boardInfo, currentPos, goals);
        }

		// If there are moves on the way, select the next move and clear the list of moves.
        if (pathMoves.Count > 1)
        {
            Vector2 nextMove = pathMoves[1] - pathMoves[0];
            movesCount++;
            if (nextMove == Vector2.right)
            {
                currentMove = "Right";
                pathMoves.Clear();
                Debug.Log(movesCount);
                return Locomotion.MoveDirection.Right;
            }
            else if (nextMove == Vector2.left)
            {
                currentMove = "Left";
                pathMoves.Clear();
                Debug.Log(movesCount);
                return Locomotion.MoveDirection.Left;
            }
            else if (nextMove == Vector2.up)
            {
                currentMove = "Up";
                pathMoves.Clear();
                Debug.Log(movesCount);
                return Locomotion.MoveDirection.Up;
            }
            else if (nextMove == Vector2.down)
            {
                currentMove = "Down";
                pathMoves.Clear();
                Debug.Log(movesCount);
                return Locomotion.MoveDirection.Down;
            }          
        }
		
		// If there is no next move, return None
        return Locomotion.MoveDirection.None;
    }

	// Method for finding the path using the A* algorithm
    protected override void FindPath(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
        // Increase search time
        //timeToFindPath = 0;
        timeToFindPath += Time.deltaTime;

        // Reset lists and variables related to the found path
        pathFound = false;
        openNodes.Clear();
        closedNodes.Clear();
        pathMoves.Clear();

        // Get start position
        Vector2 start = new Vector2(currentPos.ColumnId, currentPos.RowId);
        Vector2 goal;

        // Set goal as enemy if there are enemies
        if(boardInfo.Enemies.Count > 0)
        {
            goal = new Vector2(boardInfo.Enemies[0].CurrentPosition().ColumnId, boardInfo.Enemies[0].CurrentPosition().RowId);
        }
        else
        {
            goal = new Vector2(goals[0].ColumnId, goals[0].RowId);
        }

        float depthDistance = Mathf.Abs(goal.x - currentPos.ColumnId) + Mathf.Abs(goal.y - currentPos.RowId);

        if (depthDistance < depth)
        {
            depthAux = (int)depthDistance + 1;
        }
        else
        {
            depthAux = depth;
        }

        // Add the first node to the list of open nodes if it is empty
        if(openNodes.Count <= 0)
        {
            firstNode = new Node(new Vector2(start.x, start.y), null, goal, true);
            openNodes.Add(firstNode);
        }

		// Main loop of algorithm A*
        while (openNodes.Count > 0 && depthAux > 0)
        {

			// Sort the list of open nodes by cost f and h
            SortList_fCost_hCost(openNodes);
			
			// Select the node with the lowest cost f from the list of open nodes.
            currentNode = openNodes[0];
            currentCell = boardInfo.CellInfos[(int)currentNode.position.x, (int)currentNode.position.y];

			// Remove the selected node from the list of open nodes and add it to the list of closed nodes.
            openNodes.RemoveAt(0);
            closedNodes.Add(currentNode);

            if(currentNode.position == goal)
            {
                pathFound = true;
            }

			// Explore the current node's neighbours
            foreach (CellInfo neighbour in currentCell.WalkableNeighbours(boardInfo))
            {
                if (neighbour != null)
                {
                    // Check if the neighbour is already in the list of closed nodes
                    if (!Is_inList(closedNodes, new Vector2(neighbour.ColumnId, neighbour.RowId)))
                    {
						// Create a neighbour node
                        Node neighbourNode = new Node(new Vector2(neighbour.ColumnId, neighbour.RowId), currentNode, goal, true);

                        // Check if the neighbour is already in the list of open nodes
                        Node existingNode = openNodes.Find(node => node.position == neighbourNode.position);
                        if (existingNode == null)
                        {
							// If the neighbour is not in the list of open nodes, add it to the list.
                            openNodes.Add(neighbourNode);
                        }
                        // If is in openNodes reasign the cost and the parent
                        else if (neighbourNode.gCost < existingNode.gCost)
                        {
                            existingNode.SetParent(currentNode);
                            existingNode.Set_gCost(neighbourNode);
                            existingNode.Set_fCost();
                        }
                    }
                }
            }

            // Set that a path has been found if there are open nodes available.
            if (openNodes.Count > 0)
            {
                pathFound = true;
            }

            // If no path is available or the enemy is in a non-traversable cell, print a message and reset path-related variables.
            else
            {
                Debug.Log("Path not found.");
                timeToFindPath = 0;
                pathFound = false;
                return;
            }

            // Decrease the depth of the search
            depthAux--;

        }

        // Pick the best move
        FinalPath(currentNode, start);
        Debug.Log("Path found.");
    }
}
