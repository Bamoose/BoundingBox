using UnityEngine;

public class CameraControllerLockOn : MonoBehaviour
{
    public Transform lookTarget;
    public float distance = 10.0f;
    public float horizontalSpeed = 50f;
    public float verticalSpeed = 50f;
    public float zoomSpeed = 10f;
    public Vector3 posOffset;

    private float currentDistance;
    private float currentHorizontalAngle;
    private float currentVerticalAngle;

    private void Start()
    {
        currentDistance = distance;
        currentHorizontalAngle = transform.eulerAngles.y;
        currentVerticalAngle = transform.eulerAngles.x;
    }

    private void Update()
    {
        if (lookTarget == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {
            currentHorizontalAngle -= horizontalSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentHorizontalAngle += horizontalSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            currentVerticalAngle -= verticalSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentVerticalAngle += verticalSpeed * Time.deltaTime;
        }

        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -90.0f, 90.0f);

        currentDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0.0f);
        //Vector3 position = lookTarget.position - rotation * Vector3.forward * currentDistance;

        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(lookTarget.position);  // Convert target position to screen space
        Vector3 offset = Camera.main.ScreenToWorldPoint(new Vector3(targetScreenPos.x + posOffset.x, targetScreenPos.y + posOffset.y, targetScreenPos.z + posOffset.z)) - lookTarget.position;  // Calculate the offset vector in world space
        Vector3 position = lookTarget.position - rotation * Vector3.forward * currentDistance + offset;  // Add the offset vector to the camera position


        transform.rotation = rotation;
        transform.position = position;

        
    }
}
