using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : Entity
{
    private Rigidbody rb = null;

    private Vector3 moveInput = Vector3.zero;

    private Vector3 moveVelocity = Vector3.zero;

    private Camera mainCamera;

    private Vector3 targetPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(cameraRay, out hit))
            {
                targetPos = hit.point;

                transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));

                moveVelocity = transform.forward;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            targetPos = transform.position;
        }
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, targetPos) > 1)
        {
            rb.MovePosition(transform.position + moveVelocity.normalized * currentMoveSpeed * Time.fixedDeltaTime);
        }
    }
}