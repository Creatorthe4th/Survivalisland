using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f;
    public TextMeshProUGUI promptText;
    public float interactionCooldown = 0.5f;
    public Inventory inventory;
    public AxeWeapon axeWeapon;

    private InteractableResource currentResource;

    private Animator animator;
    private bool isInteracting;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
        }

        if (axeWeapon == null)
        {
            axeWeapon = GetComponent<AxeWeapon>();
        }

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        FindNearbyResource();
    }

    private void FindNearbyResource()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);

        InteractableResource closestResource = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            InteractableResource resource = hitCollider.GetComponent<InteractableResource>();
            if (resource != null)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestResource = resource;
                }
            }
        }

        currentResource = closestResource;

        if (promptText == null)
        {
            return;
        }

        if (currentResource != null && !isInteracting)
        {
            promptText.text = currentResource.promptText;
            promptText.gameObject.SetActive(true);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }

    public void OnInteract(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if (currentResource == null || isInteracting)
        {
            return;
        }

        StartCoroutine(HandleInteraction());
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }

        if (axeWeapon != null)
        {
            axeWeapon.TryAttack();
        }
    }

    private IEnumerator HandleInteraction()
    {
        isInteracting = true;

        if (animator != null && currentResource != null)
        {
            animator.SetTrigger(currentResource.animationTrigger);
        }

        currentResource.Interact(inventory);

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(interactionCooldown);

        isInteracting = false;
    }
}