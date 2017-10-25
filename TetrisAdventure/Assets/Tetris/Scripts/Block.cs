using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    private Collectable collectable;
    private Grid grid;

    public void SetColour (Color colour)
    {
        transform.GetComponent<SpriteRenderer>().color = colour;
    }

    public void SetGrid (Grid newGrid)
    {
        grid = newGrid;
    }

    public void Destroy ()
    {
        if (collectable != null)
        {
            int column = grid.GetLocalPosition(transform.position)[0];
            int row = grid.getHeighestAvailableCellInColumn(column);
            collectable.transform.parent = grid.transform;
            collectable.transform.localPosition = new Vector3(column, row, -1);
            collectable.SetActive(true);
        }
        Destroy(gameObject);
    }

    public bool AddCollectable(Collectable newCollactable)
    {
        if (!collectable)
        {
            collectable = newCollactable;
            collectable.transform.parent = transform;
            collectable.transform.localPosition = new Vector3(0, 0, -1);
            return true;
        }
        return false;
    }

    public void CheckCharacterCollision()
    {
        Collider2D characterCollider = Physics2D.OverlapBox(transform.position, transform.lossyScale, 0f, LayerMask.GetMask("Character"));
        if (characterCollider) {
            Vector2 characterPosition = characterCollider.transform.position;
            if (Mathf.Abs(characterPosition.x - transform.position.x) < .158f && characterPosition.y - transform.position.y < .158f && characterPosition.y - transform.position.y > -.63f)
            {
                characterCollider.GetComponent<ICharacterController>().HitByBlock();
            }
        }
    }
}
