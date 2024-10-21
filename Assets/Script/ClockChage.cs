using UnityEngine;
using UnityEngine.UI;

public class ClockSpriteChanger : MonoBehaviour
{
    private Image clockImage; 
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite middaySprite;
    [SerializeField] private Sprite afternoonSprite;
    [SerializeField] private Sprite DuskSprite;
    [SerializeField] private Sprite nightSprite;
    public Sprite latenightSprite;

    void Start()
    {
        clockImage = GetComponent<Image>();
    }

    public void UpdateClockSprite(float CurrentDayAngle)
    {
        if (CurrentDayAngle >= 0 && CurrentDayAngle < 60)
        {
            clockImage.sprite = morningSprite;
        }
        else if (CurrentDayAngle >= 60 && CurrentDayAngle < 120)
        {
            clockImage.sprite = middaySprite;
        }
        else if (CurrentDayAngle >= 120 && CurrentDayAngle < 180)
        {
            clockImage.sprite = afternoonSprite;
        }
        else if (CurrentDayAngle >= 180 && CurrentDayAngle < 240)
        {
            clockImage.sprite = DuskSprite;
        }
        else if (CurrentDayAngle >= 240 && CurrentDayAngle < 300)
        {
            clockImage.sprite = nightSprite;
        }
        else
        {
            clockImage.sprite = latenightSprite;
        }
    }
}

