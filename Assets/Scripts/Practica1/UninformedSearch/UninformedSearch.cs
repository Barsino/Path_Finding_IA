// This script defines an abstract class UninformedSearch that extends AbstractPathMind. 
// This class provides basic functionality for implementing uninformed search algorithms.
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;
using UnityEngine;

public abstract class UninformedSearch : AbstractPathMind
{
    // Data structures for the search algorithm
    protected Queue<Node> nodeQueue = new Queue<Node>();
    protected List<Vector2> pathMoves = new List<Vector2>();
    protected HashSet<Vector2> visitedNodes = new HashSet<Vector2>();

    // Variables for algorithm state control 
    protected bool pathFound = false;
    protected bool startSearching = false;

    // Variables to track the current position and the current node
    protected CellInfo currentCell;
    protected Node currentNode;
    protected Node firstNode;

    // Variables to measure search time and current movement 
    protected float timeToFindPath;
    protected string currentMove = "None";

    // Special Vector2 representing a null position
    protected Vector2 nullVector2 = new Vector2(float.NaN, float.NaN);

    // Abstract method to be implemented by the child classes to find the way 
    protected abstract void FindPath(BoardInfo boardInfo, CellInfo currentPos, CellInfo[] goals);

    // Method for reconstructing the final path once it has been found. 
    protected void FinalPath(Node currentNode)
    {
        Node _currentNode = currentNode;

        pathMoves.Add(_currentNode.position);

        while (_currentNode.parent != null)
        {
            _currentNode = _currentNode.parent;
            pathMoves.Insert(0, _currentNode.position);
        }
    }

    // Access methods to obtain information about the path, the current movement and time. 
    public int GetPathMoves() { return pathMoves.Count - 1; }
    public string GetCurrentMove() { return currentMove; }
    public void StartSearching() { startSearching = true; }
    public float GetTime() { return timeToFindPath; }
}
