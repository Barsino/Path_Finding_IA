// This script implements a specific version of AstarPathFinder for an offline environment.
// Defines how to find the next move to reach a target on a board using the A* algorithm.

using Assets.Scripts;
using Assets.Scripts.DataStructures;
using UnityEngine;

public class Astar_OffLine : AstarPathFinder
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
	
	// Method to find the path using the A* algorithm
    protected override void FindPath(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals)
    {
		// Clean the node lists
        openNodes.Clear();
        closedNodes.Clear();
        pathMoves.Clear();
		
		// Get start and target coordinates
        Vector2 start = new Vector2(currentPos.ColumnId, currentPos.RowId);
        Vector2 goal = new Vector2(goals[0].ColumnId, goals[0].RowId);
		
		// Add the first node to the list of open nodes
        if (openNodes.Count == 0)
        {
            firstNode = new Node(new Vector2(start.x, start.y), null, goal, true);
            openNodes.Add(firstNode);
        }
		
		// Main loop of algorithm A*
        while (openNodes.Count > 0 && !pathFound)
        {
			// Increase search time
            timeToFindPath += Time.deltaTime;

			// Sort the list of open nodes by cost f and h
            SortList_fCost_hCost(openNodes);
			
			// Select the node with the lowest cost f from the list of open nodes.
            currentNode = openNodes[0];
            currentCell = boardInfo.CellInfos[(int)currentNode.position.x, (int)currentNode.position.y];

			// Remove the selected node from the list of open nodes and add it to the list of closed nodes.
            openNodes.RemoveAt(0);
            closedNodes.Add(currentNode);

            // Check if we reach the goal
            if (currentNode.position == goal)
            {
				// If the objective has been reached, reconstruct the final path and end the search.
                Debug.Log("Goal reached.");
                Debug.Log("Time to find the Path: " + timeToFindPath.ToString("F3"));
                FinalPath(currentNode, start);
                pathFound = true;
                return;
            }

			// Explore the neighbours of the current node
            foreach (CellInfo neighbour in currentCell.WalkableNeighbours(boardInfo))
            {
                if (neighbour != null)
                {
                    // Check if the neighbour is already in the list of closed nodes
                    if (!Is_inList(closedNodes, new Vector2(neighbour.ColumnId, neighbour.RowId)))
                    {
						// Create a node for the neighbour
                        Node neighbourNode = new Node(new Vector2(neighbour.ColumnId, neighbour.RowId), currentNode, goal, true);

                        // Check if the neighbour is already in the list of open nodes
                        Node existingNode = openNodes.Find(node => node.position == neighbourNode.position);
                        if(existingNode == null)
                        {
							// If the neighbour is not in the list of open nodes, add it to the list.
                            openNodes.Add(neighbourNode);
                        }
                        else if(neighbourNode.gCost < existingNode.gCost)
                        {
							// If the neighbour is already in the list of open nodes but the new path is better, update the node in the list.
                            existingNode.SetParent(currentNode);
                            existingNode.Set_gCost(neighbourNode);
                            existingNode.Set_fCost();
                        }
                    }
                }

            }
        }

		// If a path is not found, print a message and restart the search time.
        if(!pathFound)
        {
            Debug.Log("Path not found.");

            timeToFindPath = 0;
            pathFound = false;
            startSearching = false;
            return;
        }
    }
}
