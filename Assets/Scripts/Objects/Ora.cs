using UnityEngine;
using System.Collections;

public class Ora : Unlockable, Interactable {

    public Clock clock;

    private int objectsPlaced = 0;

    public void Interact() {

        if (objectsPlaced < requirements.Length) {
            Item item;

            if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

                InventoryManager.Instance.RemoveElement(item.id);

                switch (item.id) {
                    case ItemID.Lever:
                        transform.GetChild(0).gameObject.SetActive(true);
                        break;
                    case ItemID.Gear:
                        transform.GetChild(1).gameObject.SetActive(true);
                        break;
                }

                objectsPlaced++;
            }
        } else {

            areaManager.SolvedOne();
            GetComponent<AudioSource>().Play();
            this.GetComponent<Animator>().SetTrigger("start");
            this.transform.gameObject.layer = LayerMask.GetMask("Default");

            StartCoroutine(WaitForAWhile(1.0f));

        }
    }

    public void SetPointer() {
        Item item;
        if (objectsPlaced < requirements.Length) {
            if (InventoryManager.Instance.SearchInventory(requirements, out item)) {
                PointerManager.Instance.SetIcon(item.icon);
            } else {
                PointerManager.Instance.SetNope();
            }
        } else
            PointerManager.Instance.SetHand();
    }

    IEnumerator WaitForAWhile(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        clock.Begin();
    }


}
