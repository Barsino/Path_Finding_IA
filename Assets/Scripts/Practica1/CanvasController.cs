using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
	// Reference to the object containing the path controllers
    [SerializeField] GameObject pathController;

	// Variables for the different path controllers
    private Astar_OffLine astar_OffLine;
    private Astar_OnLine astar_OnLine;
    private UninformedSearch_OffLine uninformedSearch_OffLine;
    private UninformedSearch_OnLine uninformedSearch_OnLine;
    

	// References to user interface elements
    [SerializeField] private TextMeshProUGUI timeText; // Text to display the time
    [SerializeField] private TextMeshProUGUI currentmoveText; // Text to show the current movement
    [SerializeField] private TextMeshProUGUI movesLeftText; // Text to show remaining movements
    [SerializeField] private Button startButton; // Button to start search

    private void Awake()
    {
		// Add a listener to the button click event
        startButton.onClick.AddListener(OnClickButton);

		// Get the path controller components by type
        if(pathController.GetComponent<Astar_OffLine>())
        {
            astar_OffLine = pathController.GetComponent<Astar_OffLine>();
        }
        else if(pathController.GetComponent<Astar_OnLine>())
        {
            astar_OnLine = pathController.GetComponent<Astar_OnLine>();
        }
        else if(pathController.GetComponent<UninformedSearch_OffLine>())
        {
            uninformedSearch_OffLine = pathController.GetComponent<UninformedSearch_OffLine>();
        }
        else if (pathController.GetComponent<UninformedSearch_OnLine>())
        {
            uninformedSearch_OnLine = pathController.GetComponent<UninformedSearch_OnLine>();
        }
    }


    private void LateUpdate()
    {
		// Update the user interface according to the selected path controller
        if(astar_OffLine != null)
        {
            timeText.text = astar_OffLine.GetTime().ToString("F3") + " s";
            currentmoveText.text = astar_OffLine.GetCurrentMove();
            movesLeftText.text = astar_OffLine.GetPathMoves().ToString();
        }
        else if(astar_OnLine != null)
        {
            timeText.text = astar_OnLine.GetTime().ToString("F3") + " ms";
            currentmoveText.text = astar_OnLine.GetCurrentMove();
            movesLeftText.text = astar_OnLine.GetPathMoves().ToString();
        }
        else if (uninformedSearch_OffLine != null)
        {
            timeText.text = uninformedSearch_OffLine.GetTime().ToString("F3") + " ms";
            currentmoveText.text = uninformedSearch_OffLine.GetCurrentMove();
            movesLeftText.text = uninformedSearch_OffLine.GetPathMoves().ToString();
        }
        else if (uninformedSearch_OnLine != null)
        {
            timeText.text = uninformedSearch_OnLine.GetTime().ToString("F3") + " ms";
            currentmoveText.text = uninformedSearch_OnLine.GetCurrentMove();
            movesLeftText.text = uninformedSearch_OnLine.GetPathMoves().ToString();
        }
    }

    private void OnDestroy()
    {
		// Remove the listener from the button click event
        startButton.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {
		// Start the search in the corresponding path controller according to type
        if(astar_OffLine != null)
        {
            astar_OffLine.StartSearching();
        }
        else if(astar_OnLine != null)
        {
            astar_OnLine.StartSearching();
        }
        else if(uninformedSearch_OffLine != null)
        {
            uninformedSearch_OffLine.StartSearching();
        }
        else if (uninformedSearch_OnLine != null)
        {
            uninformedSearch_OnLine.StartSearching();
        }
    }
}
