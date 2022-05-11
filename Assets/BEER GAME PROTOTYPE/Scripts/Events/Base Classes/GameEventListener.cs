using UnityEngine;
using UnityEngine.Events;


public class GameEventListener : MonoBehaviour
{
    [Header("EVENT")]
    [SerializeField]
    private GameEvent gameEvent;
    [SerializeField] bool eventEnabled;

    [Header("DEBUG")]
    [SerializeField]
    bool eventInvoked;    

    [Header("RESPONSE")]
    [SerializeField]
    private UnityEvent response;
    
    private void OnEnable() => gameEvent.RegisterListener(this);
    
    private void OnDisable() => gameEvent.UnregisterListener(this);

    public void OnEventRaised()
    {
        if (eventInvoked) { return; }
        if(!eventEnabled)
        {
            Debug.LogWarning("Event " + gameEvent + " not invoked. Trying to invoke a disabled event.");
            return;
        }

        response.Invoke();

        if (gameEvent.oneTimeEvent) { eventInvoked = true; }        
    }

    public void _EnableEvent() => eventEnabled = true;

    public void _DisableEvent() => eventEnabled = false;
    
}
