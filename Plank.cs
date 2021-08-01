using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{
    public float totalHealth = 100f;
    private float health = 100f;

    public Material damageMaterial;
    public Renderer plankRenderer;
    private bool didSwap = false;

    public int scoreValue = 100;

    public void Awake()
    {
        health = totalHealth;
        didSwap = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        health -= collision.relativeVelocity.magnitude;

        if(!didSwap && health < totalHealth / 2f)
        {
            SwapToDamaged();
        }

        if(health <= 0)
        {
            Destroy(gameObject);
            LevelTracker.AddScore(scoreValue);
        }
    }

    public void SwapToDamaged()
    {
        didSwap = true;

        if (plankRenderer == null) return;
        if (damageMaterial == null) return;

        plankRenderer.sharedMaterial = damageMaterial;
    }
}
