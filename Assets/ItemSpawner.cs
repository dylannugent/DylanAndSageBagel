using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : MonoBehaviour
{

    public GameObject itemPrefab;
    public string clickedTag;

  
    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);


            if (hit2D.collider != null && hit2D.collider.gameObject == gameObject)
            {

                clickedTag = hit2D.collider.gameObject.tag;

                SpawnAndDrag();
            }

        }
    }

    void SpawnAndDrag()
    {
        GameObject clone = Instantiate(itemPrefab, transform.position,Quaternion.identity);

        DraggableItem dragScript = clone.AddComponent<DraggableItem>();

        clone.tag = clickedTag;

        Debug.Log(clickedTag);

        dragScript.StartDragging();
    }

}
