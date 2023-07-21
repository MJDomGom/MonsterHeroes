using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TimedSpikeController : MonoBehaviour
{
    private readonly int AC_BOOL_SPIKE = Animator.StringToHash("spikes");

    [SerializeField] private float timeActivated = 5f;
    [SerializeField] private float timeDeactivated = 5f;
    [SerializeField] private bool initiallyActive = false;

    private Animator animator;
    private bool currentlyActive;
    private float timeToChange;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	// Start is called before the first frame update
	void Start()
    {
        animator.SetBool(AC_BOOL_SPIKE, initiallyActive);
        currentlyActive = initiallyActive;
        ScheduleNextChange();
	}

	private void OnDisable()
	{
        CancelInvoke();
	}

    private void ScheduleNextChange()
    {
        Invoke("UpdateSpikesState", currentlyActive ? timeDeactivated : timeActivated);
    }

	private void UpdateSpikesState()
    {
        currentlyActive = !currentlyActive;
        animator.SetBool(AC_BOOL_SPIKE, currentlyActive);
        ScheduleNextChange();
    }

}
