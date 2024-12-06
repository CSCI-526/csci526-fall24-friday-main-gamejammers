using UnityEngine;
using UnityEngine.UI;

public class ScreenFlickerController : MonoBehaviour
{
    [Header("Flicker Settings")]
    public Image brightnessOverlay;
    public float maxAlpha = 0.3f;
    public float flickerSpeed = 3f;

    [Header("Wall Proximity Settings")]
    public Transform player;
    public GameObject[] walls;
    public float dangerDistance = 35f;
    public float loseDistanceThreshold = 0.5f;
    public Canvas cv;

    private bool flickeringEnabled = false;
    private Color originalColor;

    void Start()
    {
        originalColor = new Color(1f, 0f, 0f, maxAlpha);
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        if (walls == null || walls.Length == 0)
        {
            Debug.LogWarning("No walls assigned.");
            return;
        }
    }

    void Update()
    {
        HandleFlickerEffect(); // Handles flickering visuals
        HandleWallProximity(); // Handles danger checks and lose conditions
    }

    private void HandleFlickerEffect()
    {
        if (!flickeringEnabled)
        {
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            return;
        }

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed)) * maxAlpha;
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }

    private void HandleWallProximity()
    {
        if (player == null || walls == null) return;

        bool playerInDanger = false;

        for (int i = 0; i < walls.Length; i++)
        {
            GameObject wall = walls[i];
            if (wall == null) continue;

            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer == null) continue;

            bool isWithinWallBounds = IsPlayerWithinWallBounds(player.position, wallRenderer);
            float perpendicularDistance = Mathf.Abs(player.position.z - wall.transform.position.z);
            //Debug.Log("Perpendicular distance: " + perpendicularDistance);

            if (isWithinWallBounds && perpendicularDistance <= dangerDistance)
            {
                playerInDanger = true;
            }

            if (isWithinWallBounds && perpendicularDistance <= loseDistanceThreshold)
            {
                TriggerLose(i);
            }
        }

        if (!playerInDanger && flickeringEnabled)
        {
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
    }

    private bool IsPlayerWithinWallBounds(Vector3 playerPosition, Renderer wallRenderer)
    {
        Bounds wallBounds = wallRenderer.bounds;

        float extendedMinY = wallBounds.min.y - 10f;
        float extendedMaxY = wallBounds.max.y + 10f;

        bool withinX = playerPosition.x >= wallBounds.min.x && playerPosition.x <= wallBounds.max.x;
        bool withinY = playerPosition.y >= extendedMinY && playerPosition.y <= extendedMaxY;

        return withinX && withinY;
    }

    private void TriggerLose(int wallIndex)
    {
        Debug.Log($"Player touched wall {wallIndex}.");
        StopFlickering();

        Debug.Log("Game Over!");
        cv.PlayerLose(1); // Game-over logic
    }

    public void TriggerFirstGrapple()
    {
        Debug.Log("First grapple enabled.");
        flickeringEnabled = true;
    }

    public void StopFlickering()
    {
        Debug.Log("Stop called.");
        flickeringEnabled = false;
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
