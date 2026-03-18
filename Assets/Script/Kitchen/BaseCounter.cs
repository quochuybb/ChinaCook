using UnityEngine;
using Unity.Netcode;

// Kế thừa NetworkBehaviour để sau này đồng bộ đồ ăn qua mạng
public class BaseCounter : NetworkBehaviour 
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _highlightMaterial;
    
    private Material _originalMaterial;

    private void Awake()
    {
        if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
        
        if (_meshRenderer != null)
        {
            _originalMaterial = _meshRenderer.material;
        }
    }

    public void Highlight()
    {
        if (_highlightMaterial != null && _meshRenderer != null)
        {
            _meshRenderer.material = _highlightMaterial;
        }
    }

    public void Unhighlight()
    {
        if (_originalMaterial != null && _meshRenderer != null)
        {
            _meshRenderer.material = _originalMaterial;
        }
    }

    public virtual void Interact(PlayerInventory inventory)
    {
        Debug.Log("Đã tương tác với quầy bếp!");
    }
    public virtual void Cut(PlayerInventory inventory)
    {
        Debug.Log("Đã tương tác với quầy bếp!");
    }
}