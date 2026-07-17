using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 20;

    private InventorySlotData[] slots;
    public InventorySlotUI[] slotUIs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slots = new InventorySlotData[inventorySize];
    }

    public void AddItem(ItemData item, int amount)
    {
        // First pass: try to stack onto existing slots with the same item
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            if (slots[i].item == item && slots[i].amount < item.maxStackSize)
            {
                int spaceLeft = item.maxStackSize - slots[i].amount;
                int amountToAdd = Mathf.Min(spaceLeft, amount);

                slots[i].amount += amountToAdd;
                amount -= amountToAdd;

                if (amount <= 0)
                {
                    UpdateUI();
                    return;
                }
            }
        }

        // Second pass: place any remainder into empty slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                continue;
            }

            int amountToAdd = Mathf.Min(item.maxStackSize, amount);
            slots[i] = new InventorySlotData(item, amountToAdd);
            amount -= amountToAdd;

            if (amount <= 0)
            {
                UpdateUI();
                return;
            }
        }

        if (amount > 0)
        {
            Debug.LogWarning($"Inventory full — could not add {amount}x {item.name}.");
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            if (slots[i] == null)
            {
                slotUIs[i].ClearSlot();
            }
            else
            {
                slotUIs[i].SetSlot(slots[i].item, slots[i].amount);
            }
        }
    }
}