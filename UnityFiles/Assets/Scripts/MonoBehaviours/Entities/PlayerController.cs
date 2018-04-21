using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Entity
{
    [Header("Camera Settings")]
    [SerializeField]
    [Range(0.1f, 5)]
    private float mouseSensitivity = 2.0f;

    [SerializeField]
    private float pushPower;

    private Camera myCamera = null;

    private CharacterController controller;

    private float xAxisClamp = 0;

    private void Awake()
    {
        myCamera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        MovePlayer();
        RotateCamera();
        Fire();
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotCam = myCamera.transform.eulerAngles;
        Vector2 targetRotBody = transform.rotation.eulerAngles;

        targetRotCam.x -= rotAmountY;

        // Clamps the y rotation of the camera
        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotCam.x = 270;
        }

        targetRotCam.z = 0f;

        targetRotBody.y += rotAmountX;

        myCamera.transform.rotation = Quaternion.Euler(targetRotCam);
        transform.rotation = Quaternion.Euler(targetRotBody);
    }

    // Handles input to move the player forward and sideways
    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveSideWays = transform.right * horizontal * currentMoveSpeed;
        Vector3 moveForward = transform.forward * vertical * currentMoveSpeed;

        // Keep forward and sideways movement separate, Doesn't work to add both vectors together before using simple move
        controller.SimpleMove(moveSideWays);
        controller.SimpleMove(moveForward);
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = myCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (!body || body.isKinematic)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }
}