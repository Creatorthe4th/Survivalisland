using UnityEngine;
using TMPro;

public class ResourceCounter : MonoBehaviour
{
    public TextMeshProUGUI appleText;
    public TextMeshProUGUI oreText;

    private int apples;
    private int ores;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (appleText != null)
        {
            appleText.text = $"Apples: {apples}";
        }

        if (oreText != null)
        {
            oreText.text = $"Ores: {ores}";
        }
    }

    // ---- Public API ----

    public void AddResource(string resourceName, int amount)
    {
        switch (resourceName)
        {
            case "Apple":
                AddApples(amount);
                break;
            case "Ore":
                AddOres(amount);
                break;
            default:
                Debug.LogWarning($"Unknown resource name '{resourceName}' — no matching Add method.");
                break;
        }
    }

    public void AddApples(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Use SpendApples to remove apples, not a negative AddApples.");
            return;
        }

        apples += amount;
        UpdateUI();
    }

    public void AddOres(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Use SpendOres to remove ores, not a negative AddOres.");
            return;
        }

        ores += amount;
        UpdateUI();
    }

    public bool SpendApples(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("SpendApples amount should be positive.");
            return false;
        }

        if (apples < amount)
            return false;

        apples -= amount;
        UpdateUI();
        return true;
    }

    public bool SpendOres(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("SpendOres amount should be positive.");
            return false;
        }

        if (ores < amount)
            return false;

        ores -= amount;
        UpdateUI();
        return true;
    }

    public int GetApples() => apples;
    public int GetOres() => ores;
}