using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentTurn = 0;
    public int currentLead = 0;

    public List<Card> cardsOnTable = new List<Card>();

    public CardManager cardManager;

    public GameObject biddingUIPanel;
    private int playerBidSelection;

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
        
        biddingUIPanel.SetActive(true);
    }

    //Setting players bid amount from the UI
    public void SetBidAmount(int amount)
    {
        playerBidSelection = amount;
    }

    //Sending Bid amount to Set in players Bid value and UI
    public void ConfirmPlayerBid()
    {
        cardManager.players[0].SetPlayerBid(playerBidSelection);

        biddingUIPanel.SetActive(false);
    }
}
