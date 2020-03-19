using UnityEngine;
using UnityEngine.EventSystems;

public class Introduction : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
	public GameObject prefab;
    private GameObject image;
    public void OnPointerEnter(PointerEventData eventData)
	{
        image=Instantiate(Resources.Load("UI/Introductions/"+prefab.name)) as GameObject;
        image.transform.SetParent(GameObject.Find("UICanvas").transform);
        image.transform.position =new Vector2(transform.position.x+transform.GetComponent<RectTransform>().rect.size.x/2,transform.GetComponent<RectTransform>().rect.size.y + image.transform.GetComponent<RectTransform>().rect.size.y/2);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(image);
    }
}
