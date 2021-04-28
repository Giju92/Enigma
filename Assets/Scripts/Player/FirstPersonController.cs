using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonController : MonoBehaviour {
    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_StickToGroundForce;
    [SerializeField]
    private float m_GravityMultiplier;
    [SerializeField]
    private MouseLook m_MouseLook;
    [SerializeField]
    private bool m_UseHeadBob;
    [SerializeField]
    private CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField]
    private float m_StepInterval;
    [SerializeField]
    private AudioClip[] m_FootstepSounds;

    private Camera m_Camera;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private AudioSource m_AudioSource;

    private bool cursorEnabled = false;
    private bool lookEnabled = true;
    private bool movementEnabled = true;
    private Transform focusTarget;

    public static FirstPersonController Instance { get; set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_AudioSource = GetComponent<AudioSource>();
        m_MouseLook.Init(transform, m_Camera.transform);
    }

    private void Update() {
        if (lookEnabled) {
            if (focusTarget != null) {

                Quaternion focusRotation = Quaternion.LookRotation(focusTarget.position - m_Camera.transform.position);
                Quaternion focusCameraRotation = Quaternion.Euler(new Vector3(focusRotation.eulerAngles.x, 0f, 0f));
                Quaternion focusCharacterRotation = Quaternion.Euler(new Vector3(0f, focusRotation.eulerAngles.y, 0f));

                m_Camera.transform.localRotation = Quaternion.Slerp(m_Camera.transform.localRotation, focusCameraRotation, 5f * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, focusCharacterRotation, 5f * Time.deltaTime);
            } else {

                m_MouseLook.LookRotation(transform, m_Camera.transform);
            }
        }
    }
    private void FixedUpdate() {

        if (!cursorEnabled) {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (movementEnabled) {
            GetInput();

            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * m_WalkSpeed;
            m_MoveDir.z = desiredMove.z * m_WalkSpeed;

            m_MoveDir.y = -m_StickToGroundForce;

            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle();
            UpdateCameraPosition();
        }
    }

    public void GetInput() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Input = new Vector2(horizontal, vertical);
        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1) {
            m_Input.Normalize();
        }
    }

    public void SetFocus(Transform target) {
        focusTarget = target;
        InteractionController.Instance.Disable();
    }

    public void ResetFocus() {
        focusTarget = null;
        InteractionController.Instance.Enable();
        m_MouseLook.Init(Instance.transform, m_Camera.transform);
    }

    public void DisableMovement() {
        movementEnabled = false;
    }

    public void EnableMovement() {
        movementEnabled = true;
    }

    public void DisableLook() {
        lookEnabled = false;
    }

    public void EnableLook() {
        lookEnabled = true;
    }

    public bool IsLookEnabled() {
        return lookEnabled;
    }

    public void DisableCursor() {
        cursorEnabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EnableCursor() {
        cursorEnabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartAnimation() {
        DisableCursor();
        GetComponent<Animator>().SetTrigger("revertZoom");
    }

    public void StopAnimation() {
        GetComponent<Animator>().enabled = false;
        GameManager.Instance.SwitchToTutorial();
    }

    private void ProgressStepCycle() {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0)) {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (m_WalkSpeed)) * Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep)) {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio() {
        if (!m_CharacterController.isGrounded) {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }

    private void UpdateCameraPosition() {
        Vector3 newCameraPosition;
        if (!m_UseHeadBob) {
            return;
        }
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded) {
            m_Camera.transform.localPosition =
                m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude + m_WalkSpeed);
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y;
        } else {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y;
        }
        m_Camera.transform.localPosition = newCameraPosition;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below) {
            return;
        }

        if (body == null || body.isKinematic) {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
