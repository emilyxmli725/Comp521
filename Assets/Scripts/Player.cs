using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject bulletSpawn; 
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed; 
    [SerializeField] private float sensitivity;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSpeed;
    public float groundCheckDistance = 2.2f;
    [SerializeField] LayerMask groundLayer;
    public int ammo;
    public GameObject bullet; 
    private bool _isShooting;
    //turn up down 
    private float _pitch = 0f;
    // turn left right 
    private float _yaw = 0f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            ammo++;
        }
        
    }
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraInput();
        MovePlayer();
    }

    public void Clear()
    {
        ammo = 0;
    }
    private void MovePlayer()
    {
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
        if(Mouse.current.leftButton.wasPressedThisFrame && ammo > 0 && !bullet)
        {
            bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            bullet.GetComponent<Rigidbody>().linearVelocity = cam.transform.forward * bulletSpeed; 
            ammo--;
            
        }
        transform.position += Quaternion.Euler(0f,_yaw,0f) * direction * (Time.deltaTime * movementSpeed);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && GroundCheck())
        {
            rb.AddForce(Vector3.up * jumpForce,  ForceMode.Impulse);
        }
    }

    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer );
    }
    
    private void CameraInput()
    {
        Vector2 mouseMovement = sensitivity * Mouse.current.delta.ReadValue();
        _pitch -=  mouseMovement.y;
        _yaw += mouseMovement.x;
        
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        cam.transform.rotation = Quaternion.Euler(_pitch,_yaw, 0f);
    }
}
