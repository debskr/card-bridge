using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Card> cardDeck = new List<Card>(); //for storing all 52 cards
    
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

        Debug.Log(cardDeck.Count);
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
