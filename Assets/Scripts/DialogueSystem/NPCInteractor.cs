using System.Collections.Generic;
using UnityEngine;

public class NPCInteractor : MonoBehaviour
{
    private List<NPCInteractable> npcsInRange = new();

    public Canvas canvas;

    private GameObject currentBubble;

    private PlayerInputStore playerInputStore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInputStore = transform.parent.GetComponent<PlayerInputStore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputStore.playerInput.actions["Pickup_Throw"].IsPressed() && npcsInRange.Count > 0)
        {
            NPCInteractable currentNPC = Utils.GetClosestObject(transform, npcsInRange);

            // TODO: START HERE - spawn bubble and other stuff on npc
            if (currentNPC.currentBubble == null) currentNPC.SpawnSpeechBubble(canvas);
            else currentNPC.DeleteCurrentBubble();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NPCInteractable>(out NPCInteractable npc))
        {
            npcsInRange.Add(npc);
            npc.LoadDialogue();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<NPCInteractable>(out NPCInteractable npc))
        {
            npcsInRange.Remove(npc);
        }
    }
}
