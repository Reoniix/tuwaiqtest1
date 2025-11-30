using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public static PlayerAudio instance;

    public AudioSource walkSource;
    public AudioSource jumpSource;
    public AudioClip jumpSFX;

    void Awake()
    {
        instance = this;
    }

    public void PlayJump()
    {
        if (jumpSource != null && jumpSFX != null)
            jumpSource.PlayOneShot(jumpSFX);
    }

    public void PlayWalk()
    {
        if (walkSource != null && !walkSource.isPlaying)
            walkSource.Play();
    }

    public void StopWalk()
    {
        if (walkSource != null && walkSource.isPlaying)
            walkSource.Stop();
    }
}
