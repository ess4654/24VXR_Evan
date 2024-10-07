using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBasics : MonoBehaviour
{
    //VARIABLES

    // int default = 0
    // float default = 0.0
    // string default = null
    // bool default = false

    //OPERATORS

    // Arithmetic

    // Addition (+)
    private int length;
    // Subrtaction (-)
    private int cash;
    // Multiplication (*)
    private float speed = 1.0f;
    // Division (/)
    private float inverse;
    // Modulus (%)
    private int even;

    // Assignment

    // = assignment of value
    // += addition assignment
    private string Name;
    // -= subtraction assignment
    private float health;
    // *= multiplication assignment
    private float exponent = 1.0f;
    // /= division assignment
    private float logarimic = 100f;
    // %= modulus assignment
    private float loopBetween0and9;

    // Comparison

    // Equal to (==)
    // Not equal to (!=)
    // Greater than (>)
    // Less than (<)
    // Greater than or equal to (>=)
    // Less than or equal to (<=)

    //Logical Operators

    // AND &&
    // OR ||
    // NOT !

    // ++
    // --
    private float remainingTime;

    // Start is called before the first frame update
    void Start()
    {
        //Addition
        var x = length + 10;

        //Subtraction
        var y = cash - 6;

        //Multiplication
        var z = speed * 4.5f;

        //Division
        inverse = 1 / 10f;

        //Modulus
        even = 6;
        if(even % 2 == 0)
        {
            //This will trigger
            Debug.Log("Is Even");
        }
        else
        {
            Debug.Log("Is odd");
        }

        //Equals
        Name = "Evan";

        //Addition Assignment
        Name += " Svendsen!";

        //Subtraction Assignment
        health -= 60;

        //Dot operator
        transform.position = new Vector3(x, y, z);

        remainingTime = 1000; //1000 frames

        //Comparisons
        if(remainingTime == 0)
        {
            Debug.Log("Time has ended");
        }
        if (remainingTime != 0)
        {
            Debug.Log("We still have time");
        }

        //Greater Than
        if (health > 0)
        {
            Debug.Log("We are alive");
        }
        //Less Than OR Equal
        else if (health <= 0)
        {
            Debug.Log("We are dead");
        }

        //Less Than
        if (cash < 0)
        {
            Debug.Log("We are bankrupt");
        }
        //Greater Than OR Equal
        else if (cash >= 0)
        {
            Debug.Log("We have money");
        }


        //Logic

        bool canDance = false;
        bool youCanDance = true;

        //AND
        if (canDance && youCanDance)
        {
            Debug.Log("Both can dance");
        }

        //OR
        if (canDance || youCanDance)
        {
            Debug.Log("At least one can dance");
        }
        //NOT
        if (!canDance)
        {
            Debug.Log("I can't dance");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Multiplication assignment
        exponent *= 1.25f;

        //Division assignment
        logarimic /= 1.1f;

        //++
        loopBetween0and9++;
        //Modulus assignment
        loopBetween0and9 %= 10;

        //--
        remainingTime--;
    }
}
