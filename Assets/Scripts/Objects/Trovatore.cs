using UnityEngine;
using System.Collections;

public class Trovatore : Unlockable, Interactable {

    public void Interact() {
        Item item;

        if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

            InventoryManager.Instance.RemoveElement(item.id);
            areaManager.SolvedOne();

            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).GetComponent<Interactable>().Interact();

            GetComponent<AudioSource>().Play();

            this.transform.gameObject.layer = LayerMask.GetMask("Default");
        }
    }

    public void SetPointer() {
        Item item;
        if (InventoryManager.Instance.SearchInventory(requirements, out item)) {
            PointerManager.Instance.SetIcon(item.icon);
        } else {
            PointerManager.Instance.SetNope();
        }
    }
}
