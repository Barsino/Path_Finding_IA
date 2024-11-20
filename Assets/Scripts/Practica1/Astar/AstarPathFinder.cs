// This script defines an abstract class AstarPathFinder that inherits from AbstractPathMind.
// Provides basic functionalities for the implementation of A* informed search algorithms.
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AstarPathFinder : AbstractPathMind
{
	// Data structures for the A* search algorithm
    protected List<Node> openNodes = new List<Node>();
    protected List<Node> closedNodes = new List<Node>();
    protected List<Vector2> pathMoves = new List<Vector2>();

	// Variables for algorithm state control
    protected bool pathFound = false;
    protected bool startSearching = false;

    // Variables to track the current position and the current node
    protected CellInfo currentCell;
    protected Node currentNode;
    protected Node firstNode;

    // Variables for tracking current movement and search time
    protected string currentMove = "None";
    protected float timeToFindPath;

    // Abstract method to be implemented by the child classes to find the way
    protected abstract void FindPath(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals);

    // Method to sort a list of nodes according to their cost f and h
    protected void SortList_fCost_hCost(List<Node> list)
    {
        list.Sort((nodeA, nodeB) =>
        {
            int compare_fCost = nodeA.fCost.CompareTo(nodeB.fCost);
            if (compare_fCost == 0)
            {
                return nodeA.hCost.CompareTo(nodeB.hCost);
            }
            return compare_fCost;
        });
    }

    // Method to check if a node is in a list
    protected bool Is_inList(List<Node> list, Vector2 node)
    {
        int index = list.FindIndex(nodeInList => nodeInList.position == node);

        return index != -1;
    }

    // Method for reconstructing the final path once it has been found.
    protected void FinalPath(Node currentNode, Vector2 start)
    {
        pathMoves.Clear();

        Node _currentNode = currentNode;

        pathMoves.Add(_currentNode.position);

        while (_currentNode.position != start)
        {
            _currentNode = _currentNode.parent;
            pathMoves.Insert(0, _currentNode.position);
        }
    }

    // Access methods to obtain information about the path, the current movement and time. 
    public float GetTime() { return timeToFindPath; }
    public void StartSearching() { startSearching = true; }
    public string GetCurrentMove() { return currentMove; }
    public int GetPathMoves() { return pathMoves.Count - 1; }
}
