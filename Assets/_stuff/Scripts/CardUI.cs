using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descText;
    public Button button;

    private CardSO card;
    private LevelUpManager manager;

    public void Setup(CardSO card, LevelUpManager manager)
    {
        this.card = card;
        this.manager = manager;

        if(card.sprite != null) icon.sprite = card.sprite;
        nameText.text = card.itemName;
        descText.text = card.itemDesc;

        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        manager.ChooseCard(card);
    }
}
