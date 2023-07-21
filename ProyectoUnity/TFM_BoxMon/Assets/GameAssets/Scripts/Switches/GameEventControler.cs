using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event")]
public class GameEventControler : ScriptableObject 
{
    
    private List<GameEventListener> listeners =
        new List<GameEventListener>(); // Awake can also work...

    public void TriggerEvents()
    { 
        for (int i = listeners.Count -1; i >= 0; i--)
        {
            listeners[i].OnEventTriggered();
        }
    }

    public void AddListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
