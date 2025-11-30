using UnityEngine;
using TMPro;
using System.Collections;

public class PickupUI : MonoBehaviour
{
    public TextMeshProUGUI pickupText;

    private void Start()
    {
        pickupText.gameObject.SetActive(false);  // ui starts turned off
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        pickupText.text = message;
        pickupText.gameObject.SetActive(true);  // when called

        yield return new WaitForSeconds(2f);    // show for 2secs

        pickupText.gameObject.SetActive(false); // then disappear
    }
}
