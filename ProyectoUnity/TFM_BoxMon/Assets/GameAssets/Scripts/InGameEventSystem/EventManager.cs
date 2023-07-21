using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Tooltip("Preconditions to trigger the effects")]
    [SerializeField] private AbstractPrecondition[] preconditions;

    [Tooltip("Effects that preconditions would trigger")]
    [SerializeField] private AbstracEffect[] effects;

    [Tooltip("Indicates if the event manager is an or (True) or and (False)")]
    [SerializeField] private bool IsOrEvent = true;

    [Tooltip("Indicates if the event can be triggered more than once")]
    [SerializeField] private bool isMultipleTriggerEvent = false;

    private bool triggered = false;

	private void Start()
	{
		foreach(var precondition in preconditions)
        {
            precondition.AddEventManager(this);
        }
	}

	public void NotifyPreconditionTriggered()
    {
        // Do not allow multiple triggers of the event if it is not a multiple trigger event
        if (!isMultipleTriggerEvent && triggered)
        {
            return;
        }

        // TODO Evaluate if the effects must be triggered
        if (IsOrEvent)
        {
            foreach(AbstracEffect effect in effects)
            {
                effect.TriggerEffect();
            }
        } else
        {
            bool triggerEffect = true;
            foreach(AbstractPrecondition precondition in preconditions) { 
                if (!precondition.IsPreconditionActive())
                {
                    triggerEffect = false; break;
                }
            }

            if (triggerEffect)
            {
				foreach (AbstracEffect effect in effects)
				{
					effect.TriggerEffect();
				}
			}
        }
    }
}
