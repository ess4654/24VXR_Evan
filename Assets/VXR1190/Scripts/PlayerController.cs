using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;

    CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        var inputAxis = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
            inputAxis.x++;
        if (Input.GetKey(KeyCode.A))
            inputAxis.x--;

        if (Input.GetKey(KeyCode.W))
            inputAxis.y++;
        if (Input.GetKey(KeyCode.S))
            inputAxis.y--;

        characterController.Move(speed * Time.deltaTime * new Vector3(inputAxis.x, 0, inputAxis.y));
    }
}
