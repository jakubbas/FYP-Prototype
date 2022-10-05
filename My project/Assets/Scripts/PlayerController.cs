using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float lookSpeed;
    public Camera playerCam;
    public CharacterController charController;
    private Vector3 dir = Vector3.zero;
    private float rotX = 0f;
    public float lookXLimit = 45.0f;
    private Vector3 cameraCentre;
    public float weaponRange;
    public float weaponInaccuracy;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    

    void Update()
    {
        Vector3 forwardDir = transform.TransformDirection(Vector3.forward);
        Vector3 rightDir = transform.TransformDirection(Vector3.right);
        float curSpeedX = moveSpeed * Input.GetAxis("Vertical");
        float curSpeedY = moveSpeed * Input.GetAxis("Horizontal");
        float dirY = dir.y;
        dir = (forwardDir * curSpeedX) + (rightDir * curSpeedY);


        charController.Move(dir * Time.deltaTime);

        rotX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotX = Mathf.Clamp(rotX, -lookXLimit, lookXLimit);
        playerCam.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        cameraCentre = playerCam.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, playerCam.nearClipPlane));

        if (Input.GetAxis("Fire1") == 1)
        {
            Fire();
        }
    }
    
    void Fire()
    {
        RaycastHit hit;
        Physics.Linecast(playerCam.transform.position, cameraCentre * weaponRange, out hit);
        Debug.DrawLine(transform.position, Vector3.forward * weaponRange, Color.red, 5f);
        //Debug.DrawLine(playerCam.transform.position, cameraCentre * weaponRange, Color.red, 5f);
        Debug.Log(hit.collider.gameObject.name);
    }

}

