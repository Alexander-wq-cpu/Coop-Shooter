using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

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

        //ƒвигаю персонажа по ос€м x и z
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        /*ќбнул€€ компоненту Y у векторов направлени€ камеры до сложени€, вы избегаете вли€ни€ наклона камеры на движение.
         Ќормализаци€ гарантирует, что длина векторов forward и right равна 1, что корректно масштабирует движение.
         ¬ результате движение персонажа всегда происходит по горизонтали, и он не "застревает" при взгл€де вниз.*/
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = Camera.main.transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 move = right * x + forward * z;
        controller.Move(move.normalized * speed * Time.deltaTime);
        /*старый способ
         * Vector3 move = Camera.main.transform.right * x + Camera.main.transform.forward * z;
        move.y = 0f;*/

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