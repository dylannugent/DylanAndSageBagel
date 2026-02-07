using Unity.VisualScripting;
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

    private bool isTouchingBagel = false;
    private bool canCut = false;

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {

        Mouse mouse = Mouse.current;

        Drag knifeScript = other.GetComponent<Drag>();
        knifeScript.isOverBagel = true;

        if (other.CompareTag(cuttingToolTag) && mouse.leftButton.wasReleasedThisFrame) {

            Debug.Log("Hello");
           
                canCut = true;
                knifeScript.isDraggable = false;
            
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(cuttingToolTag))
        {
            Drag knifeScript = other.GetComponent<Drag>();
            if (knifeScript != null)
            {
                knifeScript.isOverBagel = false;
            }
        }
    }



    void FinishCut()
    {
        // 1. Find the knife by its tag
        GameObject knife = GameObject.FindWithTag(cuttingToolTag);

        if (knife != null)
        {
            Drag knifeScript = knife.GetComponent<Drag>();

            // 2. Set the knife back to draggable and move it home
            knifeScript.isDraggable = true;
            knife.transform.position = knifeScript.startPosition;
        }

        // 3. Spawn the sliced bagel
        Instantiate(slicedBagelPrefab, transform.position, Quaternion.identity);

        // 4. Reset our local cut variable (though the object is about to be destroyed)
        canCut = false;

        // 5. FINALLY destroy the bagel
        Destroy(gameObject);
    }


}
