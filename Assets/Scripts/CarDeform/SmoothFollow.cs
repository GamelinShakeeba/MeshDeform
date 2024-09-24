using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 6.0f;
    public float height = 2.0f;
    public float heightOffset = 0.0f;
    public float heightDamping = 4.0f;
    public float rotationDamping = 2.0f;
    public LayerMask obstructionLayer; // Layer mask for objects that can obstruct the view

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the wanted position and rotation of the camera
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Calculate the desired position of the camera
        Vector3 desiredPosition = target.position - currentRotation * Vector3.forward * distance;
        desiredPosition.y = currentHeight + heightOffset;

        // Check for obstructions
        RaycastHit hit;
        if (Physics.Linecast(target.position, desiredPosition, out hit, obstructionLayer))
        {
            // Set the position of the camera to be in front of the obstruction
            transform.position = hit.point;
        }
        else
        {
            // Set the position of the camera to the desired position
            transform.position = desiredPosition;
        }

        transform.LookAt(target);
    }
}
