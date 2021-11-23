using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{ 
    Grid grid; //그리드 형식의 변수 선언 마우스 포인트(목적지)의 노드를 저장
    Main main;  //길찾기의 시작과 끝 지점을 지정하기 위한 main변수

    int settingButton = 1;

    bool pauseBut;

    private void Awake()
    {
        main = GetComponent<Main>();
        grid = GetComponent<Grid>();
    }
    void Update()
    {
         SetObstacle();
        //마우스 클릭 시 노드의 장애물 값을 true, false로 변경
        SetTarget();
    }

    public void SetObstacle()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            Node node = RayCast();
            if (node != null) //마우스 포인트에 닿은 것이 널이 아니라면 return
            {
                if(node.start || node.end)      // Ray에 맞은 노드가 시작, 끝인지 판단 시작지점이거나 끝 지점이면 시작/끝의 위치를 변경할 수 있다.
                    StartCoroutine("SwitchStartEnd", node);
                else
                    StartCoroutine("ChangeWalkable", node); //시작이나 끝이 아니라면 장애물로 변경
            }
            return;
        }

    }

    public void SetTarget()
    {
        //기존의 다른 end노드를 없애고 마우스로 찍은 노드로 변경_ 아직 구현 안함
        if (Input.GetMouseButtonDown(1))
        {
            Node node = RayCast();
            Node oldnode = main.end;
            if(node != null)
            {
                if (oldnode != null)
                {
                    oldnode.ChangeEnd = false;
                    oldnode = null;
                }
                node.ChangeEnd = true;
                main.end = node;

                main.StartFinding(true);
            }
        }
    }

    IEnumerator SwitchStartEnd(Node node)   // 드래그로 스타트 엔드 위치 변경
    {
        bool start = node.start;
        Node nodeOld = node;
       
        while (Input.GetMouseButton(0))
        {
            node = RayCast();
            if (node != null && node != nodeOld)
            {
                if (start && !node.end)
                {
                    node.ChangeStart = true;
                    main.start = node;
                    nodeOld.ChangeStart = false;
                    nodeOld = node;
                }
                else if (!start && !node.start)
                {
                    node.ChangeEnd = true;
                    main.end = node;
                    nodeOld.ChangeEnd = false;
                    nodeOld = node;
                }
            }
            yield return null;
        }
    }

    IEnumerator ChangeWalkable(Node node)
    {
        bool walkable = !node.walkable; // 현재 불값을 반대로 변환

        while (Input.GetMouseButton(0)) //마우스 버튼을 누르는 동안 계속 실행
        {
            node = RayCast();
            if (node != null && !node.start && !node.end) //해당 노드가 있어야 하고, 시작과 끝점이 아닐 때 실행
            {
                node.ChangeNode = walkable;
            }
            yield return null;
        }
    }

    

    public Node RayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //카메라를 기준으로 마우스 위치로 Ray발사
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            GameObject obj = hit.collider.gameObject; //맞은 hit의 정보를 반환
            Debug.Log(obj.name);
            Debug.Log(obj.transform.position);
            return grid.NodePoint(obj.transform.position);  // 선택한 노드의 x,y 값으로 grid[x,y]를 찾음
        }
        return null; // 맞은 collider가 없으면 null 반환
    }
    //void ReconstructionGrid(int windowId)
    //{
    //    string value;

    //    GUIStyle textStyle = new GUIStyle("TextField");
    //    GUILayout.BeginVertical("box");
    //    GUILayout.Label("Size X : 2 ~ 100");
    //    GUILayout.BeginHorizontal();
    //    GUILayout.Label("Size X", GUILayout.MinWidth(50));
    //    value = GUILayout.TextField(grid.gridWorldSize.x.ToString(), textStyle, GUILayout.MinWidth(50));
    //    grid.gridWorldSize.x = int.Parse(value);
    //    GUILayout.EndHorizontal();
    //    GUILayout.Label("Size Y : 2 ~ 50");
    //    GUILayout.BeginHorizontal();
    //    GUILayout.Label("Size Y", GUILayout.MinWidth(50));
    //    value = GUILayout.TextField(grid.gridWorldSize.y.ToString(), textStyle, GUILayout.MinWidth(50));
    //    grid.gridWorldSize.y = int.Parse(value);
    //    GUILayout.EndHorizontal();
    //    GUILayout.EndVertical();
    //    if (GUILayout.Button("Change Grid"))
    //    {
    //        main.StartGrid();
    //        pauseBut = false;
    //    }
    //    if (!main.finding && pauseBut)
    //    {
    //        if (GUILayout.Button("Resume Search"))
    //        {
    //            main.finding = true;
    //            pauseBut = false;
    //        }
    //    }
    //    else
    //    {
    //        if (GUILayout.Button("Start Search"))
    //        {
    //            main.StartFinding(true);
    //        }
    //    }
    //    if (pauseBut)
    //    {
    //        if (GUILayout.Button("Cancel Search"))
    //        {
    //            pauseBut = false;
    //            main.StartFinding(false);
    //        }
    //    }
    //    else
    //    {
    //        if (GUILayout.Button("Pause Search"))
    //        {
    //            if (main.finding)
    //            {
    //                pauseBut = true;
    //                main.finding = false;
    //            }
    //        }
    //    }

    //}
    //private void OnGUI()
    //{

    //    if (GUILayout.Button("Menu"))
    //    {
    //        settingButton *= -1;
    //    }
    //    if (settingButton == 1)
    //    {
    //        GUILayout.Window(0, new Rect(10, 30, 2, 2), ReconstructionGrid, "Settings");
    //    }
    //}
}
