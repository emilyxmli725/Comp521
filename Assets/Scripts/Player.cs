using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float sensitivity;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float jumpForce;

    [SerializeField] private float movementSpeed; 
    //turn up down 
    private float pitch = 0f;
    // turn left right 
    private float yaw = 0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraInput();
        
        var direction = Vector3.zero;

        if (Keyboard.current.wKey .isPressed)
        {
            direction += Vector3.forward ; 
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction +=   Vector3.left ;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            direction +=  Vector3.back ;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            direction +=  Vector3.right;
        }
        
        transform.position += Quaternion.Euler(0f,yaw,0f) * direction * (Time.deltaTime * movementSpeed);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce,  ForceMode.Impulse);
        }
    }

    private void CameraInput()
    {
        Vector2 mouseMovement = sensitivity * Mouse.current.delta.ReadValue();
        pitch -=  mouseMovement.y;
        yaw += mouseMovement.x;
        
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        cam.transform.rotation = Quaternion.Euler(pitch,yaw, 0f);
    }
}
