using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject line;
    LineRenderer lineRander;

    public GameObject Player;

    Grid grid;
    public Node start; 
     public Node end;


    public bool finding;

    private void Awake()
    {
       //lineRander = line.GetComponent<LineRenderer>();
        grid = GetComponent<Grid>();
        StartGrid();
        
    }

    public Camera mainCamera;
    public void Update()
    {
        
    }

    public void StartGrid()
    {
        StopCoroutine("FindPath");
       // line.SetActive(false);
        finding = false;

        bool success = grid.CreateGrid();// 게임 시작시 맵에 타일 생성

        if (success)        // 생성이 끝나면 처음 시작과 끝이 될 노드를 설정
        {
            start = grid.StartNode;
          //  end = grid.EndNode;
            start.ChangeStart = true;
         //   end.ChangeEnd = true;
        }
    }

    public void StartFinding(bool search)
    {
        StopCoroutine("FindPath");
      //  line.SetActive(false);
        finding = false;
        //grid.ResetGrid();
        if(search) StartCoroutine("FindPath");        
    }

    IEnumerator FindPath()
    {
        finding = true;
        bool pathSuccess = false;        

        List<Node> openSet = new List<Node>(); 
        HashSet<Node> closedSet = new HashSet<Node>(); //Close
        openSet.Add(start); //Open은 Start지점의 노드를 저장

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0]; // CurrentNode은 처음 노드부터

            //Open에 fCost가 가장 작은 노드를 찾기
            for(int i = 1; i<openSet.Count; i++)
            { //0은 시작 노드이기 때문에 i는 1부터 시작
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {   
                    //
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // 현재 노드가 목적지면 while문 탈출
            if (currentNode == end)
            {
                pathSuccess = true;
                break;
            }

            yield return new WaitUntil(() => finding);
            yield return new WaitForSeconds(0.1f);

            if (currentNode != start)
                currentNode.ChangeColor = Color.Lerp(Color.cyan, Color.white, 0.2f);


            //이웃 노드를 검색
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                //이동불가 노드 이거나 이미 검색한 노드는 제외
                if (!neighbour.walkable  || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, end);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                        if (neighbour.walkable && !neighbour.end)
                            neighbour.ChangeColor = Color.Lerp(Color.green, Color.white, 0.2f);
                    }
                }
            }
        }
        if (pathSuccess)
        {
            //DrawingLine(RetracePath(start, end));


        }
        finding = false;
    }

    //도착지점 부터 출발지점까지 부모 노드를 입력하여 웨이포인트를 생성

    //여기서 게임오브젝트의 이동경로를 웨이 포인트로 지정해줘야 할 듯
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    //반복되는 이동을 삭제해주며 웨이포인트를 간단하게 만든다.
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].ground.transform.position + Vector3.up * 0.1f);
            }
            directionOld = directionNew;
        }
        waypoints.Add(start.ground.transform.position + Vector3.up * 0.1f);
        return waypoints.ToArray();
    }

    public void DrawingLine(Vector3[] waypoints)
    {
        line.SetActive(true);
        lineRander.positionCount = waypoints.Length;
        for (int i = 0; i < waypoints.Length; i++)
        {
           // lineRander.SetPosition(i, waypoints[i]);
            
        }
    }

    //노드간의 거리를 계산
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
