using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] new Camera camera;
    [SerializeField] Transform followTarget;
    [SerializeField] float followSpeed;



    void FollowTarget()
    {
        Vector3 followPos = camera.transform.position;

        followPos.x = Mathf.Lerp(camera.transform.position.x, followTarget.position.x, followSpeed * Time.deltaTime);
        followPos.y = Mathf.Lerp(camera.transform.position.y, followTarget.position.y, followSpeed * Time.deltaTime);

        camera.transform.position = followPos;
    }



    #region Unity lifecycle

    void Start()
    {
        followTarget = transform;
    }

    void FixedUpdate()
    {
        FollowTarget();
    }

    #endregion
}
