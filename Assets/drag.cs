/*using UnityEngine;

public class drag : MonoBehaviour
{

    private bool isDragging = false;

    private void OnMouseDown()
    {
        isDragging = true; 
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Keeps on 2D Plane
            transform.position = mousePos;
        }
    }
}
*/

using UnityEngine;
using UnityEngine.InputSystem; // You must add this line!

public class Drag : MonoBehaviour
{

    [HideInInspector] public Vector3 startPosition;
    private Vector3 offset;
    private bool isDragging = false;
    [HideInInspector] public bool isOverBagel = false;
    [HideInInspector] public bool isDraggable = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 1. Get the current mouse reference from the New Input System
        Mouse mouse = Mouse.current;

        // 2. Check if the left button was JUST pressed
        if (mouse.leftButton.wasPressedThisFrame && isDraggable)
        {
            Vector3 mousePos = GetMouseWorldPos(mouse.position.ReadValue());

            // Check if we clicked the knife's collider
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mousePos;
            }
        }

        // 3. Move while dragging
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPos(mouse.position.ReadValue());
            transform.position = mousePos + offset;
        }

        // 4. Stop dragging
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;

            if(!isOverBagel)
            {
                transform.position = startPosition;
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