using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class AbstractPrecondition : MonoBehaviour
{

    private List<EventManager> eventManagers = new List<EventManager>();

    public void AddEventManager(EventManager eventManager)
    {
        if (!eventManagers.Contains(eventManager))
        {
            eventManagers.Add(eventManager);
        }
    }

    public void NotifyPreconditionTriggered()
    {
        foreach(EventManager e in eventManagers)
        {
            e.NotifyPreconditionTriggered();
        }
    }

    public abstract bool IsPreconditionActive();
}
