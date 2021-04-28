using UnityEngine;
using System.Collections;

public class Profeta : Unlockable, Interactable {

	public Lavagna lavagna;

    public void Interact() {
        Item item;

        if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

            InventoryManager.Instance.RemoveElement(item.id);

			lavagna.Begin();

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
