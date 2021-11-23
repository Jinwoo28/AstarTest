using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public GameObject ground;  //노드를 저장하고 있는 게임오브젝트
    public bool walkable;   //이동 가능한 지역인지 판단하는 변수
    public int gridX;   //노드의 x축 번호
    public int gridY;   // 노드의 y축 번호
       
    public bool start;  //시작부분
    public bool end;    //도착부분

    public int gCost;   // 출발지점으로 부터 현재노드까지의 경로값
    public int hCost;   // 현재 노드부터 목적지까지의 경로값
    public Node parent; // 현재 노드의 부모(바로 전 노드)

    public Node(GameObject _ground, bool _walkable, int _gridX, int _gridY)
    {   
        //생성자
        ground = _ground;
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }

        //정보은닉
        //private 으로 선언된 데이터를 외부에서 사용할 수 있게 만드는 것
        //프로퍼티 : 코드의 은닉성과 구현의 편리성을 제공
        // get : 필드로부터 값을 읽어옴
        // set : 필드에 값을 할당, 암묵적 매개변수 value사용

        //Example
//class testClass
//    {
//        // 멤버변수
//        private int data;

//        // 함수사용
//        public int getData()
//        {
//            return data;
//        }
//        public void setData(int data)
//        {
//            this.data = data;
//        }

//        // 프로퍼티
//        public int Data
//        {
//            get
//            {
//                return data;
//            }
//            set
//            {
//                // value는 암묵적 매개변수로 선언 없이 사용 가능
//                data = value;
//            }
//        }

//        test.setData(4);

//        test.Data = 7;

    public int fCost
    {
        get{ return gCost + hCost; }
    }

    public bool ChangeNode
    {   
        // 선택된 노드의 색을 바꾸는 함수_ 장애물인지 아닌지
                set
        {
            Color color = value ? Color.white : Color.gray;
            walkable = value;
            ChangeColor = color;                       
        }
    }


    public bool ChangeStart // 스타트 지점을 마우스로 옮길 때 사용
    {
        set
        {
            if (value)
            {
                start = value;
                ChangeColor = Color.Lerp(Color.blue, Color.white, 0.2f);
            }
            else
            {
                start = value;
                ChangeNode = walkable;
            }
        }
    }
    public bool ChangeEnd 
    {
        set
        {
            if (value)      //외부에서 함수를 호출할 때 true면 빨간색으로 바꺼줌
            {
                end = value;
                ChangeColor = Color.Lerp(Color.red, Color.white, 0.2f);
            }
            else
            {
                end = value;
                ChangeNode = walkable;      //펄스면 장애물인지 아닌지 판단 후 원래 색으로 변경
            }
        }
    }
    public Color ChangeColor 
    {
        set{ground.GetComponent<MeshRenderer>().material.color = value;}
    }
}
