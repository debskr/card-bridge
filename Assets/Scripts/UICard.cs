using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    private Card myCardData;
    private Image cardImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetupCardData(Card assignedCard)
    {
        myCardData = assignedCard;

        // Safe check so it doesn't crash if an Image isn't attached
        if (myCardData != null)
        {
            this.gameObject.GetComponent<Image>().sprite = myCardData.cardSprite;
        }
    }
}
