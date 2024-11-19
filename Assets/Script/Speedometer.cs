using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody target;

    public float maxSpeed = 0.0f; // The max speed in km/h

    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;

    [Header("UI")]
    public TMP_Text speedLabel; // The arrow in the speedometer
    public RectTransform arrow;

    public float speed = 0.0f;

    private void Update()
    {
        // 3.6f to convert to kilometers, clamped to a max of 260 km/h
        speed = Mathf.Min(target.velocity.magnitude * 3.6f, 260f);

        if (speedLabel != null)
            speedLabel.text = ((int)speed) + " km/h";

        if (arrow != null)
            arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
    }
}
