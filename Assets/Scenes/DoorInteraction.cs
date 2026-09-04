using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public string targetSceneName;
    public bool isPlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<Player>() != null)
        {
            isPlayerInRange = true;
            Debug.Log("Player entered door interaction zone: " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<Player>() != null)
        {
            isPlayerInRange = false;
            Debug.Log("Player exited door interaction zone: " + gameObject.name);
        }
    }
}