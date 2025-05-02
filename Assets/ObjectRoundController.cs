using UnityEngine;

public class ObjectRoundController : MonoBehaviour
{
    public GameObject pickupObject;     // The object to be picked up
    public Transform tableCenter;       // Where the object should go at round start
    public float roundStartDelay = 5f;  // Time before round starts

    private bool roundStarted = false;
    private bool isPickedUp = false;

    private Transform playerHoldPoint; // Where the player holds the object

    void Start()
    {
        // Optional: set player hold point via tag or inspector
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerHoldPoint = player.transform.Find("HoldPoint");

        Invoke("StartRound", roundStartDelay);
    }

    void Update()
    {
        if (!roundStarted && Input.GetKeyDown(KeyCode.E))
        {
            TryPickUp();
        }
    }

    void TryPickUp()
    {
        if (isPickedUp || pickupObject == null || playerHoldPoint == null) return;

        float distance = Vector3.Distance(pickupObject.transform.position, playerHoldPoint.position);
        if (distance < 2f) // pickup range
        {
            pickupObject.transform.SetParent(playerHoldPoint);
            pickupObject.transform.localPosition = Vector3.zero;
            pickupObject.GetComponent<Rigidbody>().isKinematic = true;
            isPickedUp = true;
        }
    }

    void StartRound()
    {
        roundStarted = true;

        // Detach and move to table center
        pickupObject.transform.SetParent(null);
        pickupObject.transform.position = tableCenter.position;
        pickupObject.transform.rotation = tableCenter.rotation;

        Rigidbody rb = pickupObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Fix it in place
        }

        // Optional: disable collider if needed
        Collider col = pickupObject.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }
}