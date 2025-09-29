using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float moveSpeed;

    Vector2 moveDirection;
    Vector3 moveValue;



    void UpdateMoveInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(x, y).normalized;
        moveValue = new Vector3(moveDirection.x, moveDirection.y, 0) * moveSpeed;
    }
    void UpdateMovement()
    {
        rb.linearVelocity = moveValue;
    }



    #region Unity lifecycle

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateMoveInput();
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    #endregion
}
