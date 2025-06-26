using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private float speed;
    float gravity = -9.81f * 2;
    [SerializeField] private float jumpHeight = 3f;
    float groundSphereRadius = 0.4f;

    private Vector3 verticalVelocity;
    public bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundSphereRadius, groundMask);

        //устанавливаю дефолтное вертикальное движение чтобы она не уменьшалась бесконечно
        if(isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        //Двигаю персонажа по осям x и z
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move =  Camera.main.transform.right * x + Camera.main.transform.forward * z;
        controller.Move(move.normalized * speed * Time.deltaTime);

        //прикладываю к персонажу силу гравитации
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

}
