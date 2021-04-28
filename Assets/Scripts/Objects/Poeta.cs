using UnityEngine;
using System.Collections;
using System;

public class Poeta : Unlockable, Interactable {

    public Stairs stairs;
    public Transform wall;

    public void Interact() {
        Item item;

        if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

            InventoryManager.Instance.RemoveElement(item.id);
            areaManager.SolvedOne();

            transform.GetChild(0).gameObject.SetActive(true);
            transform.parent.GetComponent<Animator>().SetTrigger("start");
            GetComponent<AudioSource>().Play();
            StartCoroutine(StartStairs());


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

    IEnumerator StartStairs() {
        yield return new WaitForSeconds(1);
        stairs.Begin();
        Destroy(wall.gameObject);
    }
}
