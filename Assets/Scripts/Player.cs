using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Player : MonoBehaviour
{
    [SerializeField] float maxWalkSpeed = 2.5f;
    [SerializeField] float maxSprintSpeed = 7.5f;
    [SerializeField] float jumpForce = 1f;
    [SerializeField] Vector2 lookSensitivity = new Vector2(1f, 0.5f);
    [SerializeField] Transform cameraTransform = null;
    [SerializeField] Transform objectsContainer = null;

    PlayerInput playerInput;
    CharacterController characterController;
    Rigidbody rb;

    InputAction moveAction;
    InputAction sprintAction;
    InputAction lookAction;
    InputAction jumpAction;
    InputAction attackAction;

    Vector3 moveValue = Vector3.zero;
    bool isGrounded = false;
    Animator weaponAnimator;
    float lastAttackTime = 0f;
    Weapon currentWeapon = null;
    int currentWeaponIndex = 0;



    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        moveAction = playerInput.actions.FindAction("Move");
        sprintAction = playerInput.actions.FindAction("Sprint");
        lookAction = playerInput.actions.FindAction("Look");
        jumpAction = playerInput.actions.FindAction("Jump");
        attackAction = playerInput.actions.FindAction("Attack");

        jumpAction.performed += Jump;
    }

    void Start()
    {
        EquipWeapon(WeaponItem.Base);
    }

    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * lookAction.ReadValue<Vector2>().x * lookSensitivity.x);
        cameraTransform.localEulerAngles = new Vector3(cameraTransform.localEulerAngles.x - (lookAction.ReadValue<Vector2>().y * lookSensitivity.y), 0f, 0f);

        if (currentWeapon.WeaponItem != WeaponItem.Base)
        {
            currentWeapon.GetComponent<Animator>().SetBool("Attacking", attackAction.IsPressed());
        }

        if (Cursor.lockState == CursorLockMode.None || !GameManager.Instance.gameStarted) return;

        if (!currentWeapon.ContinuousAttack && attackAction.WasPerformedThisFrame() ||
            currentWeapon.ContinuousAttack && attackAction.IsPressed())
        {
            Attack();
        }

        float scrollValue = Mouse.current.scroll.y.ReadValue();
        if (scrollValue != 0)
        {
            if (scrollValue > 0)
            {
                for (int i = currentWeaponIndex; i < currentWeaponIndex + objectsContainer.childCount; i++)
                {
                    int nextIndex = i + 1 > objectsContainer.childCount ? 0 : i + 1;

                    switch (nextIndex)
                    {
                        case 0: 
                            EquipWeapon((WeaponItem)nextIndex); 
                            return;
                            break;
                        case 1: 
                            if (GameManager.Instance.RebelLevel > 0) 
                            { 
                                EquipWeapon((WeaponItem)nextIndex); 
                                return;
                            } 
                            break;
                        case 2:
                            if (GameManager.Instance.MagicianLevel > 0)
                            {
                                EquipWeapon((WeaponItem)nextIndex);
                                return;
                            } 
                            break;
                        case 3:
                            if (GameManager.Instance.InnocentLevel > 0)
                            {
                                EquipWeapon((WeaponItem)nextIndex); 
                                return;
                            }
                            break;
                        case 4:
                            if (GameManager.Instance.ExplorerLevel > 0)
                            {
                                EquipWeapon((WeaponItem)nextIndex); 
                                return;
                            }
                            break;
                    }
                }
            }
            else
            {
                for (int i = currentWeaponIndex; i > currentWeaponIndex - objectsContainer.childCount; i--)
                {
                    int nextIndex = i - 1 < 0 ? (objectsContainer.childCount + i) - 1 : i - 1;

                    switch (nextIndex)
                    {
                        case 0:
                            EquipWeapon((WeaponItem)nextIndex); 
                            return; 
                            break;
                        case 1:
                            if (GameManager.Instance.RebelLevel > 0)
                            {
                                EquipWeapon((WeaponItem)nextIndex);
                                return;
                            }
                            break;
                        case 2:
                            if (GameManager.Instance.MagicianLevel > 0)
                            {
                                EquipWeapon((WeaponItem)nextIndex);
                                return;
                            }
                            break;
                        case 3:
                            if (GameManager.Instance.InnocentLevel > 0)
                            {
                                EquipWeapon((WeaponItem)nextIndex);
                                return;
                            }
                            break;
                        case 4:
                            if (GameManager.Instance.ExplorerLevel > 0)
                            {
                                EquipWeapon((WeaponItem)nextIndex);
                                return;
                            }
                            break;
                    }
                }
            }
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            EquipWeapon(WeaponItem.Base);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame && GameManager.Instance.RebelLevel > 0)
        {
            EquipWeapon(WeaponItem.Mace);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame && GameManager.Instance.MagicianLevel > 0)
        {
            EquipWeapon(WeaponItem.Staff);
        }
        else if (Keyboard.current.digit4Key.wasPressedThisFrame && GameManager.Instance.InnocentLevel > 0)
        {
            EquipWeapon(WeaponItem.Flower);
        }
        else if (Keyboard.current.digit5Key.wasPressedThisFrame && GameManager.Instance.ExplorerLevel > 0)
        {
            EquipWeapon(WeaponItem.Hat);
        }
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude < (sprintAction.IsPressed()? maxSprintSpeed : maxWalkSpeed))
        {
            Vector3 movementVector = transform.forward * moveValue.y + transform.right * moveValue.x;
            rb.linearVelocity += movementVector;
        }

        isGrounded = Physics.Raycast(transform.position - (transform.up * 0.9f), -transform.up, 0.2f, -1, QueryTriggerInteraction.Ignore);
    }

    void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            rb.linearVelocity += Vector3.up * jumpForce;
        }
    }

    void Attack()
    {
        if ((lastAttackTime == 0 || lastAttackTime + currentWeapon.AttackRate <= Time.time))
        {
            if (currentWeapon.WeaponItem == WeaponItem.Base)
            {
                currentWeapon.Attack();
                currentWeapon.GetComponent<VisualEffect>().SendEvent("Attack");
            }
            else if (weaponAnimator)
            {
                weaponAnimator.SetTrigger("Attack");
            }

            lastAttackTime = Time.time;
        }
    }

    void EquipWeapon(WeaponItem weapon)
    {
        for (int i = 0; i < objectsContainer.childCount; i++)
        {
            objectsContainer.GetChild(i).gameObject.SetActive((int)weapon == i);
        }

        weaponAnimator = GetComponentInChildren<Animator>(false);
        currentWeapon = GetComponentInChildren<Weapon>(false);
        currentWeaponIndex = (int)weapon;

        HUD.Instance.SetArchetype(currentWeapon.Sprite, currentWeapon.Archetype);
        lastAttackTime = 0;
    }
}
