using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Image icon;
    public Text nameText;

    private CardSO card;
    private LevelUpManager manager;

    public void Setup(CardSO card, LevelUpManager manager)
    {
        this.card = card;
        this.manager = manager;

        // if (icon != null) icon.sprite = card.cardSprite;
        if (nameText != null) nameText.text = card.name;
    }

    public void OnClick()
    {
        manager.ChooseCard(card);
    }
}
