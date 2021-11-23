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

    public List<Node> GetNeighbours(Node node)  //������Ƽ�� �������� ������ ���� �� �̵� ������ ��带 ��ȯ
    {
        List<Node> neighbours = new List<Node>();
        int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } }; //���� ��带 �������� ���� �¿�
                                                                    // 0    1
                                                                    // 1    0
                                                                    // 0   -1
                                                                    //-1    0
        bool[] walkableUDLR = new bool[4];  // �̵����������� ���ε� �����¿�� 4���� �غ��Ѵ�

        //�����¿��� ��� ���� ���
        for (int i = 0; i < 4; i++)
        {
            int checkX = node.gridX + temp[i, 0];
            int checkY = node.gridY + temp[i, 1];
            if (checkX >= 0 && checkX < (int)GridWorldSizeX && checkY >= 0 && checkY < (int)GridWorldSizeY)
            {  //�˻� ��尡 ��ü ���ȿ� ��ġ�� �ִ��� �˻�

                if (Grid.gridInstance.gridindex(checkX, checkY).walkable) walkableUDLR[i] = true; //�ش� ��忡 ��ֹ��� ���� �� �� �ִٸ� �ش� bool ���� true�� ����
                //��ֹ��� �ִ� ����� false�� �߰�

                neighbours.Add(Grid.gridInstance.gridindex(checkX, checkY));   //�̿� ��忡 �߰�
            }
        }

        //�밢���� ��带 ���
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
        //�̿����� ���� List�� ��ȯ
    }

    //====================================================
   
}
