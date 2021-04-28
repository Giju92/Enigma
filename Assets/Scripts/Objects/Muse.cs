using UnityEngine;

public class Muse : Unlockable, Interactable {

    public Animator coverAnimator;

    public void Interact() {
        Item item;

        if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

            InventoryManager.Instance.RemoveElement(item.id);
            areaManager.SolvedOne();

            coverAnimator.SetTrigger("start");
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
