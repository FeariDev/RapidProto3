using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    public float baseMoveSpeed;
    public float moveSpeed;

    Vector2 moveDirection;
    Vector3 moveValue;



    void UpdateMoveInput()
    {
        /*
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        */
        float x = Player.Instance.input.Player.Move.ReadValue<Vector2>().x;
        float y = Player.Instance.input.Player.Move.ReadValue<Vector2>().y;

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
