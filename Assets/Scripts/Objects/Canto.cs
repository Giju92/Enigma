using UnityEngine;
using System.Collections;

public class Canto : Unlockable, Interactable {

    private int objectsPlaced = 0;

    public Drawer drawer;

    public void Interact() {
        Item item;

        if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

            InventoryManager.Instance.RemoveElement(item.id);

            switch (item.id) {
                case ItemID.Ball:
                    transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case ItemID.Glove:
                    transform.GetChild(1).gameObject.SetActive(true);
                    break;
                case ItemID.Head:
                    transform.GetChild(2).gameObject.SetActive(true);
                    break;
            }

            objectsPlaced++; 
            if (objectsPlaced >= requirements.Length) {

                drawer.Open();
                areaManager.SolvedOne();               
                this.transform.gameObject.layer = LayerMask.GetMask("Default");
            }
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
