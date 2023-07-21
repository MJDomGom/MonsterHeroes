using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesObjects : MonoBehaviour
{
    [SerializeField] private AudioClip clickButton;
    [SerializeField] private GameEventControler switches;

    private AudioSource audioSource;
    private bool triggerAllowed = true;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other)
    {
        if (triggerAllowed)
        {
            //if (other.tag == Interactionable) { }
            switches.TriggerEvents();
            audioSource.PlayOneShot(clickButton);
            //this.gameObject.SetActive(false);
            this.enabled = false;
            triggerAllowed = false;
        }

        //Destroy(this);
    }


}
