using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	// Node g, h and f costs
    public int gCost { get; private set; } // Cumulative cost from the initial node to this node
    public int hCost { get; private set; } // Heuristic cost from this node to the target node
    public int fCost { get; private set; } // Sum of gCost and hCost, used to determine the search priority

	// Position of the node in the game space
    public Vector2 position { get; private set; }
	
	// Position of the target in the game space
    public Vector2 goal { get; private set; }
	
	// Parent node on the way to this node
    public Node parent { get; private set; }

	
	// Node class constructor
    public Node(Vector2 position, Node parent, Vector2 goal, bool heuristicNode)
    {
        this.position = position;
        this.parent = parent;
        this.goal = goal;

		// If specified as a heuristic node, calculate the costs g, h and f
        if(heuristicNode)
        {
            gCost = Set_gCost(parent);
            hCost = Set_hCost(goal, position);
            fCost = Set_fCost();
        }
    }


    #region Set
	// Method for calculating the node cost g
    public int Set_gCost(Node parent)
    {
        if(parent != null)
        {
            int cost = parent.gCost + 1;
            return cost;
        }
        else
        {
			// If there is no parent node, the cost is 0 (for the initial node).
            return 0;
        }
    }

	// Method for calculating the (heuristic) node cost h
    public int Set_hCost(Vector2 goal, Vector2 position)
    {
		// Calculates the Manhattan distance between the node's position and the target's position.
        float manhattanDistance = Mathf.Abs(goal.x - position.x) + Mathf.Abs(goal.y - position.y);
		
		// Converts the distance from float to int and returns it
        return (int)manhattanDistance;
    }

	// Method for calculating node cost f
    public int Set_fCost()
    {
        return gCost + hCost;
    }

	// Method to set the parent node
    public void SetParent(Node _parent)
    {
        this.parent = _parent;
    }
    #endregion
}
