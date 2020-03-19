using UnityEngine;

public class Route : MonoBehaviour
{
    public static Transform[][] routes;//静态交错数组存储所有路线
    private int routeNum;//路线数
    private Transform[] waypoints;//一条路线的路径点

    //从场景中读取路线与路径点存入静态数组
    private void Start()
    {
        routeNum = transform.childCount;
        routes = new Transform[routeNum][];
        for (int i = 1; i <= routeNum; i++)
        {
            Transform route = transform.GetChild(i-1);
            waypoints = new Transform[route.childCount];
            for (int t = 1; t<= waypoints.Length; t++)
            {
                waypoints[t - 1] = route.GetChild(t - 1);
            }
            routes[i - 1] = waypoints;
        }
    }
}
