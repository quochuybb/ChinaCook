using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : NetworkBehaviour
{
    private static readonly ServerRpcParams DefaultServerRpcParams = new ServerRpcParams();

    
    public virtual void Update()
    {

    }
    

    private readonly MoveEvent _moveEvent = new MoveEvent();
    private readonly UnityEvent _onDash = new UnityEvent();
    

    public UnityEvent OnDash => _onDash;
    public MoveEvent OnMoveEvent => _moveEvent;
}