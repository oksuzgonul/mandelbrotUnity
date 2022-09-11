using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class controller : MonoBehaviour
{
    public Material mat;
    public Vector2 pos;
    public float scale, angle;


    Vector2 movement;
    float rotation;
    bool IsMoving;
    bool IsRotating;

    private Vector2 smoothPos;
    private float smoothScale, smoothAngle;

    private void UpdateShader()
    {
        smoothPos = Vector2.Lerp(smoothPos, pos, .03f);
        smoothScale = Mathf.Lerp(smoothScale, scale, .03f);
        smoothAngle = Mathf.Lerp(smoothAngle, angle, .03f);

        float aspect = (float)Screen.width / (float)Screen.height;
        float scaleX = smoothScale;
        float scaleY = smoothScale;


        if (aspect > 1f)
        {
            scaleY /= aspect;
        }
        else
        {
            scaleX *= aspect;
        }

        mat.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
        mat.SetFloat("_Angle", smoothAngle);
    }

    public void ScrollZoom(InputAction.CallbackContext context)
    {
       float z = context.ReadValue<float>();
       if (z > 0)
       {
           scale *= 1.03f;
       }
       else if (z < 0)
       {
           scale *= .97f;
       }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement = context.ReadValue<Vector2>();
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
            
    }
    public void OnRotate(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            rotation = context.ReadValue<float>();
            IsRotating = true;
        }
        else
        {
            IsRotating = false;
        }
    }

    void Rotate()
    {
        if (IsRotating)
        {
            angle += rotation * .01f;
        }
    }

    void Move()
    {
        if (IsMoving)
        {
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);
            Vector2 dir = movement * .01f * scale;
            dir = new Vector2(dir.x*c - dir.y*s, dir.x*s + dir.y*c);

            pos += dir;
        }
    }

    void Start()
    {
        scale = 1f;
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
        UpdateShader();
    }
}