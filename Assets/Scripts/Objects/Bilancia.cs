using UnityEngine;
using System.Collections;

public class Bilancia : Unlockable, Interactable {

    private int objectsPlaced = 0;
    private bool move;
    private Vector3 destination;
    private float lerpTime = 1f, currentTime = 0f;

    public SlideDoor slideDoor;

    public void Interact() {

        Item item;
        if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

            InventoryManager.Instance.RemoveElement(item.id);

            if (!move)
                destination = transform.position - new Vector3(0f, 0.1f, 0f);
            else
                destination -= new Vector3(0f, 0.1f, 0f);

            currentTime = 0f;
            move = true;


            transform.GetChild(objectsPlaced++).gameObject.SetActive(true);

            if (objectsPlaced >= requirements.Length) {
                slideDoor.Activate();
                areaManager.SolvedOne();
                GetComponent<AudioSource>().Play();
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

    public void Update() {
        if (move) {
            if (currentTime <= lerpTime) {
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, destination, currentTime);
            } else {
                move = false;
                currentTime = 0f;
            }

        }
    }
}
