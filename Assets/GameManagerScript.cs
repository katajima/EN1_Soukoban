using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //�z��̐錾
    public GameObject playerPrefab; //�Q�[���I�u�W�F�N�g����
    public GameObject BoxPrefab; //�Q�[���I�u�W�F�N�g����
    public GameObject goalPrefab; // ���x���f�U�C���p�̔z��
    public GameObject clearText; // ���x���f�U�C���p�̔z��
    int[,] map; //���x���f�U�C���p�z��
    GameObject[,] field; //�Q�[���Ǘ��p�̔z��

    Vector3 IndexToPosition(Vector2Int index)
    {
        return new Vector3(index.x - map.GetLength(1) / 2 + 0.5f,index.y -map.GetLength(0) /2,0);
    }

    Vector2Int GetPlayerIndex()
    {


        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null) { continue; }
                if (field[y, x].tag == "Player") { return new Vector2Int(x, y); }

            }
        }
        return new Vector2Int(-1, -1);
    }

    //�ړ�
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {

        //�c�������̔z��O���Q�Ƃ��Ă��Ȃ���
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }

        //Box�^�O�������Ă�����ċA����
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int veloctiy = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + veloctiy);
            if (!success) { return false; }

        }

        //�v���C���[�@���ւ�炸�̈ړ�����
        //GameObject�̍��W(position)���ړ������Ă���C���f�b�N�X�̓���ւ�
        field[moveFrom.y, moveFrom.x].transform.position =
            IndexToPosition(moveTo);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }


    bool IsCleard()
    {
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0;x < map.GetLength(1); x++)
            {
                // �i�[�ꏊ���ۂ��𔻒�
                if (map[y,x] == 3)
                {
                    // �i�[�ꏊ�C���f�b�N�X���������Ă���
                    goals.Add(new Vector2Int(x,y));
                }
            }
        }

        //�@�v�f����goals.Count�Ŏ擾
        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                //��ł������Ȃ�������
                return false;
            }
        }
        //�@�������B���łȂ���Ώ����B��
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {



        map = new int[,]
        {
            {2,2,2,2,2,2,2,2,2},
            {2,0,0,0,0,0,0,0,2},
            {2,0,2,3,1,3,2,0,2},
            {2,0,2,3,0,3,2,0,2},
            {2,0,0,0,3,0,0,0,2},
            {2,0,0,0,2,0,0,0,2},
            {2,0,0,0,0,0,0,0,2},       
            {2,2,2,2,2,2,2,2,2},
       
        };
        field = new GameObject
        [
        map.GetLength(0),
        map.GetLength(1)
        ];


        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    //GameObject instance = // �������� //
                             field[y,x] = 
                        Instantiate(
                        playerPrefab,
                        /*new Vector3(x,map.GetLength(0) -y,0)*/ IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 2)
                {
                    //GameObject instance = // �������� //
                         field[y,x] = 
                        Instantiate(
                        BoxPrefab,
                         /* new Vector3(x, map.GetLength(0) - y, 0)*/IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                    );
                }               
                if (map[y, x] == 3)
                {
                    //GameObject instance = // �������� //
                         field[y,x] = 
                        Instantiate(
                        goalPrefab,
                         /* new Vector3(x, map.GetLength(0) - y, 0)*/IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                    );
                }
            }
        }

    }

    //Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //�����Ɉړ�����������
            //1.����������L�q
            //������Ȃ��������̂��߂�-1�ŏ�����
            Vector2Int playerIndex = GetPlayerIndex(); 
            //�ړ��������֐���
            MoveNumber("Player", playerIndex, 
                playerIndex + new Vector2Int(1,0)
                );
            // �����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
                //Debug.Log("clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //�����Ɉړ�����������
            //1.����������L�q
            //������Ȃ��������̂��߂�-1�ŏ�����
            Vector2Int playerIndex = GetPlayerIndex();
            //�ړ��������֐���
            MoveNumber("Player", playerIndex, 
                playerIndex + new Vector2Int(-1, 0)
                );
            // �����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
                //Debug.Log("clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //�����Ɉړ�����������
            //1.����������L�q
            //������Ȃ��������̂��߂�-1�ŏ�����
            Vector2Int playerIndex = GetPlayerIndex();
            //�ړ��������֐���
            MoveNumber("Player", playerIndex,
                playerIndex + new Vector2Int(0, 1)
                );
            // �����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
                //Debug.Log("clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //�����Ɉړ�����������
            //1.����������L�q
            //������Ȃ��������̂��߂�-1�ŏ�����
            Vector2Int playerIndex = GetPlayerIndex();
            //�ړ��������֐���
            MoveNumber("Player", playerIndex,
                playerIndex + new Vector2Int(0, -1)
                );
            // �����N���A���Ă�����
            if (IsCleard())
            {
                //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
                clearText.SetActive(true);
                //Debug.Log("clear");
            }
        }
    }
}

