using UnityEngine;

public class FacilitySorting : MonoBehaviour
{
    void Start()
    {
       float sortY =  gameObject.transform.position.y;//y가 높을수록 뒤로
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = (int)(sortY * -1);
    }
}
