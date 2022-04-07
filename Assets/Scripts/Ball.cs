// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    public float maxVelocity = 3;

    public MainManager mainManager;

    public AudioSource playerAudio;
    public AudioClip brickSound;
    public AudioClip paddleSound;

    public ParticleSystem hitSmokePrefab;
    private ParticleSystem hitSmoke;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        hitSmoke = Instantiate(hitSmokePrefab, hitSmokePrefab.transform.position, hitSmokePrefab.transform.rotation);

    }
    
    // When we exit collision wit walls, bricks and paddle
    private void OnCollisionExit(Collision other) {

        // Make a sound when the ball exits the paddle
        if (other.gameObject.name.Equals("Paddle") == true) {

            if (mainManager == null) {
                mainManager = GetComponent<MainManager>();
            }
            mainManager.playerAudio.PlayOneShot(paddleSound,1f);
        }

        // Make a sound when the ball exits the paddle
        if (other.gameObject.name.StartsWith("Brick") == true) {

            if (mainManager == null) {
                mainManager = GetComponent<MainManager>();
            
            }
            hitSmoke.transform.position = other.gameObject.transform.position;
            hitSmoke.Play();
            mainManager.playerAudio.PlayOneShot(brickSound,1f);
        }

        var velocity = m_Rigidbody.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.3f) {
                velocity += velocity.y > 0 ? Vector3.up * 0.6f : Vector3.down * 0.6f;
        }

        //max velocity
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        m_Rigidbody.velocity = velocity;
    }
}
