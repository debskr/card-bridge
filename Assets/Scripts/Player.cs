using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int playerId;
    public List<Card> hand = new List<Card>();
    public UICard[] myUICards = new UICard[13];

    public TextMeshProUGUI bidText;
    public TextMeshProUGUI roundWonText;
    public TextMeshProUGUI totalScoreText;

    public int bid;
    public int roundsWon;
    public float totalScore;

    public void SetupPlayer(int id)
    {
        this.playerId = id;
        hand.Clear();
        this.bid = 0;
        this.roundsWon = 0;
        this.totalScore = 0;
    }

    public void ReceiveCard(Card matchedCard)
    {
        hand.Add(matchedCard);
    }

    public void UpdateHandUI()
    {
        for (int i = 0; i < myUICards.Length; i++)
        {
            if(i < hand.Count)
            {
                myUICards[i].SetupCardData(hand[i]);//Setting from card list data to single cards
            }
            
        }
    }

    public void UpdateScoreUI()
    {
        bidText.text = bid.ToString();//Update UI bid value with Players selected Bid amount
    }

    public void SetPlayerBid(int amount)
    {
        this.bid = amount; //Storing Players bid amount to Bid Variable
        UpdateScoreUI();
    }


}
