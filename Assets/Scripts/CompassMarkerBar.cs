using TMPro;
using UnityEngine;

public class CompassMarkerBar : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public RectTransform markerContainer;
    public TMP_Text markerPrefab;

    [Header("Compass Settings")]
    [SerializeField] private float panelWidth = 500f;
    [SerializeField] private float pixelsPerDegree = 4f;

    [Header("Fade Settings")]
    [Tooltip("How many pixels from the edge markers start fading out")]
    [SerializeField] private float fadeZonePixels = 60f;

    [Header("Cardinal Highlight")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color northColor = Color.red;

    private readonly string[] labels =
    {
        "N", "NE", "E", "SE", "S", "SW", "W", "NW"
    };

    private TMP_Text[] markers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Pull the real width from the RectTransform so resizing the UI
        // doesn't desync the visibility/fade math.
        if (markerContainer != null && markerContainer.rect.width > 0f)
        {
            panelWidth = markerContainer.rect.width;
        }

        markers = new TMP_Text[labels.Length];

        for (int i = 0; i < labels.Length; i++)
        {
            TMP_Text marker = Instantiate(markerPrefab, markerContainer);
            marker.text = labels[i];
            marker.alignment = TextAlignmentOptions.Center;
            marker.color = (labels[i] == "N") ? northColor : defaultColor;
            marker.gameObject.SetActive(true);
            markers[i] = marker;
        }

        markerPrefab.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || markers == null)
        {
            return;
        }

        float heading = player.eulerAngles.y;

        for (int i = 0; i < markers.Length; i++)
        {
            float markerAngle = i * 45f;
            float delta = Mathf.DeltaAngle(heading, markerAngle);
            float x = delta * pixelsPerDegree;

            RectTransform rect = markers[i].rectTransform;
            rect.anchoredPosition = new Vector2(x, 0f);

            float halfWidth = panelWidth * 0.5f;
            bool visible = Mathf.Abs(x) <= halfWidth;
            markers[i].gameObject.SetActive(visible);

            if (visible)
            {
                // Fade markers out as they approach the edge of the panel
                // instead of a hard on/off cutoff.
                float distanceFromEdge = halfWidth - Mathf.Abs(x);
                float alpha = Mathf.Clamp01(distanceFromEdge / fadeZonePixels);

                Color c = markers[i].color;
                c.a = alpha;
                markers[i].color = c;
            }
        }
    }
}