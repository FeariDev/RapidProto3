using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class LevelUpManager : MonoBehaviour
{

    [Header("Cards")]
    public List<CardSO> allCards;
    public GameObject cardUIPrefab;
    public Transform cardUIParent;

    [Header("UI")]
    public GameObject levelUpScreen;

    void Start()
    {
        levelUpScreen.SetActive(false);

        Player.Instance.level.OnLevelUp += TriggerLevelUp;
    }


    void TriggerLevelUp()
    {
        Time.timeScale = 0f;

        levelUpScreen.SetActive(true);

        RevealCards();
    }

    public float cardRevealTime = 0.33f;
    public Button rerollButton;
    void RevealCards()
    {
        StartCoroutine(RevealCardSequence());
    }
    IEnumerator RevealCardSequence()
    {
        ToggleRerollButton(false);

        foreach (Transform child in cardUIParent)
            Destroy(child.gameObject);

        List<CardSO> chosen = PickRandomCards(3);

        
        foreach (CardSO card in chosen)
        {
            yield return new WaitForSecondsRealtime(cardRevealTime);

            GameObject cardUI = Instantiate(cardUIPrefab, cardUIParent);
            CardUI ui = cardUI.GetComponent<CardUI>();
            ui.Setup(card, this);
        }

        yield return new WaitForSecondsRealtime(cardRevealTime);

        ToggleRerollButton(true);

        yield return null;
    }

    void ToggleRerollButton(bool toggle)
    {
        if (toggle)
        {
            rerollButton.interactable = true;
        }
        else
        {
            rerollButton.interactable = false;
        }
    }
    
    public void Button_Reroll()
    {
        RevealCards();
    }



    public void ChooseCard(CardSO chosen)
    {
        Player.Instance.inventory.AddItem(chosen, 1);

        CloseLevelUp();
    }

    void CloseLevelUp()
    {
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    List<CardSO> PickRandomCards(int amount)
    {
        List<CardSO> chosen = new List<CardSO>();
        List<CardSO> pool = new List<CardSO>(allCards);

        for (int i = 0; i < amount; i++)
        {
            if (pool.Count == 0) break;

            CardSO card = GetWeightedRandom(pool);
            chosen.Add(card);
            pool.Remove(card);
        }

        return chosen;
    }

    CardSO GetWeightedRandom(List<CardSO> pool)
    {
        int totalWeight = 0;
        foreach (CardSO c in pool)
            totalWeight += c.weight;

        int rand = Random.Range(0, totalWeight);
        int sum = 0;

        foreach (CardSO c in pool)
        {
            sum += c.weight;
            if (rand < sum) return c;
        }

        return pool[0];
    }



    
}
