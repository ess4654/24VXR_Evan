using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowSphere : MonoBehaviour
{
    [SerializeField] float speed = 1;

    Renderer rend;
    Color currentColor;
    Color targetColor;
    float t;
    bool isLerping;
    bool isMouseOver;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        ResetColor();
    }

    void Update()
    {
        if (isLerping)
        {
            rend.material.color = Color.Lerp(currentColor, targetColor, t);

            t += Time.deltaTime * speed;

            if (t >= 1)
            {
                if(!isMouseOver)
                {
                    isLerping = false;
                }
                else
                {
                    ResetColor();
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
        isLerping = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        currentColor = rend.material.color;
        targetColor = Color.white;
        t = 0;
    }

    private void ResetColor()
    {
        currentColor = rend.material.color;
        targetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        t = 0;
    }
}
