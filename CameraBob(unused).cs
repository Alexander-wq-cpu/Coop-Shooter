using System;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    public PlayerMovement mover;
    [Header("Bobbing")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.25f;
    public Vector3 bobLimit = Vector3.one * 0.01f;

    Vector3 bobPosition;

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    Vector2 walkInput;

    float smooth = 10f;
    float smoothRot = 12f;

    Vector3 swayPos;
    Vector3 swayEulerRot;

    private void Update()
    {
        walkInput.x = Input.GetAxis("Horizontal");
        walkInput.y = Input.GetAxis("Vertical");
        walkInput = walkInput.normalized;
        BobOffset();
        BobRotation();

        CompositePositionRotation();
    }


    void BobOffset()
    {
        speedCurve += Time.deltaTime * (mover.isGrounded ? mover.controller.velocity.magnitude : 1f) + 0.01f;

        bobPosition.x = (curveCos * bobLimit.x * (mover.isGrounded ? 1:0)) - (walkInput.x + travelLimit.x);

        bobPosition.y = (curveSin * bobLimit.y) - (mover.controller.velocity.y * travelLimit.y);

        bobPosition.z = -(walkInput.y * travelLimit.z);
    }

    void BobRotation()
    {
        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
    }

    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);

        transform.localRotation = Quaternion.Slerp(transform.localRotation,
            Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation),
            Time.deltaTime * smoothRot);
    }
}
