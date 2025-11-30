using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;

public class CoinInteraction : MonoBehaviour
{

    // List of items that count toward winning
    private static string[] winItems = { "Bandages", "Ammo Case", "Health Kit", "Pill Bottle" };
    private static int collectedCount = 0;

    public GameObject winCanvas; // assign my win canvas here
    public AudioClip pickupSFX;
    private AudioSource audioSource;

    //win sfx
    public AudioSource winAudio; 
    public AudioClip winSFX;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (winCanvas != null)
            winCanvas.SetActive(false); // hide win screen at start
    }


    //code for when a chara walks through an item and that item makes a SFX with UI msg using item name in hierarchy THEN destroys it self + ill add a debug log "picked up coin"

    //first off make sure the object has [obj> collider component> Is Trigger=ON], and that the SFX in [Audio Source component]:vv
    //1- Audio Resource has the sfx i want
    //2- Play On Awake IS OFF
    //3- Spatial Blend is 3D
    //THEN ADD THIS SCRIPT


    //private AudioSource audioSource
    //private void Start()
    //{
    //    audioSource = GetComponent<AudioSource>();
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        AudioSource soundCoins = GetComponent<AudioSource>();

    //        soundCoins.Play();
    //        Debug.Log("sound play");

    //        PickupUI ui = FindObjectOfType<PickupUI>();         // calls UI script
    //        ui.ShowMessage($"Picked up {gameObject.name}!");    // UI msg w the object name in hierarchy (will cause issues if theres duplicats of the same object)
    //                                                            // e.g.: coin1, coin2, ...etc
    //        Destroy(gameObject, 0.3f);


    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play pickup SFX
            if (audioSource != null)
                audioSource.PlayOneShot(pickupSFX);

            // Show UI message
            PickupUI ui = FindObjectOfType<PickupUI>();
            if (ui != null)
                ui.ShowMessage($"Picked up {gameObject.name}!");

            // Check if this is one of the win items
            foreach (string itemName in winItems)
            {
                if (gameObject.name == itemName)
                {
                    collectedCount++;
                    Debug.Log($"Picked up win item: {gameObject.name} ({collectedCount}/{winItems.Length})");

                    // Check for win
                    if (collectedCount >= winItems.Length)
                    {
                        WinGame();
                        ZombieAudio.StopAllWalking();

                    }
                    break;
                }
            }

            // Destroy the item after short delay so SFX plays
            Destroy(gameObject, 0.3f);
        }
    }

    void WinGame()
    {
        Debug.Log("Player won!");

        FindObjectOfType<PlayerControl>().canMove = false;
        PlayerAudio.instance.StopWalk(); // walksfx stops immediately

        // Show Win Canvas
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
            Debug.Log("winAudio enabled? " + winAudio.enabled);
            Debug.Log("winAudio gameObject active? " + winAudio.gameObject.activeInHierarchy);
            Debug.Log("winAudio clip null? " + (winSFX == null));

        }

        //win sfx even when game time is frozen -i think-
        if (winAudio != null && winSFX != null)
        {
            winAudio.ignoreListenerPause = true;
            winAudio.playOnAwake = false;
            winAudio.Stop();
            winAudio.PlayOneShot(winSFX, 1f);

        }

        // Freeze game
        Time.timeScale = 0f;

        // Optional: show cursor if needed
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public static void ResetWinProgress()
    {
        collectedCount = 0;
    }


}

