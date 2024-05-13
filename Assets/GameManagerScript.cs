using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //配列の宣言
    public GameObject playerPrefab; //ゲームオブジェクト生成
    public GameObject BoxPrefab; //ゲームオブジェクト生成
    public GameObject goalPrefab; // レベルデザイン用の配列
    public GameObject clearText; // レベルデザイン用の配列
    int[,] map; //レベルデザイン用配列
    GameObject[,] field; //ゲーム管理用の配列

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

    //移動
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {

        //縦軸横軸の配列外を参照していないか
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }

        //Boxタグを持っていたら再帰処理
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int veloctiy = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + veloctiy);
            if (!success) { return false; }

        }

        //プレイヤー　箱関わらずの移動処理
        //GameObjectの座標(position)を移動させてからインデックスの入れ替え
        field[moveFrom.y, moveFrom.x].transform.position =
            IndexToPosition(moveTo);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }


    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0;x < map.GetLength(1); x++)
            {
                // 格納場所か否かを判定
                if (map[y,x] == 3)
                {
                    // 格納場所インデックスを押さえておく
                    goals.Add(new Vector2Int(x,y));
                }
            }
        }

        //　要素数はgoals.Countで取得
        for(int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                //一つでも箱がなかったら
                return false;
            }
        }
        //　条件未達成でなければ条件達成
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
                    //GameObject instance = // 書き換え //
                             field[y,x] = 
                        Instantiate(
                        playerPrefab,
                        /*new Vector3(x,map.GetLength(0) -y,0)*/ IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 2)
                {
                    //GameObject instance = // 書き換え //
                         field[y,x] = 
                        Instantiate(
                        BoxPrefab,
                         /* new Vector3(x, map.GetLength(0) - y, 0)*/IndexToPosition(new Vector2Int(x, y)),
                        Quaternion.identity
                    );
                }               
                if (map[y, x] == 3)
                {
                    //GameObject instance = // 書き換え //
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
            //ここに移動処理を書く
            //1.をここから記述
            //見つからなかった時のために-1で初期化
            Vector2Int playerIndex = GetPlayerIndex(); 
            //移動処理を関数化
            MoveNumber("Player", playerIndex, 
                playerIndex + new Vector2Int(1,0)
                );
            // もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
                //Debug.Log("clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //ここに移動処理を書く
            //1.をここから記述
            //見つからなかった時のために-1で初期化
            Vector2Int playerIndex = GetPlayerIndex();
            //移動処理を関数化
            MoveNumber("Player", playerIndex, 
                playerIndex + new Vector2Int(-1, 0)
                );
            // もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
                //Debug.Log("clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //ここに移動処理を書く
            //1.をここから記述
            //見つからなかった時のために-1で初期化
            Vector2Int playerIndex = GetPlayerIndex();
            //移動処理を関数化
            MoveNumber("Player", playerIndex,
                playerIndex + new Vector2Int(0, 1)
                );
            // もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
                //Debug.Log("clear");
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //ここに移動処理を書く
            //1.をここから記述
            //見つからなかった時のために-1で初期化
            Vector2Int playerIndex = GetPlayerIndex();
            //移動処理を関数化
            MoveNumber("Player", playerIndex,
                playerIndex + new Vector2Int(0, -1)
                );
            // もしクリアしていたら
            if (IsCleard())
            {
                //ゲームオブジェクトのSetActiveメソッドを使い有効化
                clearText.SetActive(true);
                //Debug.Log("clear");
            }
        }
    }
}

