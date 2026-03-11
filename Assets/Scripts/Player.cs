using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public int playerId;
    public List<Card> hand = new List<Card>();
    public int bid;
    public int roundsWon;
    public float totalScore;

    public Player(int id)
    {
        this.playerId = id;
        this.hand = new List<Card>();
        this.bid = 0;
        this.roundsWon = 0;
        this.totalScore = 0;
    }
}
