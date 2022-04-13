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

        // Start lava rising
        StartCoroutine(MakeLavalRiseRoutine());
    }

    // Looping coroutine that makes the lava rise
    IEnumerator MakeLavalRiseRoutine() {
        // Wait a little before increasing lava rise
        yield return new WaitForSeconds(lavaRiseWaitTime);

        // Increase Y scale
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y + lavaRiseRate, gameObject.transform.localScale.z);

        // Move particles up (need to halve lava rise rate because scaling the lava goes up and down so it seems like it's going up at half speed).
        particles.gameObject.transform.position += new Vector3(0, lavaRiseRate / 2, 0);

        // Loop
        StartCoroutine(MakeLavalRiseRoutine());
    }
}
