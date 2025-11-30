using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    public int damagePerSecond = 5; //dmg persec
    public float damageInterval = 3f; //time between dmg taken
    private float timer;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                //added false so it wouldnt give knockback w water damage
                playerHealth.TakeDamage(damagePerSecond, transform.position, false);
                timer = 0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            timer = 0f;
        }
    }
}
