using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    // Define a class in wich for every inventory element we have its Item and GameObject instances
    private class InventoryElement {
        public Item item;
        public GameObject instance;

        public InventoryElement(Item item, GameObject instance) {
            this.item = item;
            this.instance = instance;
        }
    }

    // Define a fixed array of inventory elements
    private InventoryElement[] elements;
    private Animator animator;
    private RectTransform position;
    public static InventoryManager Instance { get; private set; }

    void Awake() {
        Instance = this;
        animator = GetComponent<Animator>();
        elements = new InventoryElement[20];
        position = GetComponent<RectTransform>();
    }

    // Adds an element to the inventory
    public void AddElement(Item item) {

        // Creates a new ui object with the item's icon and attach it to the inventory object
        GameObject element = new GameObject();
        Image image = element.AddComponent<Image>();
        element.transform.SetParent(transform, false);
        element.name = item.id.ToString();
        image.sprite = item.icon;

        // Add the element to the array at the position given by its item id
        elements[(int)item.id] = new InventoryElement(item, element);

        // Finally, slide in the inventory to show to the user that the item has been added
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SlideIn")) {
            animator.SetTrigger("slideIn");
        } 

        GetComponent<AudioSource>().Play();
        if (item.id >= ItemID.Handle1)
            GameManager.Instance.PlayHighNote();
    }

    // Remove an element from the inventory
    public void RemoveElement(ItemID id) {

        // Destroy the actual instance and remove it from the array
        Destroy(elements[(int)id].instance);
        elements[(int)id] = null;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SlideIn")) {
            animator.SetTrigger("slideIn");
        }
    }

    // Search the inventory until it finds one of the items listed in the requirements, 
    // returns true and the item in output if it is successful
    public bool SearchInventory(ItemID[] requirements, out Item item) {
        foreach (ItemID i in requirements) {
            if (elements[(int)i] != null) {
                item = elements[(int)i].item;
                return true;
            }
        }
        item = null;
        return false;
    }

    // Returns true if the inventory contains the item
    public bool Contains(ItemID id) {
        return elements[(int)id] != null;
    }

    public void SwitchToPauseScreen() {
        animator.enabled = false;
        position.anchoredPosition = new Vector2(0, 0);
    }

    public void SwitchToBookScreen() {
        animator.enabled = false;
        position.anchoredPosition = new Vector2(0, -60);
    }

    public void SwitchToGameScreen() {
        position.anchoredPosition = new Vector2(0, -60);
        animator.enabled = true;
    }
}
