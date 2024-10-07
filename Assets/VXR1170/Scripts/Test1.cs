using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * These lines are using directives in C#, specifying which namespaces the script will have access to.
 * make's it easier to use various built-in collections and Unity-specific classes in your script
 * without needing to specify their full paths every time you reference them.
 */

public class Test1 : MonoBehaviour

/*
 * 
 * The line of code public class Test1 : MonoBehaviour 
 * in Unity defines a new class called Test1 that inherits from the MonoBehaviour class.
 *
 * public: This keyword makes the class accessible to other scripts. 
 * Other scripts can interact with this class and access its public members.
 */


{
    // variables declared inside a class but outside a method is known as fields or member variables
    // these variables are accessible to all methods within the class and sometimes other classes depending on their access modifier (public, or private)

    private int score = 0;


    // awake is called when the script component is loaded, CALLED BEFORE START
    // useful for setting up references that are needed before the initialization of the script
    // awake is often marked as private (or left without an access modifier which automatically defaults to private)
    // it's meant for internal intialization within the class

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        // local variable only accessible within the Start method

        int health = 100;
        
        // Debug is a class provided by Unity that contains static methods for debugging
        // helps developers track and understand what is happening in their code while the game is running 

        // Log() is a method of Debug class that outputs a message to the Console
        // Primarily used to display information, track variable values, and detect when certain parts of the code are executing

        // "Health:" is a string of characters
        // + is an operator used to concatenate (join) the string and the int value of health 

        Debug.Log("Health:" + health);

      

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // when a method is declared with void, it means it performs an action but does not send any value back to whatever called it

}
