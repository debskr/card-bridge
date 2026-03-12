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
            if (i < hand.Count)
            {
                myUICards[i].SetupCardData(hand[i]);//Setting from card list data to single cards
            }
            else
            {
                myUICards[i].gameObject.SetActive(false);
            }
            
        }
    }

    //public void CalculateCards()
    //{
    //    for (int i = 0; i < hand.Count; i++)
    //    {
    //        Card card = hand[i];

    //        if (card.suit == Suit.Spades)
    //        {
    //            Debug.Log("Found Spade of Player :"+ this.playerId);
    //        }
    //    }
    //}

    public void CalculateAndSetAIBid()
    {
        int estimatedBid = 0;

        for (int i = 0; i < hand.Count; i++)
        {
            Card card = hand[i];

            if (card.suit == Suit.Spades)
            {
                estimatedBid++;
            }
            else if (card.rank == Rank.Ace || card.rank == Rank.King)
            {
                estimatedBid++;
            }
        }

        if (estimatedBid < 1)
        {
            estimatedBid = 1;
        }

        SetBid(estimatedBid);
    }

    public void UpdateScoreUI()
    {
        bidText.text = bid.ToString();//Update UI bid value with Players selected Bid amount
        roundWonText.text = roundsWon.ToString();//Update Who won the round score
    }

    public void SetBid(int amount)
    {
        this.bid = amount; //Storing Players bid amount to Bid Variable
        UpdateScoreUI();
    }


}
