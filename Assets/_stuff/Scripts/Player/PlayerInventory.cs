using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] InventorySlot[] inventory;

    /// <summary>
    /// Gets called when an item in the inventory is added/removed.
    /// </summary>
    public event Action<InventorySlot> OnInventoryUpdate;



    /// <summary>
    /// Tries to add the specified <paramref name="amount"/> of <paramref name="item"/> to the inventory. 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns>The amount of items that were left over. </returns>
    public int AddItem(ItemSO item, int amount)
    {
        int leftOverItems = amount;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].itemType != item) continue;

            int newStackAmount = inventory[i].itemAmount + amount;
            int freeStackAmount = inventory[i].itemType.maxStackAmount - inventory[i].itemAmount;

            if (amount <= freeStackAmount)
            {
                inventory[i].itemAmount += amount;
                leftOverItems = 0;
            }
            else if (freeStackAmount > 0)
            {
                inventory[i].itemAmount += amount - (amount - freeStackAmount);
                leftOverItems = amount - freeStackAmount;
            }
            else
            {
                Debug.Log($"Couldn't find free space for item:{item.itemName}!");
                Debug.Log(leftOverItems);
                return leftOverItems;
            }

            OnInventoryUpdate?.Invoke(inventory[i]);
            //Debug.Log(leftOverItems);
            return leftOverItems;
        }

        Debug.Log($"Couldn't find any inventory slot for item:{item.itemName}!");
        return leftOverItems;
    }

    public ItemSO debugItem;
    public int debugAmount;
    [Button]
    void DebugAddItem()
    {
        AddItem(debugItem, debugAmount);
    }

    public InventorySlot[] GetInventorySlots()
    {
        return inventory;
    }



    #region Unity lifecycle



    #endregion
}

[Serializable]
public class InventorySlot
{
    public ItemSO itemType;
    public int itemAmount;
}
