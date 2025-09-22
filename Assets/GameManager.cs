using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int player1Score = 0;
    public int player2Score = 0;

    public TMP_Text player1ScoreText;  // Assign in Inspector
    public TMP_Text player2ScoreText;  // Assign in Inspector

    public int winningScore = 5;  // Example: First to 5 points wins
    public int rearrangeThreshold = 3;

    public Vector3[] cupPositions;
    public GameObject[] cupObjects;  // Assign cup GameObjects in Inspector

    // Diamond positions for cups
    private Vector3[] diamondPositions = new Vector3[]
    {
        new Vector3(0, 0, 5),   // 1
        new Vector3(-1, 0, 4),  // 2
        new Vector3(1, 0, 4),   // 3
        new Vector3(-2, 0, 3),  // 4
        new Vector3(2, 0, 3),   // 5
        new Vector3(-3, 0, 2),  // 6
        new Vector3(3, 0, 2),   // 7
        new Vector3(-1, 0, 1),  // 8
        new Vector3(1, 0, 1),   // 9
        new Vector3(0, 0, 0)    // 10 (center cup)
    };

    //Line positions 
    private Vector3[] linePositions = new Vector3[]
    {
        new Vector3(-4.5f, 0, 0),  // 1
        new Vector3(-3.5f, 0, 0),  // 2
        new Vector3(-2.5f, 0, 0),  // 3
        new Vector3(-1.5f, 0, 0),  // 4
        new Vector3(-0.5f, 0, 0),  // 5
        new Vector3(0.5f, 0, 0),   // 6
        new Vector3(1.5f, 0, 0),   // 7
        new Vector3(2.5f, 0, 0),   // 8
        new Vector3(3.5f, 0, 0),   // 9
        new Vector3(4.5f, 0, 0)    // 10
    };


    public void AddScore(int playerID)
    {
        if (playerID == 1)
        {
            player1Score++;
            player1ScoreText.text = "Player 1: " + player1Score;

            if (player1Score >= winningScore)
            {
                EndGame(1);
            }
            else if (player1Score % rearrangeThreshold == 0) // Check if it's time to rearrange
            {
                RearrangeCups();
            }
        }
        else if (playerID == 2)
        {
            player2Score++;
            player2ScoreText.text = "Player 2: " + player2Score;

            if (player2Score >= winningScore)
            {
                EndGame(2);
            }
            else if (player2Score % rearrangeThreshold == 0) // Check if it's time to rearrange
            {
                RearrangeCups();
            }
        }
    }


    void EndGame(int winningPlayer)
    {
        Debug.Log("Player " + winningPlayer + " wins!");
        // Implement end game logic (e.g., reset game, show UI, etc.)
    }

   public void RearrangeCups()
    {
        Debug.Log("Rearranging cups...");

        Vector3[] selectedPositions = diamondPositions; // Change to linePositions for line arrangement if needed


        for (int i = 0; i < cupObjects.Length; i++)
        {
            if (i < cupPositions.Length)
            {
                cupObjects[i].transform.position = cupPositions[i]; // Move each cup to its new position
            }
            else
            {
                Debug.LogWarning("Not enough positions for all cups!"); // Safety check
            }
        }
    }
}
