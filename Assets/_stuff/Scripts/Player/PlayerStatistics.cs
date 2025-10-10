using System;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    [SerializeField] PlayerStatistic[] playerStatistics;



    [Button]
    void InitializePlayerStatisticsArray()
    {
        PlayerStatistic.StatisticType[] array = (PlayerStatistic.StatisticType[])Enum.GetValues(typeof(PlayerStatistic.StatisticType));
        Array.Resize(ref playerStatistics, array.Length);

        for (int i = 0; i < playerStatistics.Length; i++)
        {
            if (playerStatistics[i] == null) playerStatistics[i] = new PlayerStatistic();
            else
            {
                playerStatistics[i].finalValue = playerStatistics[i].baseValue;
            }
            playerStatistics[i].statisticType = array[i];

            //Debug.Log($"Initialized player statistic '{playerStatistics[i].statisticType}'");
        }

        //Debug.Log($"Player statistics initialized!");
    }

    void GetPlayerStatisticsBaseValues()
    {
        foreach (PlayerStatistic statistic in playerStatistics)
        {
            switch (statistic.statisticType)
            {
                case PlayerStatistic.StatisticType.MoveSpeed:
                    statistic.baseValue = Player.Instance.movement.baseMoveSpeed;
                    break;
                case PlayerStatistic.StatisticType.AttackSpeed:
                    statistic.baseValue = Player.Instance.attack.currentWeapon.baseCooldown;
                    break;
                case PlayerStatistic.StatisticType.AttackDamage:
                    statistic.baseValue = Player.Instance.attack.currentWeapon.baseDamage;
                    break;
                case PlayerStatistic.StatisticType.Defense:
                    
                    break;
            }
        }
    }

    void ApplyPlayerStatistics()
    {
        foreach (PlayerStatistic statistic in playerStatistics)
        {
            switch (statistic.statisticType)
            {
                case PlayerStatistic.StatisticType.MoveSpeed:
                    Player.Instance.movement.moveSpeed = statistic.finalValue;
                    break;
                case PlayerStatistic.StatisticType.AttackSpeed:
                    Player.Instance.attack.currentWeapon.cooldown = statistic.finalValue;
                    break;
                case PlayerStatistic.StatisticType.AttackDamage:
                    Player.Instance.attack.currentWeapon.damage = statistic.finalValue;
                    break;
                case PlayerStatistic.StatisticType.Defense:
                    
                    break;
            }
        }
    }

    void CalculatePlayerStatistics()
    {
        foreach (PlayerStatistic statistic in playerStatistics)
        {
            statistic.modifierFlat = 0;
            statistic.modifierMultiplier = 0;

            foreach (StatisticModifier modifier in statistic.statisticModifiers)
            {
                switch (modifier.cardModifier.modifierType)
                {
                    case CardModifier.ModifierType.Flat:
                        statistic.modifierFlat += modifier.cardModifier.modifierValue * modifier.amount;
                        break;
                    case CardModifier.ModifierType.Percentage:
                        statistic.modifierMultiplier += modifier.cardModifier.modifierValue / 100 * modifier.amount;
                        break;
                }
            }

            statistic.finalValue = statistic.baseValue + (statistic.baseValue * statistic.modifierFlat);
        }
    }
    void WeaponSwitchedUpdate(int value)
    {
        GetPlayerStatisticsBaseValues();
        CalculatePlayerStatistics();
        ApplyPlayerStatistics();
    }



    void OnInventoryUpdate(InventorySlot slot)
    {
        if (slot.itemType.GetType() != typeof(CardSO)) return;

        CardSO card = (CardSO)slot.itemType;

        GetPlayerStatisticsBaseValues();
        ApplyCardModifier(card, slot.itemAmount);
        CalculatePlayerStatistics();
        ApplyPlayerStatistics();
    }

    void ApplyCardModifier(CardSO card, int cardAmount)
    {
        foreach (CardModifier modifier in card.cardModifiers)
        {
            for (int i = 0; i < playerStatistics.Length; i++)
            {
                bool isMatchingStatistic = playerStatistics[i].statisticType == modifier.modifierTarget;

                if (!isMatchingStatistic) continue;

                for (int j = 0; j <= playerStatistics[i].statisticModifiers.Length; j++)
                {
                    if (j == playerStatistics[i].statisticModifiers.Length)
                    {
                        Array.Resize(ref playerStatistics[i].statisticModifiers, playerStatistics[i].statisticModifiers.Length + 1);

                        StatisticModifier statisticModifier = new StatisticModifier();
                        statisticModifier.cardModifier = modifier;
                        statisticModifier.amount = cardAmount;
                        playerStatistics[i].statisticModifiers[playerStatistics[i].statisticModifiers.Length - 1] = statisticModifier;
                        break;
                    }

                    if (playerStatistics[i].statisticModifiers[j].cardModifier != modifier) continue;

                    playerStatistics[i].statisticModifiers[j].amount = cardAmount;
                    break;
                }

                playerStatistics[i].modifierFlat = 0;
                playerStatistics[i].modifierMultiplier = 0;

                foreach (StatisticModifier modifier1 in playerStatistics[i].statisticModifiers)
                {
                    switch (modifier1.cardModifier.modifierType)
                    {
                        case CardModifier.ModifierType.Flat:
                            playerStatistics[i].modifierFlat += modifier1.cardModifier.modifierValue * modifier1.amount;
                            break;
                        case CardModifier.ModifierType.Percentage:
                            playerStatistics[i].modifierMultiplier += modifier1.cardModifier.modifierValue / 100 * modifier1.amount;
                            break;
                    }
                }

                playerStatistics[i].finalValue = playerStatistics[i].baseValue + (playerStatistics[i].baseValue * playerStatistics[i].modifierMultiplier + playerStatistics[i].modifierFlat);
            }
        }
    }



    #region Unity lifecycle

    void Start()
    {
        InitializePlayerStatisticsArray();
        WeaponSwitchedUpdate(0);

        Player.Instance.inventory.OnInventoryUpdate += OnInventoryUpdate;
        Player.Instance.attack.OnWeaponSwitched += WeaponSwitchedUpdate;
    }

    #endregion
}

[Serializable]
public class PlayerStatistic
{
    public StatisticType statisticType;
    public enum StatisticType
    {
        MoveSpeed,
        AttackSpeed,
        AttackDamage,
        Defense
    }

    public float baseValue;
    public float modifierFlat;
    public float modifierMultiplier;
    public float finalValue;

    public StatisticModifier[] statisticModifiers;
}

[Serializable]
public class StatisticModifier
{
    public CardModifier cardModifier;
    public int amount;
}
