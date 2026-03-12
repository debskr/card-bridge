using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public int currentTurn = 0;
    public int currentLead = 0;

    public List<Card> cardsOnTable = new List<Card>();

    public UICard[] tableCards = new UICard[4];

    public CardManager cardManager;

    public GameObject biddingUIPanel;
    private int playerBidSelection;

    void Start()
    {
        currentLead = 0;
        cardsOnTable.Clear();
        ClearTableUI();
        StartNewLead();
    }

    public void ClearTableUI()
    {
        if (tableCards != null)
        {
            for (int i = 0; i < tableCards.Length; i++)
            {
                if (tableCards[i] != null)
                {
                    tableCards[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void StartNewLead()
    {
        currentTurn = Random.Range(0, 4);

        if (cardManager != null)
        {
            cardManager.InitializePlayers();
            cardManager.GenerateDeckCards();
            cardManager.ShuffleDeck();
            cardManager.DealCards();
            //AIPlayersCardCalculate();
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
        cardManager.players[0].SetBid(playerBidSelection);

        biddingUIPanel.SetActive(false);

        StartCoroutine(CalculateAIBidding());
    }

    //public void AIPlayersCardCalculate()
    //{
    //    for (int i = 1; i < cardManager.players.Length; i++)
    //    {
    //        cardManager.players[i].CalculateCards();
    //    }
    //}

    IEnumerator CalculateAIBidding()
    {
        for (int i = 1; i < cardManager.players.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

            cardManager.players[i].CalculateAndSetAIBid();
        }

        StartGamePhase();
    }

    public void StartGamePhase()
    {
        if (currentTurn == 0)
        {
            Debug.Log("Waiting for Player");
        }
        else
        {
            Debug.Log("AI " + currentTurn + " is Playing");
            StartCoroutine(AITurnRoutine());
        }
    }

    IEnumerator AITurnRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));

        Player ai = cardManager.players[currentTurn];
        
        List<Card> validCards = GetValidCards(ai);
        
        int randomIndex = Random.Range(0, validCards.Count);
        Debug.Log(randomIndex);
        Card choosenCard = validCards[randomIndex];

        PlayCardSequence(choosenCard, currentTurn);
    }

    public void TryPlayCard(Card cardInfo)
    {
        if (currentTurn != 0)
        {
            return;
        }
        
        List<Card> validCards = GetValidCards(cardManager.players[0]);
        bool isValid = false;

        for (int i = 0; i < validCards.Count; i++)
        {
            if (validCards[i] == cardInfo)
            {
                isValid = true;
                PlayCardSequence(cardInfo, 0);
                break;
            }
        }
        
        if(isValid == false)
        {
            Debug.Log("Play Required Card Only");
        }
    }

    public void PlayCardSequence(Card playedCard, int playerID)
    {
        Player currentPlayer = cardManager.players[playerID];

        currentPlayer.hand.Remove(playedCard);

        currentPlayer.UpdateHandUI();

        cardsOnTable.Add(playedCard);

        if (tableCards != null && playerID < tableCards.Length)
        {
            if (tableCards[playerID] != null)
            {
                tableCards[playerID].gameObject.SetActive(true);
                tableCards[playerID].SetupCardData(playedCard);
            }
        }

        Debug.Log("Player " + playerID + " played " + playedCard.rank + " of " + playedCard.suit);

        DetermineNextTurn();
    }

    public void DetermineNextTurn()
    {
        if (cardsOnTable.Count < 4)
        {
            currentTurn++;
            if (currentTurn > 3)
            {
                currentTurn = 0;
            }

            StartGamePhase();
        }
    }

    public List<Card> GetValidCards(Player player)
    {
        List<Card> validCards = new List<Card>();

        //on the first play required card is not needed and play any card
        if (cardsOnTable.Count == 0)
        {
            for (int i = 0; i < player.hand.Count; i++)
            {
                validCards.Add(player.hand[i]);
            }
            return validCards;
        }

        //getting the required suit after a card deal
        Suit requiredSuit = cardsOnTable[0].suit;
        
        bool hasRequiredSuit = false;
        for (int i = 0; i < player.hand.Count; i++)
        {
            if (player.hand[i].suit == requiredSuit)
            {
                hasRequiredSuit = true;
                break;
            }
        }
        //add required suit cards from hand to valid cards
        if (hasRequiredSuit)
        {
            for (int i = 0; i < player.hand.Count; i++)
            {
                if (player.hand[i].suit == requiredSuit)
                {
                    validCards.Add(player.hand[i]);
                }
            }

            return validCards;
        }
        
        //if does not have required suit then play any card
        for (int i = 0; i < player.hand.Count; i++)
        {
            validCards.Add(player.hand[i]);
        }

        return validCards;
    }
}
