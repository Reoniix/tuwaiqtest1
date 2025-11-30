using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    public static ZombieAudio instance;

    public AudioSource walkingSource;  // Looping audio source

    void Awake()
    {
        //instance = this;

        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    //public static void PatrolSFX()
    //{
    //    if (instance != null && instance.walkingSource != null)
    //        instance.walkingSource.Stop();
    //}

    public void StopWalkingSFX()
    {
        if (walkingSource != null && walkingSource.isPlaying)
            walkingSource.Stop();
    }

    public void PlayWalkingSFX()
    {
        if (walkingSource != null && !walkingSource.isPlaying)
            walkingSource.Play();
    }

    // static convenience:
    public static void StopAllWalking() => instance?.StopWalkingSFX();
    public static void PlayAllWalking() => instance?.PlayWalkingSFX();
}
