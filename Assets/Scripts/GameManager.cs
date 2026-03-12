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

    private List<int> cardPlayers = new List<int>();

    public CardManager cardManager;

    public GameObject biddingUIPanel;
    private int playerBidSelection;

    void Start()
    {
        currentLead = 0;
        cardsOnTable.Clear();
        cardPlayers.Clear();
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
        cardPlayers.Add(playerID);

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
        else
        {
            CalculateTrickWinner();
            
        }
    }

    public void CalculateTrickWinner()
    {
        Suit requiredSuit = cardsOnTable[0].suit;
        Card firstPlayerCard = cardsOnTable[0];
        int trickPlayerIndex = 0;

        for (int i = 1; i < cardsOnTable.Count; i++)
        {
            Card PlayedCard = cardsOnTable[i];

            //When A spade trumps another suit card
            if (PlayedCard.suit == Suit.Spades && firstPlayerCard.suit != Suit.Spades)
            {
                trickPlayerIndex = i;
            }
            //when Both are spades, higher rank wins
            else if(PlayedCard.suit == Suit.Spades && firstPlayerCard.suit == Suit.Spades)
            {
                if ((int)PlayedCard.rank > (int)firstPlayerCard.rank)
                {
                    trickPlayerIndex = i;
                }
            }
            //when there's no spades, needed to match required suit and higher rank wins
            else if(PlayedCard.suit == requiredSuit && firstPlayerCard.suit == requiredSuit)
            {
                if((int)PlayedCard.rank > (int)firstPlayerCard.rank)
                {
                    trickPlayerIndex = i;
                }
            }
        }
        //getting winner id from players
        int winnerID = cardPlayers[trickPlayerIndex];

        Debug.Log(winnerID);

        cardManager.players[winnerID].roundsWon += 1;
        cardManager.players[winnerID].UpdateScoreUI();

        currentTurn = winnerID;

        //Moving to next round
        StartCoroutine(NextRound());
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(2.0f);
        cardsOnTable.Clear();
        ClearTableUI();
        cardPlayers.Clear();

        if (cardManager.players[0].hand.Count == 0)
        {
            //Round over
        }
        else
        {
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

        //Getting Highest trump card on table
        bool isTrump = false;
        int highestSpadeOnTable = 0;

        for (int i = 0; i < cardsOnTable.Count; i++)
        {
            if (cardsOnTable[i].suit == Suit.Spades)
            {
                isTrump = true;
                if ((int)cardsOnTable[i].rank > highestSpadeOnTable)
                {
                    highestSpadeOnTable = (int)cardsOnTable[i].rank;
                }
            }
        }
        
        //Check if player has spade on hand
        bool hasSpade = false;
        for (int i = 0; i < player.hand.Count; i++)
        {
            if (player.hand[i].suit == Suit.Spades)
            {
                hasSpade = true;
                break;
            }
        }

        //check if the player spade is higher than the spade on table
        if (hasSpade)
        {

            for (int i = 0; i < player.hand.Count; i++)
            {
                if (player.hand[i].suit == Suit.Spades)
                {
                    if (isTrump && (int)player.hand[i].rank > highestSpadeOnTable)
                    {
                        validCards.Add(player.hand[i]);
                    }
                    else
                    {
                        validCards.Add(player.hand[i]);
                    }
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
