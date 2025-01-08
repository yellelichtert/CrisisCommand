using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float zoomSpeed;

    [SerializeField] private Camera mainCamera;
    
    void Update()
    {
        //Move the camera
        transform.Translate(Vector3.right * (Input.GetAxis("Horizontal") * (moveSpeed * Time.deltaTime))); 
        transform.Translate(Vector3.forward * (Input.GetAxis("Vertical") * (moveSpeed * Time.deltaTime))); 
        
        //zoom in and out
        transform.Translate(Vector3.up * (Input.GetAxis("Mouse ScrollWheel") * (zoomSpeed * Time.deltaTime)));

        //Rotate the camera when mouse is clicked
        if (Input.GetMouseButton(0))
        {
            float pitch = Input.GetAxis ("Mouse Y") * -1f * rotateSpeed;
            mainCamera.transform.Rotate (pitch * Vector3.right, Space.Self); 

            float yaw = Input.GetAxis ("Mouse X") * rotateSpeed;
            transform.Rotate (yaw * Vector3.up, Space.World);
        }
    }
}