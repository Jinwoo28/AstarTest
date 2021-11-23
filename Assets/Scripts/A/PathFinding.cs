using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    float GridWorldSizeX = 0;
    float GridWorldSizeY = 0;

    private void Awake()
    {
        GridWorldSizeX = Grid.gridInstance.GridWorldSizeX;
        GridWorldSizeY = Grid.gridInstance.GridWorldSizeY;
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
            if (checkX >= 0 && checkX < (int)GridWorldSizeX && checkY >= 0 && checkY < (int)GridWorldSizeY)
            {  //검사 노드가 전체 노드안에 위치해 있는지 검사

                if (Grid.gridInstance.gridindex(checkX, checkY).walkable) walkableUDLR[i] = true; //해당 노드에 장애물이 없어 갈 수 있다면 해당 bool 값을 true로 변경
                //장애물이 있는 노드라면 false로 추가

                neighbours.Add(Grid.gridInstance.gridindex(checkX, checkY));   //이웃 노드에 추가
            }
        }

        //대각선의 노드를 계산
        for (int i = 0; i < 4; i++)
        {
            if (walkableUDLR[i] || walkableUDLR[(i + 1) % 4])
            {
                int checkX = node.gridX + temp[i, 0] + temp[(i + 1) % 4, 0];
                int checkY = node.gridY + temp[i, 1] + temp[(i + 1) % 4, 1];
                if (checkX >= 0 && checkX < (int)GridWorldSizeX && checkY >= 0 && checkY < (int)GridWorldSizeY)
                {
                    neighbours.Add(Grid.gridInstance.gridindex(checkX, checkY));
                }
            }
        }

        return neighbours;
        //이웃으로 넣은 List를 반환
    }

    //====================================================
   
}
