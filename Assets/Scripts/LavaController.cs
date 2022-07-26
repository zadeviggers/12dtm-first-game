using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    // Constants
    private float lavaRiseWaitTime = 0.00001f;
    private float lavaRiseRate = 0.03f;
    public ParticleSystem particlesPrefab; // Set in unity inspector

    // Variables
    private ParticleSystem particles;

    // Called when gameobject is instantiated (on scene load in this case)
    void Start()
    {
        // Instantiate lava particles
        particles = Instantiate(particlesPrefab);

        // Make sure lava particles are in the same position as the top of the lava
        particles.transform.position = transform.position - new Vector3(0, transform.localScale.y / 2, 0);

        // Make the particles emitter shape the same width as the lava
        var shape = particles.shape;
        shape.scale = new Vector3(transform.localScale.x, shape.scale.y, shape.scale.z);

        // Start lava rising
        StartCoroutine(MakeLavalRiseRoutine());
    }

    // Looping coroutine that makes the lava rise
    IEnumerator MakeLavalRiseRoutine()
    {
        // Increase Y scale of lava
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + lavaRiseRate, transform.localScale.z);

        // Move particles up (need to halve lava rise rate because scaling the lava goes up and down so it seems like it's going up at half speed).
        particles.transform.position += new Vector3(0, lavaRiseRate / 2, 0);

        // Wait a little before increasing lava rise
        yield return new WaitForSeconds(lavaRiseWaitTime);

        // Loop
        StartCoroutine(MakeLavalRiseRoutine());
    }
}
