using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    public bool oneTimeEvent { get; } = true;    

    [SerializeField]
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void _Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void _EnableEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i]._EnableEvent();
        }
    }

    public void _DisableEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i]._DisableEvent();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
