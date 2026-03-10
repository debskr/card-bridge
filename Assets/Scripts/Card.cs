using UnityEngine;

[System.Serializable]
public class Card
{
    public Suit suit;
    public Rank rank;
    public Sprite cardSprite;

    public Card(Suit suit, Rank rank, Sprite sprite)
    {
        this.suit = suit;
        this.rank = rank;
        this.cardSprite = sprite;
    }
}
