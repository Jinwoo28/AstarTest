using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject groundPrefab; //바닥을 이룰 게임으보젝트(정사각형의 Quad 사용)
    GameObject parentGrid;  //groundPrefab의 부모

    public Vector2 gridWorldSize; //전체 노드의 개수(크기)

    Node[,] grid;

    public bool CreateGrid()
    {
        if (gridWorldSize.x < 2 || gridWorldSize.x > 101 || gridWorldSize.y < 2 || gridWorldSize.y > 51) //노드의 최대크기 제한
            return false;

        //타일 개수에 따른 카메라의 위치
        float cameraY = gridWorldSize.x * 0.42f > gridWorldSize.y * 0.87f ? gridWorldSize.x * 0.42f : gridWorldSize.y * 0.87f;
        transform.position = new Vector3(0, cameraY, 0);

        //노드들을 담을 빈게임오브젝트를 하나 생성
        if (parentGrid != null)
            Destroy(parentGrid);
        parentGrid = new GameObject("parentGrid"); //parentGrid라는 이름으로 하이라키에서 생성
        //이미 존재한다면 Destroy


        grid = new Node[(int)gridWorldSize.x, (int)gridWorldSize.y]; // 2차원 배열 grid의 최대 index설정 
        //이해가 좀 더 필요
        
        Vector3 worldBottomLeft = Vector3.zero - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        //노드를 중심을 기준으로 왼쪽아래부터 만들기

        //노드 생성
        for (int x = 0; x < (int)gridWorldSize.x; x++)
        {
            for (int y = 0; y < (int)gridWorldSize.y; y++)
            {
//                Vector3 worldPoint = worldBottomLeft + Vector3.right * x + Vector3.forward * y;
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x + 0.5f) + Vector3.forward * (y + 0.5f);   //0을 기준으로 좌, 우가 같은 index값을 가지기 위해 0.5씩 더한다.
                GameObject obj = Instantiate(groundPrefab, worldPoint, Quaternion.Euler(90, 0, 0));
                obj.transform.parent = parentGrid.transform;
                grid[x, y] = new Node(obj, true, x, y);
                //생성과 함께 노드의 정보 입력
            }
        }

        return true;
    }

    //public void ResetGrid()
    //{
    //    for (int x = 0; x < (int)gridWorldSize.x; x++)
    //    {
    //        for (int y = 0; y < (int)gridWorldSize.y; y++)
    //        {
    //            if (grid[x, y].walkable && !grid[x, y].start && !grid[x,y].end)
    //            {
    //                grid[x, y].ChangeNode = true;
    //            }
    //        }
    //    }
    //}

    private void Start()
    {


    }

    public List<Node> GetNeighbours(Node node)  //프로퍼티를 기준으로 주위의 노드들 중 이동 가능한 노드를 반환
    {
        List<Node> neighbours = new List<Node>();
        int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } }; //현재 노드를 기준으로 상하 좌우
       // 0    1
       // 1    0
       // 0   -1
       //-1    0
        bool[] walkableUDLR = new bool[4];  // 이동가능한지의 여부도 상하좌우로 4개씩 준비한다
        
        //상하좌우의 노드 먼저 계산
        for (int i = 0; i < 4; i++)
        {
            int checkX = node.gridX + temp[i, 0]; 
            int checkY = node.gridY + temp[i, 1];
            if (checkX >= 0 && checkX < (int)gridWorldSize.x && checkY >= 0 && checkY < (int)gridWorldSize.y)
            {  //검사 노드가 전체 노드안에 위치해 있는지 검사
              
                if (grid[checkX, checkY].walkable) walkableUDLR[i] = true; //해당 노드에 장애물이 없어 갈 수 있다면 해당 bool 값을 true로 변경
                //장애물이 있는 노드라면 false로 추가

                neighbours.Add(grid[checkX, checkY]);   //이웃 노드에 추가
            }
        }

        //대각선의 노드를 계산
        for (int i = 0; i < 4; i++)
        {
            if (walkableUDLR[i] || walkableUDLR[(i + 1) % 4])
            {
                int checkX = node.gridX + temp[i, 0] + temp[(i + 1) % 4, 0];
                int checkY = node.gridY + temp[i, 1] + temp[(i + 1) % 4, 1];
                if (checkX >= 0 && checkX < (int)gridWorldSize.x && checkY >= 0 && checkY < (int)gridWorldSize.y)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
        //이웃으로 넣은 List를 반환
    }

    public Node NodePoint(Vector3 rayPosition)
    {
        //레이에 맞은 곳의 node를 찾아 반환하기 위한 함수
        int x = (int)(rayPosition.x + gridWorldSize.x / 2);
        int y = (int)(rayPosition.z + gridWorldSize.y / 2);

        return grid[x, y];
    }

    public Node StartNode
    {
        get
        {
            grid[0, 0].start = true;
            return grid[0, 0];
        }
    }
    public Node EndNode
    {
        get
        {
            grid[(int)gridWorldSize.x - 1, (int)gridWorldSize.y - 1].end = true;
            return grid[(int)gridWorldSize.x - 1, (int)gridWorldSize.y - 1];
        }
    }
}
