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
    public float weaponRange;
    public float weaponHInaccuracy;
    public float weaponVInaccuracy;
    private float weaponHInaccuracyDefault;
    private float weaponVInaccuracyDefault;


    public float fireRate;
    private float fireTimer = 0f;
    public int ammoCount;
    private int burstCount = 0;
    private LineRenderer lineRenderer;

    private Vector3[] points = new Vector3[2];
    void Start()
    {
        weaponHInaccuracyDefault = weaponHInaccuracy;
        weaponVInaccuracyDefault = weaponVInaccuracy;

        charController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lineRenderer = GetComponent<LineRenderer>();
    }

    
    
    void Update()
    {
        Vector3 forwardDir = transform.TransformDirection(Vector3.forward);
        Vector3 rightDir = transform.TransformDirection(Vector3.right);
        float curSpeedX = moveSpeed * Input.GetAxis("Vertical");
        float curSpeedY = moveSpeed * Input.GetAxis("Horizontal");
        dir = (forwardDir * curSpeedX) + (rightDir * curSpeedY);


        charController.Move(dir * Time.deltaTime);

        rotX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotX = Mathf.Clamp(rotX, -lookXLimit, lookXLimit);
        playerCam.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        if (Input.GetAxis("Fire1") == 1 && Time.time > fireTimer && ammoCount > 0)
        {
            lineRenderer.enabled = true;
            Fire();
        }

        else if (Input.GetAxis("Fire1") == 0)
        {
            lineRenderer.enabled = false;
            burstCount = 0;
            weaponHInaccuracy = weaponHInaccuracyDefault;
            weaponVInaccuracy = weaponVInaccuracyDefault;
        }
    }

    void Fire()
    {
        float burstCountInaccuracy;
        burstCount++;
        burstCountInaccuracy = burstCount;
        weaponHInaccuracy += burstCountInaccuracy / 500;
        weaponVInaccuracy += burstCountInaccuracy / 500;

        Debug.Log(burstCount);

        ammoCount--;
        fireTimer = Time.time + fireRate;
        RaycastHit hit;
        Vector3 newVectorForward;
        Vector3 newVectorOrigin;
        
        newVectorOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        points[0] = newVectorOrigin;
        newVectorForward = playerCam.transform.forward;
        newVectorForward.x = newVectorForward.x + Random.Range(-weaponHInaccuracy, weaponHInaccuracy) * weaponRange / 100;
        newVectorForward.y = newVectorForward.y + Random.Range(-weaponVInaccuracy, weaponVInaccuracy) * weaponRange / 100;
        newVectorForward.z = newVectorForward.z + Random.Range(-weaponHInaccuracy, weaponHInaccuracy) * weaponRange / 100;
        newVectorForward *= weaponRange;
        points[1] = newVectorForward;
        lineRenderer.SetPositions(points);
        Physics.Linecast(newVectorOrigin, newVectorForward, out hit);
        Debug.DrawRay(newVectorOrigin, newVectorForward, Color.red, 0.1f);
        //Debug.Log("Burst Bullet Count : "  + burstCount);
    }




}

