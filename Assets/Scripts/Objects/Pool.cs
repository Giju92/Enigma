using UnityEngine;
using System.Collections;

public class Pool : Unlockable, Interactable {

    public void Interact() {
        Item item;

        if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

            InventoryManager.Instance.RemoveElement(item.id);

            GetComponent<Animator>().SetTrigger("start");
            GetComponent<AudioSource>().Play();

            transform.parent.GetChild(0).gameObject.SetActive(true);
			transform.parent.GetChild(1).gameObject.SetActive(false);

            areaManager.SolvedOne();
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
