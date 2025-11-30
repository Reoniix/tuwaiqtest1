using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthBar;

    public GameObject gameOverScreen;

    [Header("Damage Flash")] //dmg
    public Image damageFlash; //dmg
    public float flashDuration = 0.1f; //dmg

    [Header("Knockback")] // knockback
    public Rigidbody rb; // knockback, assign in Inspector
    public float knockbackForce = 5f; // knockback, adjust in Inspector

    [Header("Audio")] // sfx
    public AudioSource audioSource; // sfx for hit n death sound
    public AudioClip DamageSFX; // sfx
    public AudioClip deathSFX; // sfx
    public AudioClip gameOverSFX;

    [Header("Background Music")]
    public AudioSource bgMusic;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        gameOverScreen.SetActive(false);

        damageFlash.color = new Color(1, 0, 0, 0); // dmg transparent red
    }

    //TESTING if damage works VV
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        TakeDamage(10, transform.position);
    //    }
    //}

    //public void TakeDamage(int damage, Vector3 sourcePos)  //vector3 added for knockback
    public void TakeDamage(int damage, Vector3 sourcePos, bool applyKnockback = true)
    {
        if (currentHealth <= 0)
        {
            return; //dmg
        }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthUI();
        StartCoroutine(FlashEffect()); //dmg
        //ApplyKnockback(sourcePos); // knockback to all types of damage

        if (applyKnockback)
            ApplyKnockback(sourcePos);    //knockback isnt allowed on water damage

        // play hit sound
        if (audioSource != null && DamageSFX != null)
        {
            audioSource.PlayOneShot(DamageSFX);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //BROKEN PHYSICS, the zombie overlaps w the player n breaks the knockback code
    //void ApplyKnockback(Vector3 sourcePos)
    //{
    //    if (rb == null) return; // kb safety

    //    Vector3 knockDir = (transform.position - sourcePos).normalized; // kb direction
    //    rb.AddForce(knockDir * knockbackForce, ForceMode.Impulse); // kb apply
    //}

    void ApplyKnockback(Vector3 sourcePos)
    {
        if (rb == null)
        {
            return;
        }

        Vector3 direction = transform.position - sourcePos;

        // If zombie is overlapping player, direction becomes near-zero
        if (direction.magnitude < 0.1f)
        {
            direction = transform.forward; // fallback direction
        }

        direction.y = 0; // prevent launching player into space

        rb.AddForce(direction.normalized * knockbackForce, ForceMode.Impulse);
    }


    IEnumerator FlashEffect()
    {
        // Show flash
        damageFlash.color = new Color(1, 0, 0, 0.6f);

        yield return new WaitForSeconds(flashDuration);

        // Fade out
        damageFlash.color = new Color(1, 0, 0, 0);
    }

    void UpdateHealthUI()
    {
        healthBar.value = currentHealth;
        Debug.Log($"player health: {currentHealth} from playerHealth script");
    }

    public void Die()
    {
        Debug.Log("Player died");

        GetComponent<PlayerControl>().canMove = false;
        PlayerAudio.instance.StopWalk(); // walksfx stops immediately

        CoinInteraction.ResetWinProgress();

        //zombie patrol sfx
        ZombieAudio.StopAllWalking();

        // Play death SFX
        if (audioSource != null && deathSFX != null)
            audioSource.PlayOneShot(deathSFX);

        // Play game over sfx
        if (audioSource != null && gameOverSFX != null)
            audioSource.PlayOneShot(gameOverSFX);

        gameOverScreen.SetActive(true);

        // Show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; // freezes the game

        if (bgMusic != null)
        {
            bgMusic.Stop();
        }

    }

    //for RESTART button function
    public void RestartGame()
    {
        Debug.Log("Restart button pressed!");

        CoinInteraction.ResetWinProgress();


        Time.timeScale = 1f; // unfreeze game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload current scene
    }

    //if the player falls under ground this makes the gameover screen show up
    //nope didnt work
}
