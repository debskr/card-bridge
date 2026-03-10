using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string playerName;

    public List<Card> hand = new List<Card>();
    public int bid;
    public int roundsWon;
    public float totalScore;

    public Player(string name)
    {
        this.playerName = name;
        this.hand = new List<Card>();
        this.bid = 0;
        this.roundsWon = 0;
        this.totalScore = 0;
    }
}
