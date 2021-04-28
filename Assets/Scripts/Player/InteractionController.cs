using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionController : MonoBehaviour {

    public float range = 2.0f;

    private Camera cam;
    private LayerMask layer;

    public static InteractionController Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        cam = GetComponentInChildren<Camera>();
        layer = LayerMask.GetMask("Interactive");
    }

    private void Update() {
        Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        bool hitConfirmed = Physics.Raycast(rayOrigin, cam.transform.forward, out hit, range, layer);

        if (hitConfirmed) {
            Interactable interactiveObject = hit.transform.GetComponent<Interactable>();

            interactiveObject.SetPointer();

            if (Input.GetButtonDown("Left Click")) {
                interactiveObject.Interact();
            }

        } else {
            PointerManager.Instance.SetPoint();
        }
    }

    public void Disable() {
        this.enabled = false;
    }

    public void Enable() {
        this.enabled = true;
    }
}
