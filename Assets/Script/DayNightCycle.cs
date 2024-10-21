using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private Game_Manager gameManager;
    private ClockSpriteChanger clockSpriteChanger;

    [SerializeField] private float variationTime = 150f;
    private float nextToggleTime;
    public bool daylight { get; private set; } = false;
    private float currentRotationAngle;

    private void Start()
    {
        gameManager = FindAnyObjectByType<Game_Manager>();
        clockSpriteChanger = FindAnyObjectByType<ClockSpriteChanger>();
    }

    void Update()
    {
        float eachAngle = 180f / variationTime * Time.deltaTime;

        currentRotationAngle += eachAngle; 

        transform.rotation = Quaternion.Euler(currentRotationAngle, 0, 0);

        if (currentRotationAngle >= 360)
        {
            currentRotationAngle = 0;
        }
        clockSpriteChanger.UpdateClockSprite(currentRotationAngle);

        if (gameManager.globalTimer >= nextToggleTime)
        {
            daylight = !daylight;
            Debug.Log(daylight);

            nextToggleTime += variationTime;
        }
    }
}
