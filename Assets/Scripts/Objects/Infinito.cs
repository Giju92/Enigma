using UnityEngine;
using System.Collections;

public class Infinito : Unlockable, Interactable {

    private int objectsPlaced = 0;

    public void Interact() {
        Item item;

        if (objectsPlaced < 4) {
            if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

                InventoryManager.Instance.RemoveElement(item.id);

                if (objectsPlaced < 2) {
                    transform.GetChild(0).GetChild(objectsPlaced).gameObject.SetActive(true);
                } else {
                    transform.GetChild(1).GetChild(objectsPlaced - 2).gameObject.SetActive(true);
                }

                objectsPlaced++;

                if (objectsPlaced >= 4) {
                    areaManager.SolvedOne();
                    GameManager.Instance.SwitchToEndMusic();
                }

            }
        } else {
            GetComponent<AudioSource>().Play();
            GetComponent<Animator>().SetTrigger("open");

            this.transform.gameObject.layer = LayerMask.GetMask("Default");
            GameManager.Instance.SwitchToEnding();
        }
    }

    public void SetPointer() {
        if (objectsPlaced < 4) {
            Item item;
            if (InventoryManager.Instance.SearchInventory(requirements, out item)) {
                PointerManager.Instance.SetIcon(item.icon);
            } else {
                PointerManager.Instance.SetNope();
            }
        } else {
            PointerManager.Instance.SetHand();
        }
    }
}
