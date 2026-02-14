using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChosenBagel : MonoBehaviour
{

    public GameObject thisBagel;
    public Vector3[] spawnPosition;
    private static bool[] spawnedHere = new bool[4];


    private Vector3 bestSpawn;

    // Update is called once per frame
    void Update()
    {


        // 1. Get the current mouse reference from the New Input System
        Mouse mouse = Mouse.current;

        // 2. Check if the left button was JUST pressed
        if (mouse.leftButton.wasPressedThisFrame)
        {
            Vector3 mousePos = GetMouseWorldPos(mouse.position.ReadValue());

            // Check if we clicked the knife's collider
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {

                int i;

                for (i = 0; i < 4; i++)
                {

                    if (spawnedHere[i] == false)
                    {
                        bestSpawn = spawnPosition[i];
                        bestSpawn = spawnPosition[i];
                        spawnedHere[i] = true;

                        GameObject clone  = Instantiate(thisBagel, bestSpawn, Quaternion.identity);

                        clone.tag = "Bagel" + i;

                        break;
                    }


                }

            }
        }


    }

    private Vector3 GetMouseWorldPos(Vector2 screenPos)
    {
        // We use 10 for Z because that's usually the distance from Camera to 0
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
        worldPos.z = 0;
        return worldPos;
    }
}


