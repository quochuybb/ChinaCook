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
        // Tự động tìm MeshRenderer nếu quên kéo thả
        if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
        
        // Lưu lại màu gốc của viên gạch
        if (_meshRenderer != null)
        {
            _originalMaterial = _meshRenderer.material;
        }
    }

    // Hàm được gọi khi Pug nhìn vào
    public void Highlight()
    {
        if (_highlightMaterial != null && _meshRenderer != null)
        {
            _meshRenderer.material = _highlightMaterial;
        }
    }

    // Hàm được gọi khi Pug quay mặt đi chỗ khác
    public void Unhighlight()
    {
        if (_originalMaterial != null && _meshRenderer != null)
        {
            _meshRenderer.material = _originalMaterial;
        }
    }

    // Hàm này sau sẽ dùng để đặt đĩa thức ăn lên
    public virtual void Interact()
    {
        Debug.Log("Đã tương tác với quầy bếp!");
    }
}