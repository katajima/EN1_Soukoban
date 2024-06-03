using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;//プレイヤー
    public GameObject BoxPrefab;//箱
    public GameObject Box2Prefab;//壁
    public GameObject goalPrefab;//ゴール
    public GameObject clearText;//クリアテキスト
    public GameObject particlePrefab;//動いた時のパーティクル
    public GameObject backgroundPrefab; // 背景プレハブを追加

    public AudioSource audioSource; // AudioSource
    public AudioSource audioSource2; // AudioSource
    public AudioSource audioSource3; // AudioSource
    
    int[,] map;//マップ
    int[,] map2;//マップ2
    int[,] map3;//マップ3
    int mapLevel;//マップレベル
    GameObject[,] field;//ファイル
    List<GameObject> listGame;//
    List<GameObject> currentLevelObjects;//
    GameObject background;

    //ポジション位置
    Vector3 IndexToPosition(Vector2Int index)
    {
        return new Vector3(index.x - map.GetLength(1) / 2 + 0.5f, index.y - map.GetLength(0) / 2, 0);
    }

    //プレイヤーの位置取得
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

    //移動判定
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if (!success) { return false; }
        }
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box2")
        {
            return false;
        }

        Vector3 moveToPosition = IndexToPosition(moveTo);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);

        for (int i = 0; i < 5; i++)
        {
            field[moveTo.y, moveTo.x] = Instantiate(particlePrefab, moveToPosition, Quaternion.identity);
        }

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;

        audioSource.Play();

        return true;
    }


    //クリア判定
    bool IsCleard()
    {
        List<Vector2Int> goals = new List<Vector2Int>();

        if (mapLevel == 0)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == 3)
                    {
                        goals.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
        if (mapLevel == 1)
        {
            for (int y = 0; y < map2.GetLength(0); y++)
            {
                for (int x = 0; x < map2.GetLength(1); x++)
                {
                    if (map2[y, x] == 3)
                    {
                        goals.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
        if (mapLevel == 2)
        {
            for (int y = 0; y < map3.GetLength(0); y++)
            {
                for (int x = 0; x < map3.GetLength(1); x++)
                {
                    if (map3[y, x] == 3)
                    {
                        goals.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
            for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                return false;
            }
        }
        return true;
    }

    //移動ALL
    void Move()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            if (!IsCleard())
            {
                Vector2Int playerIndex = GetPlayerIndex();
                MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
            }
            if (IsCleard())
            {
                clearText.SetActive(true);
                audioSource2.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            if (!IsCleard())
            {
                Vector2Int playerIndex = GetPlayerIndex();
                MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(-1, 0));
            }
            if (IsCleard())
            {
                clearText.SetActive(true);
                audioSource2.Play();
            }
           
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!IsCleard())
            {
                Vector2Int playerIndex = GetPlayerIndex();
                MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));
            }
            if (IsCleard())
            {
                clearText.SetActive(true);
                audioSource2.Play();
            }
        
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            if (!IsCleard())
            {
                Vector2Int playerIndex = GetPlayerIndex();
                MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, -1));
            }
            if (IsCleard())
            {
                clearText.SetActive(true);
                audioSource2.Play();
            }
            
        }
    }

    //マップ初期化
    void InitializeMap(int[,] mapData)
    {
        field = new GameObject[mapData.GetLength(0), mapData.GetLength(1)];
        currentLevelObjects = new List<GameObject>();

        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                //プレイヤー
                if (mapData[y, x] == 1)
                {
                    field[y, x] = Instantiate(playerPrefab, IndexToPosition(new Vector2Int(x, y)), Quaternion.identity);
                    currentLevelObjects.Add(field[y, x]);
                }
                //Box
                if (mapData[y, x] == 2)
                {
                    field[y, x] = Instantiate(BoxPrefab, IndexToPosition(new Vector2Int(x, y)), Quaternion.identity);
                    currentLevelObjects.Add(field[y, x]);
                }
                //ゴール
                if (mapData[y, x] == 3)
                {
                    field[y, x] = Instantiate(goalPrefab, IndexToPosition(new Vector2Int(x, y)), Quaternion.identity);
                    currentLevelObjects.Add(field[y, x]);
                }
                //壁
                if (mapData[y, x] == 4)
                {
                    field[y, x] = Instantiate(Box2Prefab, IndexToPosition(new Vector2Int(x, y)), Quaternion.identity);
                    currentLevelObjects.Add(field[y, x]);
                } 
            }
        }

        listGame.AddRange(currentLevelObjects);
    }

    //マップレベル
    void MapLevel()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            audioSource3.Play();
            mapLevel = 0;
            Reset();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            audioSource3.Play();
            mapLevel = 1;
            Reset();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            audioSource3.Play();
            mapLevel = 2;
            Reset();
        }
    }

    //効果音再生
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    //リセット
    void Reset()
    {
        audioSource3.Play();
        foreach (GameObject obj in listGame)
        {
            Destroy(obj);
        }
        listGame.Clear();

        if (mapLevel == 0)
        {
            InitializeMap(map);
        }
        else if (mapLevel == 1)
        {
            InitializeMap(map2);
        }
        else if (mapLevel == 2)
        {
            InitializeMap(map3);
        }

        clearText.SetActive(false);
    }

    //スタート
    void Start()
    {
        
        Screen.SetResolution(1280, 720, false);
        listGame = new List<GameObject>();

        map = new int[,]
        {
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
            {4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 4, 4, 4, 0, 2, 0, 3, 0, 3, 0, 2, 0, 4},
            {4, 3, 2, 0, 0, 0, 0, 0, 3, 0, 0, 2, 0, 4},
            {4, 3, 2, 0, 0, 2, 0, 3, 0, 3, 0, 2, 0, 4},
            {4, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
        };

        map2 = new int[,]
        {
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
            {4, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 2, 4},
            {4, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 4, 3, 4},
            {4, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 4, 4, 4},
            {4, 0, 0, 0, 0, 4, 1, 4, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 3, 4, 4, 4, 3, 0, 0, 0, 0, 4},
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
        };

        map3 = new int[,]
        {
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
            {4, 3, 4, 4, 4, 4, 0, 0, 0, 0, 4, 1, 4, 4},
            {4, 3, 4, 4, 4, 4, 2, 0, 0, 0, 4, 2, 4, 4},
            {4, 3, 2, 2, 0, 4, 2, 0, 0, 0, 4, 0, 4, 4},
            {4, 3, 0, 0, 0, 4, 2, 0, 0, 0, 0, 0, 0, 4},
            {4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 3, 0, 0, 0, 4, 2, 0, 0, 0, 0, 0, 0, 4},
            {4, 3, 0, 0, 0, 4, 2, 0, 0, 0, 0, 0, 0, 4},
            {4, 3, 0, 0, 0, 4, 2, 0, 0, 0, 0, 0, 0, 4},
            {4, 3, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4},
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
        };

        mapLevel = 0;
        InitializeMap(map);

        audioSource = gameObject.GetComponent<AudioSource>();   

        // 背景を生成
        background = Instantiate(backgroundPrefab, new Vector3(0, 0, 1), Quaternion.identity);
    }

    //更新処理
    void Update()
    {
        //マップレベル
        MapLevel();
        //リセット
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
        //移動
        Move();
    }
}
