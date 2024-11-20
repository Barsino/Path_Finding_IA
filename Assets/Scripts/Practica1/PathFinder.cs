using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
	// Enumeration for the available search algorithms
    public enum Algorithms
    {
        Astar_OffLine,
        Astar_OnLine,
        UninformedSearch_OffLine,
        UninformedSearch_OnLine,
    }

    [Space(10)]
    [SerializeField] 
	// Algorithm selected from the editor
    private Algorithms selectedAlgorithm;

    private void Awake()
    {
		// Switch to instantiate the component corresponding to the selected algorithm
        switch(selectedAlgorithm)
        {
            case Algorithms.Astar_OffLine:

                gameObject.AddComponent<Astar_OffLine>();

                break;

            case Algorithms.Astar_OnLine:

                gameObject.AddComponent<Astar_OnLine>();

                break;

            case Algorithms.UninformedSearch_OffLine:

                gameObject.AddComponent<UninformedSearch_OffLine>();

                break;

            case Algorithms.UninformedSearch_OnLine:

                gameObject.AddComponent<UninformedSearch_OnLine>();

                break;
        }
    }
}
