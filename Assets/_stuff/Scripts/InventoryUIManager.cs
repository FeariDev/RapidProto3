using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

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
        // ✅ Keyboard open/close
        if (Input.GetKeyDown(KeyCode.Tab))
            OpenInventory();

        if (Input.GetKeyUp(KeyCode.Tab))
            CloseInventory();

        // ✅ Controller Left Bumper open/close
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftShoulder.wasPressedThisFrame)
            {
                if (!isOpen)
                    OpenInventory();
                else
                    CloseInventory();
            }
        }
    }

    void OpenInventory()
    {
        if (inventoryPanel == null) return;

        inventoryPanel.SetActive(true);
        RefreshInventoryDisplay();
        isOpen = true;

        // Optional: pause game when inventory is open
        Time.timeScale = 0f;
    }

    void CloseInventory()
    {
        if (inventoryPanel == null) return;

        inventoryPanel.SetActive(false);
        isOpen = false;

        // Resume game when inventory closes
        Time.timeScale = 1f;
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