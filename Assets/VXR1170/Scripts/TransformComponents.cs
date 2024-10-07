using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformComponents : MonoBehaviour
{
    public Transform target;

    private void Update()
    {

        MoveCube();
        RotateCube();
        ScaleCube();
    
        if(Input.GetKeyDown(KeyCode.L))
        {
            LookAtTarget();
        }
    }

    private void MoveCube()
    {
        var moveSpeed = 5f;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += moveSpeed * Time.deltaTime * Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += moveSpeed * Time.deltaTime * Vector3.back;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += moveSpeed * Time.deltaTime * Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += moveSpeed * Time.deltaTime * Vector3.right;
        }
    }

    private void RotateCube()
    {
        float rotationSpeed = 50f;
        if(Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void ScaleCube()
    {
        var scaleChange = Vector3.one * Time.deltaTime;
        if(Input.GetKey(KeyCode.W))
        {
            transform.localScale += scaleChange;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.localScale -= scaleChange;
        }
    }

    private void LookAtTarget()
    {
        if(target != null)
        {
            transform.LookAt(target);
        }
        else
        {
            Debug.Log("No target to look at.");
        }
    }
}
