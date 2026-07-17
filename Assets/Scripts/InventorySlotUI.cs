using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI stackText;

    public void SetSlot(ItemData item, int amount)
    {
        itemIcon.sprite = item.Icon;
        itemIcon.gameObject.SetActive(true);
        stackText.text = amount.ToString();
        stackText.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
        stackText.text = "";
        stackText.gameObject.SetActive(false);
    }
}