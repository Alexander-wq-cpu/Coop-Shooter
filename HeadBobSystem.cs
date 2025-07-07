using System;
using UnityEngine;

public class HeadBobSystem : MonoBehaviour
{
    [Range(0.001f,0.01f)]
    public float Amount = 0.002f;

    [Range(1f,30f)]
    public float Frequency = 10.0f;

    [Range(10f,100f)]
    public float Smooth = 10.0f;

    [Range(1f,5f)]
    public float StopHeadBobReturnSpeed = 1;

    Vector3 StartPos;
    void Start()
    {
        StartPos = transform.localPosition;
    }

    void Update()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            StartHeadBob();
        }
        else
        {
            StopHeadBob();
        }
        /*старый способ
         * CheckForHeadBobTrigger();
        StopHeadBob();*/
    }

    private void CheckForHeadBobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;
        if(inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;

        // Вычисляем целевые смещения по Y и X на основе синуса и косинуса
        float targetY = Mathf.Sin(Time.time * Frequency) * Amount * 1.4f;
        float targetX = Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f;

        // Плавно интерполируем pos.y и pos.x к целевым значениям
        pos.y = Mathf.Lerp(pos.y, targetY, Smooth * Time.deltaTime);
        pos.x = Mathf.Lerp(pos.x, targetX, Smooth * Time.deltaTime);

        // Устанавливаем позицию относительно предыдущей
        transform.localPosition += pos;

        /*pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;*/

        return pos;
    }

    private void StopHeadBob()
    {
        if (transform.localPosition == StartPos) return;

        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPos, StopHeadBobReturnSpeed * Time.deltaTime);
    }
}
