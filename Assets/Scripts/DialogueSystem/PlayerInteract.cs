using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform playerCamera; // 👈 assign in Inspector
    private float interactRange = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out NPCInteractable npcInteractable))
                {
                    // Pass this player's camera
                    npcInteractable.Interact(playerCamera);
                }
            }
        }
    }
}
