using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //�z��̐錾
    public GameObject playerPrefab;
    int[,] map;

    //void PrintArry()
    //{
    //    string debugText = "";
    //    for (int y = 0; y < map.Length; y++)
    //    {
    //        for (int x = 0; x < map.Length; x++)
    //        {
    //            debugText += map[y,x].ToString() + ",";
    //            //�v�f��������o��
    //            //Debug.Log(map[i] + ",");
    //        }
    //        debugText += "\n";//���s
    //    }
    //    //����������������o��
    //    Debug.Log(debugText);
    //}

    //int GetPlayerIndex()
    //{
    //    for (int i = 0; i < map.Length; i++)
    //    {
    //        if (map[i] == 1)
    //        {
    //            return i;
    //        }
    //    }
    //    return -1;
    //}

    //bool MoveNumber(int number, int moveFrom, int moveTo)
    //{
    //    if (moveTo < 0 || moveTo >= map.Length){return false;}
    //    //�ړ����2(��)��������
    //    if (map[moveTo] == 2)
    //    {
    //        //�ǂ̕����Ɉړ����邩���Z�o
    //        int veloctiy = moveTo - moveFrom;
    //        //�v���C���[�̈ړ��悩��A����ɐ��2(��)���ړ�������
    //        //���̈ړ������AMoveNomber���\�b�h����MoveNomber���\�b�h��
    //        //�ĂсA�������ċA���Ă���A�ړ��s��bool�ŋL�^
    //        bool success = MoveNumber(2, moveTo, moveTo + veloctiy);
    //        //���������ړ����s������A�v���C���[�̈ړ������s
    //        if (!success) { return false; }
    //    }
    //    //�v���C���[�@���ւ�炸�̈ړ�����
    //    map[moveTo] = number;
    //    map[moveFrom] = 0;
    //    return true;
    //}

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,]
        {
            {0,0,0,0,0},
            {0,0,1,0,0},
            {0,0,0,0,0},
        };
        //�ǉ��A������̐錾�Ə�����
        string debugText = "";
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";
            }
            debugText += "\n";//���s
        }
        //����������������o��
        Debug.Log(debugText);
        //PrintArry();
    }

    // Update is called once per frame
    //void Update()
    //{

    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        //�����Ɉړ�����������
    //        //1.����������L�q
    //        //������Ȃ��������̂��߂�-1�ŏ�����
    //        int playerIndex = GetPlayerIndex();
    //        //�ړ��������֐���
    //        MoveNumber(1, playerIndex, playerIndex + 1);
    //        PrintArry();
    //    }
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        //�����Ɉړ�����������
    //        //1.����������L�q
    //        //������Ȃ��������̂��߂�-1�ŏ�����
    //        int playerIndex = GetPlayerIndex();
    //        //�ړ��������֐���
    //        MoveNumber(1, playerIndex, playerIndex - 1);
    //        PrintArry();
    //    }
    //}
}

