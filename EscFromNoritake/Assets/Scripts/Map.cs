using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Map : MonoBehaviour {

    [SerializeField] Image mapImage; //マップ画像
    [SerializeField] GameObject character; //キャラクターオブジェクト
    [SerializeField] List<MapPoint> mapLists; //フィールドにあるマップ

    private const float HEIGHT_MAX = 994f;//移動できる範囲(X軸 +-で考える
    private const float WIDTH_MAX = 568f; //移動できる範囲(Y軸 +-で考える

    private Vector3 mapDownPosition; //マップをタップした位置
    private MapState currentState = MapState.None; //現在の状態
    private MapPoint currentMapPoint; //現在地

    private MapPoint nullMapData = new MapPoint();//nullデータ
    public Transform playertr;

    //マップステータス
    private enum MapState
    {
        None, //何もしていない
        CharacterMove, //キャラクター移動中
        RootSearch, //ルート探索中
    }

    private void Awake()
    {
        //キャラクターの現在位置取得・設定
        playertr = GameObject.FindGameObjectWithTag("Player").transform;
        int defaultPointIndex = PlayerPrefs.GetInt(PlayerPrefsWord.WORLD_CURRENT_POINTS, 0);
        //int defaultPointIndex =  PlayerPrefs.GetInt(playertr, 0);
        character.transform.localPosition = mapLists[defaultPointIndex].transform.localPosition;
        mapImage.transform.localPosition = -character.transform.localPosition;
        MapOutdoorJudgment();
        currentMapPoint = mapLists[defaultPointIndex];
    }

    /// <summary>
    /// マップの場外判定（マップが範囲外になった場合自動で補正
    /// </summary>
    private void MapOutdoorJudgment()
    {
        mapImage.transform.localPosition = (new Vector3(Mathf.Clamp(mapImage.transform.localPosition.x, -HEIGHT_MAX, HEIGHT_MAX),
            Mathf.Clamp(mapImage.transform.localPosition.y, -WIDTH_MAX, WIDTH_MAX),
            0f));
    }

    /// <summary>
    /// 次に進むルートを取得する（ターゲットに近いポイントを導き出す）
    /// </summary>
    /// <param name="points">現在地に隣接するルート</param>
    /// <param name="targetPoint">ゴール地点</param>
    /// <returns>nullが返ってきた場合は行き止まり</returns>
    private MapPoint NextTarget(List<MapPoint> points,Transform targetPoint)
    {    
        MapPoint shortestPoint = points[0].IsPassing ? nullMapData : points[0]; //最短ルート初期設定
        float shortestDistance = shortestPoint == nullMapData ? -1f : Vector3.Distance(points[0].transform.localPosition, targetPoint.localPosition);//最短距離計算初期設定（nullの場合-1)

        for (int i = 1;i < points.Count; i++)
        {
            if (points[i].IsPassing) continue;//探索済みのルートは見ない
            if(shortestDistance == -1f || shortestDistance > Vector3.Distance(points[i].transform.localPosition, targetPoint.localPosition))//最短距離チェック
            {
                shortestDistance = Vector3.Distance(points[i].transform.localPosition, targetPoint.localPosition);
                shortestPoint = points[i];
            }
        }
        return shortestPoint;
    }

    /// <summary>
    /// 探索をしたルートを戻ってまだ探索していないルートを導き出す
    /// </summary>
    /// <param name="rootPoints">現在探索を終えたルート</param>
    /// <returns>falseが返ってきた場合は指定のルートに行けない</returns>
    private bool BacktrackPoint(ref List<MapPoint> rootPoints)
    {
        bool isAnotherWay = false; //別のルートがあったか
        int backRootListIndex = rootPoints.Count; //戻るルートまでの配列番号
        for (int i = rootPoints.Count-1; i >= 0; i--)
        {
            for (int j = 0; j < rootPoints[i].GetGroundPoints().Count; j++)
            {
                if(rootPoints[i].GetGroundPoints()[j].IsPassing == false)//まだ探索を行っていないルートがある
                {
                    isAnotherWay = true;
                    break;
                }
            }
            if (isAnotherWay) break;
            backRootListIndex--;
        }

        rootPoints.RemoveRange(backRootListIndex, rootPoints.Count - backRootListIndex);//戻るルート分削除

        return isAnotherWay;
    }
    /// <summary>
    /// キャラクターの移動
    /// </summary>
    /// <param name="moveMapPoint">移動中継地点</param>
    private void MoveCharacter(List<MapPoint> moveMapPoint)
    {
        if (moveMapPoint.Count > 0)//移動地点があるか
        {
            character.transform.DOLocalMove(moveMapPoint[0].transform.localPosition, 0.3f).OnComplete(() => {
                moveMapPoint.RemoveAt(0);//先頭から消していく
                MoveCharacter(moveMapPoint);
            });
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsWord.WORLD_CURRENT_POINTS, currentMapPoint.GetPointIndex());//初期座標のセーブ
            currentState = MapState.None;
        }
    }

    /// <summary>
    /// マップをタップした時の処理
    /// </summary>
    public void OnMapDown()
    {
        if (currentState != MapState.None) return;//処理中は受け付けない
        mapDownPosition = Input.mousePosition;
    }

    /// <summary>
    /// マップをスクロールした時の処理
    /// </summary>
    public void OnMapDrag()
    {
        if (currentState != MapState.None) return;//処理中は受け付けない
        mapImage.transform.localPosition -= (mapDownPosition - Input.mousePosition);
        MapOutdoorJudgment();
        mapDownPosition = Input.mousePosition;
    }

    /// <summary>
    /// タップした位置までのルートを割り出す
    /// </summary>
    /// <param name="pushMap"></param>
    public void PushMap(MapPoint pushMap)
    {
        if (pushMap.GetPointIndex() == currentMapPoint.GetPointIndex()) return;
        if (currentState != MapState.None) return;//操作中はルート探索をしない
        currentState = MapState.RootSearch; //現在のステータスを変える（探索中)
        List<MapPoint> moveMapPoint = new List<MapPoint>();//ゴールまでのルート

        foreach(var map in mapLists)
        {
            map.IsPassing = false;
        }

        while (true)
        {
            currentMapPoint.IsPassing = true;
            MapPoint nextPoint = NextTarget(currentMapPoint.GetGroundPoints(), pushMap.transform);
            if (nextPoint != nullMapData)//行き止まりでないか？
            {
                nextPoint.IsPassing = true; //探索済みにする
                moveMapPoint.Add(nextPoint);
                currentMapPoint = nextPoint;
            }
            else
            {
                if (!BacktrackPoint(ref moveMapPoint))//もしもすべての探索をしてもなおたどり着けない場合探索を終える
                {
                    Debug.LogError("そのルートへはいけません");
                    break;
                }
                else//別のルートがあった場合そのルートから探索を開始する
                {
                    currentMapPoint = moveMapPoint[moveMapPoint.Count - 1]; //現在地変更
                }
            }

            if(currentMapPoint.GetPointIndex() == pushMap.GetPointIndex()){//探索終了
                MoveCharacter(moveMapPoint);
                break;
            }
        }
    }
}
