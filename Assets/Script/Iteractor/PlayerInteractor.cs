using UnityEngine;
using Unity.Netcode;

public class PlayerInteractor : NetworkBehaviour
{
    [SerializeField] private float interactDistance = 2.0f; // Tầm nhìn xa (1.5 ô vuông)
    [SerializeField] private LayerMask counterLayerMask; // RẤT QUAN TRỌNG: Chỉ nhìn quầy bếp
    [SerializeField] private Transform raycastOrigin; // Điểm bắt đầu bắn tia (ví dụ: ngang ngực nhân vật)

    private BaseCounter _selectedCounter; // Nhớ xem mình đang nhìn cái nào

    private void Update()
    {
        // Chống giật lag mạng: Chỉ máy của người chơi (Owner) mới được quyền bắn tia tính toán
        if (!IsOwner) return;

        HandleRaycast();
        HandleInteraction();
    }

    private void HandleRaycast()
    {
        // Nếu không có điểm bắn tia cụ thể, lấy tạm vị trí của nhân vật (lưu ý: nâng lên 1 chút để không bắn trúng sàn nhà)
        Vector3 origin = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 0.5f;

        // Bắn tia Laser (Vị trí bắn, Hướng bắn ra phía trước, Chỗ lưu kết quả, Khoảng cách, Lọc Layer)
        bool hitSomething = Physics.Raycast(origin, transform.forward, out RaycastHit raycastHit, interactDistance, counterLayerMask);

        if (hitSomething)
        {
            // Kiểm tra xem thứ vừa bắn trúng có script BaseCounter không?
            if (raycastHit.transform.TryGetComponent(out BaseCounter counter))
            {
                // Nếu đây là một quầy bếp MỚI (khác với cái đang nhìn)
                if (counter != _selectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                // Bắn trúng cái gì đó nhưng không phải quầy bếp
                SetSelectedCounter(null);
            }
        }
        else
        {
            // Bắn vào không khí
            SetSelectedCounter(null);
        }
    }

    private void HandleInteraction()
    {
        // Khi bấm phím E (hoặc phím bạn cấu hình) và đang nhìn vào một quầy bếp
        if (Input.GetKeyDown(KeyCode.E) && _selectedCounter != null)
        {
            _selectedCounter.Interact();
        }
    }

    // Hàm hỗ trợ đổi màu mượt mà
    private void SetSelectedCounter(BaseCounter newCounter)
    {
        // 1. Tắt màu cái cũ đi
        if (_selectedCounter != null)
        {
            _selectedCounter.Unhighlight();
        }

        // 2. Lưu cái mới
        _selectedCounter = newCounter;

        // 3. Bật màu cái mới lên
        if (_selectedCounter != null)
        {
            _selectedCounter.Highlight();
        }
    }
}