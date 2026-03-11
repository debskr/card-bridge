using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentTurn = 0;
    public int currentLead = 0;

    public List<Card> cardsOnTable = new List<Card>();

    public CardManager cardManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLead = 0;
        cardsOnTable.Clear();

        StartNewLead();
    }

    public void StartNewLead()
    {
        currentTurn = Random.Range(0, 4);

        if(cardManager != null)
        {
            cardManager.InitializePlayers();
            cardManager.GenerateDeckCards();
            cardManager.ShuffleDeck();
            cardManager.DealCards();
        }    
    }
}
