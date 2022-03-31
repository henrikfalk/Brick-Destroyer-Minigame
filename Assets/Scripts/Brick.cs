using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    
    public int PointValue;

    public Material brick1Material;
    public Material brick2Material;
    public Material brick3Material;
    public Material brick4Material;
    public Material brick5Material;
    public Material brick6Material;
    public Material brick7Material;
    public Material brick8Material;
    public Material brick9Material;

    void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        switch (PointValue) {
            case 1 :
                renderer.material = brick1Material;
                break;
            case 2:
                renderer.material = brick2Material;
                break;
            case 3:
                renderer.material = brick3Material;
                break;
            case 4:
                renderer.material = brick4Material;
                break;
            case 5:
                renderer.material = brick5Material;
                break;
            case 6:
                renderer.material = brick6Material;
                break;
            case 7:
                renderer.material = brick7Material;
                break;
            case 8:
                renderer.material = brick8Material;
                break;
            case 9:
                renderer.material = brick9Material;
                break;
            default:
                renderer.material = brick1Material;
                break;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        onDestroyed.Invoke(PointValue);
        
        //slight delay to be sure the ball have time to bounce
        Destroy(gameObject, 0.2f);
    }
}
