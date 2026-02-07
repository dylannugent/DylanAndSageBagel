using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BagelCut : MonoBehaviour
{
    public float cutTimeRequired = 2.0f;
    private float currentProgress = 0f;
    public GameObject slicedBagelPrefab;
    public string cuttingToolTag = "Knife";
    public Slider progressBar;

    private bool isKnifeInside = false; // The physics flag
    private bool canCut = false;

    void Start()
    {
        if (progressBar != null) progressBar.gameObject.SetActive(false);
    }

    // 1. Physics only sets the "Inside" flag
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(cuttingToolTag))
        {
            isKnifeInside = true;
            Drag knife = other.GetComponent<Drag>();
            if (knife != null) knife.isOverBagel = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(cuttingToolTag))
        {
            isKnifeInside = false;
            Drag knife = other.GetComponent<Drag>();
            if (knife != null) knife.isOverBagel = false;
        }
    }

    // 2. Update handles Input and Progress (very reliable)
    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        // If knife is touching AND you let go anywhere on the bagel
        if (isKnifeInside && mouse.leftButton.wasReleasedThisFrame)
        {
            canCut = true;

            // Disable knife dragging
            GameObject knifeObj = GameObject.FindWithTag(cuttingToolTag);
            if (knifeObj != null) knifeObj.GetComponent<Drag>().isDraggable = false;
        }

        if (canCut)
        {
            progressBar.gameObject.SetActive(true);
            currentProgress += Time.deltaTime;
            progressBar.value = currentProgress / cutTimeRequired;

            if (currentProgress >= cutTimeRequired)
            {
                FinishCut();
            }
        }
    }

    void FinishCut()
    {
        GameObject knife = GameObject.FindWithTag(cuttingToolTag);
        if (knife != null)
        {
            Drag knifeScript = knife.GetComponent<Drag>();
            knifeScript.isDraggable = true;
            knife.transform.position = knifeScript.startPosition;
        }

        Instantiate(slicedBagelPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}