using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    private Card myCardData;
    //private Image cardImage;

    public void SetupCardData(Card assignedCard)
    {
        myCardData = assignedCard;

        // Check if myCardData in not null
        if (myCardData != null)
        {
            //Setting sprite data to this card instance directly
            this.gameObject.GetComponent<Image>().sprite = myCardData.cardSprite;
        }
    }
}
