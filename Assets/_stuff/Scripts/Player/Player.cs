using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCamera))]
public class Player : MonoBehaviour
{
    PlayerMovement movement;
    new PlayerCamera camera;



    #region Unity lifecycle

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        camera = GetComponent<PlayerCamera>();
    }

    #endregion
}
