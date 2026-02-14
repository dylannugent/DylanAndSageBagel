/*using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // You must add this line!

public class DraggableItem : MonoBehaviour
{

    private bool isDragging = false;
    private GameObject currentBagel = null;
    public string thisItem;
    private int currentItem;

    public ItemSpawner tagFind;

    static int[] itemsList = {0, 1, 1 };

    private int checkItem()
    {

        if (gameObject.CompareTag("Butter"))
        {
            currentItem = 1;
            Debug.Log("atButter");
            return 1;
        }
        else if (gameObject.CompareTag("Bacon"))
        {
            currentItem = 2;
            Debug.Log("atBacon");
            return 2;
        }

        return 0;

    }

    public void StartDragging()
    {
        isDragging = true;
    }

    void Update()
    {
        if (isDragging)
        {



            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = 10f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(worldPos.x, worldPos.y, 0);

            if (currentBagel != null)
            {
                Debug.Log("Currently hovering over: " + currentBagel.name);
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {

                Debug.Log(itemsList[checkItem()]);
                Debug.Log(currentBagel);

                if (currentBagel != null && itemsList[checkItem()] == 1)
                {
                    transform.SetParent(currentBagel.transform);
                    transform.localPosition = new Vector3(0, 0, -0.1f);
                    itemsList[checkItem()] = 0;

                    Debug.Log("in Enter");

                    isDragging = false;
                    Destroy(this);
                    return;

                }
                else
                {
                    Destroy(gameObject);
                }
              
            }



        }


    }

     private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Bagel"))
        {
            currentBagel = other.gameObject;
            Debug.Log("in enter.");
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bagel"))
        {
            // Only set to null if we were actually hovering over THIS specific bagel
            if (currentBagel == other.gameObject)
            {
                currentBagel = null;
                Debug.Log("Exited Bagel");
            }
        }
    }
}

*/

using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DraggableItem : MonoBehaviour
{
    private bool isDragging = false;
    private GameObject currentBagel = null;

    // Static means this list is shared across ALL butter/bacon items
    static int[] itemsList = { 0, 1, 1 };

    public void StartDragging() { isDragging = true; }

    private int GetItemIndex()
    {
        // Use CompareTag on 'this' object
        if (gameObject.CompareTag("Butter")) return 1;
        if (gameObject.CompareTag("Bacon")) return 2;
        return 0;
    }

    void Update()
    {
        if (!isDragging) return;

        // 1. Follow Mouse
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0);

        // 2. MANUAL SCAN: Check if we are over a bagel right now
        // This creates a tiny circle at the butter's position to look for a bagel
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.2f);
        if (hit != null && hit.CompareTag("Bagel1"))
        {
            currentBagel = hit.gameObject;
        }
        else
        {
            currentBagel = null;
        }

        // 3. Handle Release
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            int index = GetItemIndex();

            // This should now say "Bagel? True" if you are hovering!
            Debug.Log($"Release Check: Bagel? {currentBagel != null} | Index: {index} | ArrayValue: {itemsList[index]}");

            if (currentBagel != null && index != 0 && itemsList[index] == 1)
            {
                transform.SetParent(currentBagel.transform);
                transform.localPosition = new Vector3(0, 0, -0.1f);
                itemsList[index] = 0;
                isDragging = false;

                Debug.Log("STUCK!");
                Destroy(this);
                return;
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bagel"))
        {
            currentBagel = other.gameObject;
            Debug.Log("Entered Bagel Trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bagel") && currentBagel == other.gameObject)
        {
            currentBagel = null;
            Debug.Log("Exited Bagel Trigger");
        }
    }
}