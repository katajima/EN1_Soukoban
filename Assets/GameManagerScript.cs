using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //配列の宣言
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
    //            //要素数を一つずつ出力
    //            //Debug.Log(map[i] + ",");
    //        }
    //        debugText += "\n";//改行
    //    }
    //    //結合した文字列を出力
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
    //    //移動先に2(箱)が居たら
    //    if (map[moveTo] == 2)
    //    {
    //        //どの方向に移動するかを算出
    //        int veloctiy = moveTo - moveFrom;
    //        //プレイヤーの移動先から、さらに先へ2(箱)を移動させる
    //        //箱の移動処理、MoveNomberメソッド内でMoveNomberメソッドを
    //        //呼び、処理が再帰している、移動可不可をboolで記録
    //        bool success = MoveNumber(2, moveTo, moveTo + veloctiy);
    //        //もし箱が移動失敗したら、プレイヤーの移動も失敗
    //        if (!success) { return false; }
    //    }
    //    //プレイヤー　箱関わらずの移動処理
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
        //追加、文字列の宣言と初期化
        string debugText = "";
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y, x].ToString() + ",";
            }
            debugText += "\n";//改行
        }
        //結合した文字列を出力
        Debug.Log(debugText);
        //PrintArry();
    }

    // Update is called once per frame
    //void Update()
    //{

    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        //ここに移動処理を書く
    //        //1.をここから記述
    //        //見つからなかった時のために-1で初期化
    //        int playerIndex = GetPlayerIndex();
    //        //移動処理を関数化
    //        MoveNumber(1, playerIndex, playerIndex + 1);
    //        PrintArry();
    //    }
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        //ここに移動処理を書く
    //        //1.をここから記述
    //        //見つからなかった時のために-1で初期化
    //        int playerIndex = GetPlayerIndex();
    //        //移動処理を関数化
    //        MoveNumber(1, playerIndex, playerIndex - 1);
    //        PrintArry();
    //    }
    //}
}

