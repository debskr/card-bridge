using System.CodeDom.Compiler;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Card> cardDeck = new List<Card>(); //for storing all 52 cards
    public Player[] players = new Player[4];
    
    [System.Serializable]
    public class SuitSprites
    {
        public Suit suit; //defining Suits in list
        public Sprite[] rankSprites;//getting sprites for each suit from assets
    }

    public SuitSprites[] allSuitSprites;

    void Start()
    {
        GenerateDeckCards();
        ShuffleDeck();
        DealCards();
    }

    public void GenerateDeckCards()
    {
        cardDeck.Clear();

        for (int i = 0; i < 4; i++)
        {
            Suit currentSuit = (Suit)i;
            //Debug.Log(currentSuit);

            for (int j = 2; j <= 14; j++)
            {
                Rank currentRank = (Rank)j;
                Sprite cardSprite = GetSpriteForCard(currentSuit, j - 2);//geting 13 card sprites based on current suit

                Card newCard = new Card(currentSuit, currentRank, cardSprite);

                cardDeck.Add(newCard); //assigning cards to list of 52
            }
        }

        //Debug.Log(cardDeck.Count);
    }

    //Shuffle all 52 cards
    public void ShuffleDeck()
    {
        for (int i = 0; i < cardDeck.Count; i++)
        {
            int randomIndex = Random.Range(0, cardDeck.Count);

            Card tempCard = cardDeck[i];
            cardDeck[i] = cardDeck[randomIndex];
            cardDeck[randomIndex] = tempCard;
        }
        
    }

    public void DealCards()
    {
        //loop through all 52 cards
        for (int i = 0; i < cardDeck.Count; i++)
        {
            //loop through player index from 4 players
            int playerIndex = i % 4;
            players[playerIndex].playerId = playerIndex;//assigning playerID
            players[playerIndex].hand.Add(cardDeck[i]);//assigning 13 cards to each player 52/4
        }    
    }    

    public Sprite GetSpriteForCard(Suit suit, int rankIndex)
    {
        for (int i = 0; i < allSuitSprites.Length; i++)//loop through all suit list
        {
            if (allSuitSprites[i].suit == suit) //condition to look in to specific suit
            {
                if (rankIndex >= 0 && rankIndex < allSuitSprites[i].rankSprites.Length) //condition for looking through 13 index range for current suit
                {
                    return allSuitSprites[i].rankSprites[rankIndex]; //returning sprite
                    //Debug.Log(allSuitSprites[i].rankSprites[rankIndex]);
                }
            }
               
        }
        return null;//exception if card not assigned
    }
}
