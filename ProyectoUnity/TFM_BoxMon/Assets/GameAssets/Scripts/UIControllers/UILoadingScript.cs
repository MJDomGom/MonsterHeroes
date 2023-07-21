using TMPro;
using UnityEngine;

public class UILoadingScript : MonoBehaviour
{
    private TMP_Text textMeshProReferenceText;

    private string initialText;
    private int nDots = 2;

    // Start is called before the first frame update
    void Awake()
    {
        textMeshProReferenceText = GetComponent<TMP_Text>();
#if UNITY_EDITOR
        if (textMeshProReferenceText == null)
        {
            Debug.LogError("Error - loading text " + transform.name + " has no text mesh pro reference assigned");
        }
#endif

        if (textMeshProReferenceText != null)
        {
            initialText = textMeshProReferenceText.text;
        }

    }

	private void Start()
	{
        InvokeRepeating("UpdateText", 0.5f, 0.5f);
	}


	private void UpdateText()
    {
        textMeshProReferenceText.text = initialText;
        nDots = (nDots + 1) % 3;
        for(int i=0; i<=nDots; i++)
        {
            textMeshProReferenceText.text += ".";
		}
    }
}
