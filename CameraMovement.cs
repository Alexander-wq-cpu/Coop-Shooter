using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;

    float xRotation;
    float yRotation;
    //углы равны 89 градусов потому что при 90 когда камера смотрит вниз ,невозможно передвижение вперед и назад
    float minCameraAngle = -89f;
    float maxCamerAngle = 89f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

        xRotation -= mouseY;
        //ограничиваем движение камеры вверх и вниз
        xRotation = Mathf.Clamp(xRotation, minCameraAngle, maxCamerAngle);
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
