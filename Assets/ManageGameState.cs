using TMPro;
using UnityEngine;

public class ManageGameState : MonoBehaviour
{
    public TextMeshProUGUI totalScoreText;
    
    void Start()
    {
        var gameStates = GameObject.FindGameObjectsWithTag("GameState");

        if (gameStates.Length == 1)
        {
            return;
        }
        
        foreach (var gameState in gameStates)
        {
            if (gameState.GetComponent<GameState>().isOriginal)
            {
                gameState.GetComponent<GameState>().totalScoreText = totalScoreText;
            }
            else
            {
                Destroy(gameState);
            }
        }
    }
}