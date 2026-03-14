using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterInput : CharacterController
{
    [SerializeField] private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
    }
    
    public void OnMove(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();
        OnMoveEvent.Invoke(direction);
    }
    
    
    public void OnDash(InputValue value)
    {
        base.OnDash.Invoke();
    }
    
}