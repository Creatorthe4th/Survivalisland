using UnityEngine;
using UnityEngine.InputSystem;
public class InventoryUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject InventoryPanel;
    private bool IsOpen;
    void Start()
    {
        if (InventoryPanel != null)
        {
            InventoryPanel.SetActive(false);
        }
    }
    public void OnInventory(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }
        ToggleInventory();
    }
    
    private void ToggleInventory()
    {
        IsOpen = !IsOpen;
        if (InventoryPanel != null)
        {
            InventoryPanel.SetActive(IsOpen);
        }
    }
}
