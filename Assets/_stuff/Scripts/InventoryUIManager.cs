using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    [Header("References")]
    public GameObject inventoryPanel;
    public Transform itemContainer;
    public GameObject itemUIPrefab;

    [Header("Settings")]
    public PlayerInventory playerInventory;

    private bool isOpen = false;
    private Dictionary<ItemSO, GameObject> itemUIInstances = new Dictionary<ItemSO, GameObject>();

    void Start()
    {
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (playerInventory != null)
            playerInventory.OnInventoryUpdate += OnInventoryUpdate;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            OpenInventory();

        if (Input.GetKeyUp(KeyCode.Tab))
            CloseInventory();
    }

    void OpenInventory()
    {
        if (inventoryPanel == null) return;

        inventoryPanel.SetActive(true);
        RefreshInventoryDisplay();
        isOpen = true;
    }

    void CloseInventory()
    {
        if (inventoryPanel == null) return;

        inventoryPanel.SetActive(false);
        isOpen = false;
    }

    void OnInventoryUpdate(InventorySlot slot)
    {
        if (isOpen)
            RefreshInventoryDisplay();
    }

    void RefreshInventoryDisplay()
    {
        foreach (Transform child in itemContainer)
            Destroy(child.gameObject);

        foreach (InventorySlot slot in playerInventory.GetInventorySlots())
        {
            if (slot.itemType == null || slot.itemAmount <= 0) continue;

            GameObject ui = Instantiate(itemUIPrefab, itemContainer);
            Image icon = ui.GetComponentInChildren<Image>();
            TMP_Text amountText = ui.GetComponentInChildren<TMP_Text>();

            if (icon != null && slot.itemType.sprite != null)
                icon.sprite = slot.itemType.sprite;

            
            amountText.text = $"x{slot.itemAmount}";

            itemUIInstances[slot.itemType] = ui;
        }

    }
}
