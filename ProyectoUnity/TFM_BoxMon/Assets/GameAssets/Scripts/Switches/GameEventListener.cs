using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEventControler switchesControler;
    public UnityEvent onInteractionEvent;

    private void OnEnable()
    {
        switchesControler.AddListener(this);
    }

    private void OnDisable()
    {
        switchesControler.RemoveListener(this);
    }

    public void OnEventTriggered()
    {
        onInteractionEvent.Invoke();
    }
}
