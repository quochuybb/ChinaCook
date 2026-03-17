using UnityEngine;
using Unity.Netcode;

public class PlayerInteractor : NetworkBehaviour
{
    [SerializeField] private float interactDistance = 2.0f; 
    [SerializeField] private LayerMask counterLayerMask; 
    [SerializeField] private Transform raycastOrigin; 
    [SerializeField] private PlayerInventory inventory;

    private BaseCounter _selectedCounter; 

    private void Update()
    {
        if (!IsOwner) return;

        HandleRaycast();
        HandleInteraction();
    }

    private void HandleRaycast()
    {
        Vector3 origin = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 0.5f;

        bool hitSomething = Physics.Raycast(origin, transform.forward, out RaycastHit raycastHit, interactDistance, counterLayerMask);

        if (hitSomething)
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter))
            {
                if (counter != _selectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && _selectedCounter != null)
        {
            _selectedCounter.Interact(inventory);
        }
    }

    private void SetSelectedCounter(BaseCounter newCounter)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Unhighlight();
        }

        _selectedCounter = newCounter;

        if (_selectedCounter != null)
        {
            _selectedCounter.Highlight();
        }
    }
}