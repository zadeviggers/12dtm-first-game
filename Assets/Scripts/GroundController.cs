using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public ParticleSystem particlesPrefab;
    private ParticleSystem particles;

    private float lavaRiseWaitTime = 0.00001f;
    private float lavaRiseRate = 0.01f;

    void Start()
    {
        StartCoroutine(MakeLavalRiseRoutine());
        particles = Instantiate(particlesPrefab);
    }

    void Update()
    {
        
    }


    IEnumerator MakeLavalRiseRoutine() {
        yield return new WaitForSeconds(lavaRiseWaitTime);
        // Increase Y scale
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y + lavaRiseRate, gameObject.transform.localScale.z);

        // Move particles up (need to halve lava rise rate because scaling the lava goes up and down so it seems like it's going up at half speed).
        particles.gameObject.transform.position += new Vector3(0, lavaRiseRate / 2, 0);

        // Loop
        StartCoroutine(MakeLavalRiseRoutine());
    }
}
