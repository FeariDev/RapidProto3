using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] InventorySlot[] inventory;

    /// <summary>
    /// Gets called when an item in the inventory is added/removed.
    /// </summary>
    public event Action<InventorySlot> OnInventoryUpdate;



    void AddItem(ItemSO item, int amount)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].itemType != item) continue;

            inventory[i].itemAmount += amount;
            OnInventoryUpdate?.Invoke(inventory[i]);
            return;
        }
    }

    public ItemSO debugItem;
    public int debugAmount;
    [Button]
    void DebugAddItem()
    {
        AddItem(debugItem, debugAmount);
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
