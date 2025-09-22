using UnityEngine;

public class CupTrigger : MonoBehaviour
{
    public int playerID;  // 1 for Player 1, 2 for Player 2
    public GameManager gameManager;
    public GameObject meshObject;
    public ParticleSystem explosionEffect;  // Reference to the tiny explosion effect
    public AudioClip destructionSound;  // The audio clip to play after the object is destroyed

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Add score
            gameManager.AddScore(playerID);

            // Reset the ball
            BallThrower ballThrower = other.GetComponent<BallThrower>();
            if (ballThrower != null)
            {
                ballThrower.ResetBall();
            }

            // Play particle effect
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosionEffect.Play();
            }

            // Play destruction sound
            if (destructionSound != null)
            {
                PlaySoundAfterDestruction(destructionSound, transform.position);
            }

            // Destroy the parent object
            Destroy(transform.parent.gameObject);

            // Disable mesh rendering
            if (meshObject != null)
            {
                MeshRenderer meshRenderer = meshObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }

            // Rearrange cups in the game
            gameManager.RearrangeCups();
        }
    }

    private void PlaySoundAfterDestruction(AudioClip clip, Vector3 position)
    {
        // Create a temporary GameObject for the audio
        GameObject tempAudioObject = new GameObject("TempAudio");
        tempAudioObject.transform.position = position;

        // Add an AudioSource component to the temporary GameObject
        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 2.0f; // 3D sound
        audioSource.Play();

        // Destroy the temporary audio object after the clip finishes playing
        Destroy(tempAudioObject, clip.length);
    }
}
