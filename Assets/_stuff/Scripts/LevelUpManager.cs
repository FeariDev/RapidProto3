using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpManager : MonoBehaviour
{

    [Header("Cards")]
    public List<CardSO> allCards;
    public GameObject cardUIPrefab;
    public Transform cardUIParent;

    [Header("UI")]
    public GameObject levelUpScreen;

    private bool isLevelingUp = false;

    void Start()
    {
        levelUpScreen.SetActive(false);

        Player.Instance.level.OnLevelUp += TriggerLevelUp;
    }

    /*
    void Update()
    {
        if (playerLevelScript.levelUpExpRequirement <= playerLevelScript.Experience)
            TriggerLevelUp();
    }
    */

    public List<CardSO> chosen;
    void TriggerLevelUp()
    {
        isLevelingUp = true;
        Time.timeScale = 0f;

        levelUpScreen.SetActive(true);

        foreach (Transform child in cardUIParent)
            Destroy(child.gameObject);

        chosen = PickRandomCards(3);

        foreach (CardSO card in chosen)
        {
            GameObject cardUI = Instantiate(cardUIPrefab, cardUIParent);
            CardUI ui = cardUI.GetComponent<CardUI>();
            ui.Setup(card, this);
        }
    }

    public void ChooseCard(CardSO chosen)
    {
        // TODO: Apply card effect / add to inventory
        // e.g. PlayerInventory.instance.AddCard(chosen);

        Player.Instance.inventory.AddItem(chosen, 1);

        CloseLevelUp();
    }

    void CloseLevelUp()
    {
        levelUpScreen.SetActive(false);
        Time.timeScale = 1f;
        isLevelingUp = false;
    }

    List<CardSO> PickRandomCards(int amount)
    {
        List<CardSO> chosen = new List<CardSO>();
        List<CardSO> pool = new List<CardSO>(allCards);

        for (int i=0; i<amount; i++)
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
