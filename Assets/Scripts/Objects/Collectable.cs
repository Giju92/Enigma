using UnityEngine;
using System;

public class Collectable : MonoBehaviour, Interactable {

    public Item item;
    public AreaManager areaManager;

    public void Interact() {
        InventoryManager.Instance.AddElement(item);
        areaManager.SolvedOne();
        Destroy(transform.gameObject);
    }

    public void SetPointer() {
        PointerManager.Instance.SetHand();
    }
}
