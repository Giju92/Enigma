using UnityEngine;
using System.Collections;

public class LastTile :  Unlockable, Interactable {


	void Start () {

		Renderer rend = GetComponent<Renderer> ();
		rend.enabled = false;

	}

	public void active(){

		gameObject.layer = LayerMask.NameToLayer("Interactive");
	}

	public void Interact() {
		Item item;

		if (InventoryManager.Instance.SearchInventory(requirements, out item)) {

			Renderer rend = GetComponent<Renderer> ();
			rend.enabled = true;

            GetComponent<AudioSource>().Play();

            transform.parent.parent.GetChild(1).gameObject.SetActive(true);

			InventoryManager.Instance.RemoveElement(item.id);
			areaManager.SolvedOne();

			GameObject.Find ("Puzzle").GetComponent<Animator> ().SetBool ("Open", true);

            gameObject.layer = LayerMask.NameToLayer("Default");
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
