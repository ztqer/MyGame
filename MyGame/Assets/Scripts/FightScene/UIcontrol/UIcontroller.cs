using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    internal Shop shop;//商店类实例
    internal int activeSoilders;//上场干员人数
    private GameObject dragSoilder;//拖拽过程中控制物体
    private LayerMask canPlace;//拖拽干员可放置的位置
    private GameObject chooseBackground;//选方向时的背景

    private bool isChoosingDirection=false;
    private bool isDragging=false;
    private Vector2 startPosition;
    private Vector2 endPosition;
    internal int whichIsChanged;

    private void Start()
    {
        //横竖屏设置
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
        shop = GameObject.Find("Shop").GetComponent<Shop>();
    }

    private void Update()
    {
        TouchOrMousehandler();
    }

    //暂停控制,绑定button
    public void PauseControl(bool pause)
    {
        Text text1 = GameObject.Find("Pause").transform.GetChild(0).GetComponent<Text>();
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            text1.text = "继续";
        }
        else
        {
            Time.timeScale = 1;
            text1.text = "暂停";
        }
    }

    //触摸和鼠标监听
    private void TouchOrMousehandler()
    {
        if (Input.touchCount >0)
        {
            Vector2 position = Input.touches[0].position;
            if (isChoosingDirection == false)
            {
                //放置干员
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        for (int i = 1; i <= PublicData.soilderNum; i++)
                        {
                            if (position.x-shop.images[i-1].GetComponent<RectTransform>().position.x>0 && position.x - shop.images[i - 1].GetComponent<RectTransform>().position.x < shop.size.x && position.y - shop.images[i - 1].GetComponent<RectTransform>().position.y > 0 && position.y - shop.images[i - 1].GetComponent<RectTransform>().position.y < shop.size.y)
                            {
                                if (Cost.cost >= shop.soilders[i - 1].GetComponent<Soilder>().cost && activeSoilders < Shop.level)
                                {
                                    whichIsChanged = i;
                                    dragSoilder = Instantiate(shop.soilders[i - 1]);
                                    if (dragSoilder.TryGetComponent<GroundSoilder>(out GroundSoilder ds1))
                                    {
                                        canPlace = LayerMask.GetMask(new string[1] { "standableground" });
                                    }
                                    if (dragSoilder.TryGetComponent<PlatformSoilder>(out PlatformSoilder ds2))
                                    {
                                        canPlace = LayerMask.GetMask(new string[1] { "standableplatform" });
                                    }
                                    dragSoilder.layer = LayerMask.NameToLayer("temporary");
                                    Ray ray = Camera.main.ScreenPointToRay(position);
                                    if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask(new string[3] { "standableground", "standableplatform", "unstandable" })))
                                    {
                                        dragSoilder.transform.position = new Vector3(hit.point.x, dragSoilder.transform.lossyScale.y / 2 + hit.point.y, hit.point.z);
                                    }
                                    isDragging = true;
                                }
                            }
                        }
                        break;
                    case TouchPhase.Moved:
                        if (isDragging)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(position);
                            if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask(new string[3] { "standableground", "standableplatform", "unstandable" })))
                            {
                                dragSoilder.transform.position = new Vector3(hit.point.x, dragSoilder.transform.lossyScale.y / 2 + hit.point.y, hit.point.z);
                            }
                        }
                        break;
                    case TouchPhase.Ended:
                        if (isDragging)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(position);
                            if (Physics.Raycast(ray, out RaycastHit hit, 1000,canPlace))
                            {
                                dragSoilder.transform.position = new Vector3(hit.transform.position.x, dragSoilder.transform.lossyScale.y / 2 + hit.transform.position.y, hit.transform.position.z);
                                chooseBackground = Instantiate(Resources.Load("UI/chooseDirection")) as GameObject;
                                chooseBackground.transform.SetParent(GameObject.Find("UICanvas").transform);
                                chooseBackground.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(dragSoilder.transform.position);
                                isChoosingDirection = true;
                             }
                            else
                            {
                                Destroy(dragSoilder);
                            }
                            isDragging = false;
                        }
                        break;
                }
            }
            else
            {
                //选择方向
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        PointerEventData data = new PointerEventData(EventSystem.current);
                        data.position = position;
                        List<RaycastResult> results = new List<RaycastResult>();
                        EventSystem.current.RaycastAll(data, results);
                        if (results.Count>0&&results[0].gameObject==chooseBackground)
                        {
                            startPosition = position;
                        }
                        else
                        {
                            Destroy(dragSoilder);
                            Destroy(chooseBackground);
                            isChoosingDirection = false;
                        }
                        break;
                    case TouchPhase.Ended:
                        endPosition = position;
                        Vector2 v = startPosition - endPosition;
                        if (System.Math.Abs(v.x) - System.Math.Abs(v.y)>0)
                        {
                            if (v.x > 20)
                            {
                                dragSoilder.GetComponent<Soilder>().direction = PublicData.Directions.left;
                            }
                            if (v.x < -20)
                            {
                                dragSoilder.GetComponent<Soilder>().direction = PublicData.Directions.right;
                            }
                            Refresh(false);
                            dragSoilder.GetComponent<Soilder>().isSetOver = true;
                            dragSoilder.layer = LayerMask.NameToLayer("soilder");
                            Cost.cost -= dragSoilder.GetComponent<Soilder>().cost;
                            activeSoilders++;
                        }
                        else if(System.Math.Abs(v.x) - System.Math.Abs(v.y)<0)
                        {
                            if (v.y > 20)
                            {
                                dragSoilder.GetComponent<Soilder>().direction = PublicData.Directions.down;
                            }
                            if (v.y < -20)
                            {
                                dragSoilder.GetComponent<Soilder>().direction = PublicData.Directions.up;
                            }
                            Refresh(false);
                            dragSoilder.GetComponent<Soilder>().isSetOver = true;
                            dragSoilder.layer = LayerMask.NameToLayer("soilder");
                            Cost.cost -= dragSoilder.GetComponent<Soilder>().cost;
                            activeSoilders++;
                        }
                        else
                        {
                            Destroy(dragSoilder);
                        }
                        Destroy(chooseBackground);
                        isChoosingDirection = false;
                        break;
                }
            }
        }
        //鼠标操作
        else 
        {
            Vector2 position = Input.mousePosition;
            if (isChoosingDirection == false)
            {
                //放置干员
                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 1; i <= PublicData.soilderNum; i++)
                    {
                        if (position.x - shop.images[i - 1].GetComponent<RectTransform>().position.x > 0 && position.x - shop.images[i - 1].GetComponent<RectTransform>().position.x < shop.size.x && position.y - shop.images[i - 1].GetComponent<RectTransform>().position.y > 0 && position.y - shop.images[i - 1].GetComponent<RectTransform>().position.y < shop.size.y)
                        {
                            if (Cost.cost >= shop.soilders[i - 1].GetComponent<Soilder>().cost && activeSoilders < Shop.level)
                            {
                                whichIsChanged = i;
                                dragSoilder = Instantiate(shop.soilders[i - 1]);
                                if (dragSoilder.TryGetComponent<GroundSoilder>(out GroundSoilder ds1))
                                {
                                    canPlace = LayerMask.GetMask(new string[1] { "standableground" });
                                }
                                if (dragSoilder.TryGetComponent<PlatformSoilder>(out PlatformSoilder ds2))
                                {
                                    canPlace = LayerMask.GetMask(new string[1] { "standableplatform" });
                                }
                                dragSoilder.layer = LayerMask.NameToLayer("temporary");
                                Ray ray = Camera.main.ScreenPointToRay(position);
                                if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask(new string[3] { "standableground", "standableplatform", "unstandable" })))
                                {
                                    dragSoilder.transform.position = new Vector3(hit.point.x, dragSoilder.transform.lossyScale.y / 2 + hit.point.y, hit.point.z);
                                }
                                isDragging = true;
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (isDragging)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(position);
                        if (Physics.Raycast(ray, out RaycastHit hit, 1000, canPlace))
                        {
                            dragSoilder.transform.position = new Vector3(hit.transform.position.x, dragSoilder.transform.lossyScale.y / 2 + hit.transform.position.y, hit.transform.position.z);
                            chooseBackground = Instantiate(Resources.Load("UI/chooseDirection")) as GameObject;
                            chooseBackground.transform.SetParent(GameObject.Find("UICanvas").transform);
                            chooseBackground.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(dragSoilder.transform.position);
                            isChoosingDirection = true;
                        }
                        else
                        {
                            Destroy(dragSoilder);
                        }
                        isDragging = false;
                    }
                }
                else
                {
                    if (isDragging)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(position);
                        if (Physics.Raycast(ray, out RaycastHit hit, 1000, LayerMask.GetMask(new string[3] { "standableground", "standableplatform", "unstandable" })))
                        {
                            dragSoilder.transform.position = new Vector3(hit.point.x, dragSoilder.transform.lossyScale.y / 2 + hit.point.y, hit.point.z);
                        }
                    }
                }
            }
            else
            {
                //选择方向
                if (Input.GetMouseButtonDown(0))
                {
                    PointerEventData data = new PointerEventData(EventSystem.current);
                    data.position = position;
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(data, results);
                    if (results.Count > 0 && results[0].gameObject == chooseBackground)
                    {
                        startPosition = position;
                    }
                    else
                    {
                        Destroy(dragSoilder);
                        Destroy(chooseBackground);
                        isChoosingDirection = false;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    endPosition = position;
                    Vector2 v = startPosition - endPosition;
                    if (System.Math.Abs(v.x) - System.Math.Abs(v.y) > 0)
                    {
                        if (v.x > 20)
                        {
                            dragSoilder.GetComponent<Soilder>().direction = PublicData.Directions.left;
                        }
                        if (v.x < -20)
                        {
                            dragSoilder.GetComponent<Soilder>().direction = PublicData.Directions.right;
                        }
                        Refresh(false);
                        dragSoilder.GetComponent<Soilder>().isSetOver = true;
                        dragSoilder.layer = LayerMask.NameToLayer("soilder");
                        Cost.cost -= dragSoilder.GetComponent<Soilder>().cost;
                        activeSoilders++;
                    }
                    else if (System.Math.Abs(v.x) - System.Math.Abs(v.y) < 0)
                    {
                        if (v.y > 20)
                        {
                            dragSoilder.GetComponent<Soilder>().direction = PublicData.Directions.down;
                        }
                        if (v.y < -20)
                        {
                            dragSoilder.GetComponent<Soilder>().direction = PublicData.Directions.up;
                        }
                        Refresh(false);
                        dragSoilder.GetComponent<Soilder>().isSetOver = true;
                        dragSoilder.layer = LayerMask.NameToLayer("soilder");
                        Cost.cost -= dragSoilder.GetComponent<Soilder>().cost;
                        activeSoilders++;
                    }
                    else
                    {
                        Destroy(dragSoilder);
                    }
                    Destroy(chooseBackground);
                    isChoosingDirection = false;
                }
            }
        }
    }

    //刷新商店UI
    public void Refresh(bool isadd)
    {
        if (isadd)
        {
            shop.images[whichIsChanged - 1].SetActive(true);
            for(int i = whichIsChanged+1; i <= PublicData.soilderNum; i++)
            {
                shop.images[i - 1].transform.position = new Vector2(shop.images[i - 1].transform.position.x + shop.size.x*1.1f, shop.images[i - 1].transform.position.y);
            }
        }
        else
        {
            shop.images[whichIsChanged - 1].SetActive(false);
            for (int i = whichIsChanged + 1; i <= PublicData.soilderNum; i++)
            {
                shop.images[i - 1].transform.position = new Vector2(shop.images[i - 1].transform.position.x - shop.size.x*1.1f, shop.images[i - 1].transform.position.y);
            }
        }
    }
}

