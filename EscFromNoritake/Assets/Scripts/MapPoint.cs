using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour {


    [SerializeField] int MapIndex;
    [SerializeField] List<MapPoint> groundPoints;//隣り合うマップ

    private bool isPassing = false; //trueの場合すでに探索済みfalseなら未探索
    public bool IsPassing
    {
        set { isPassing = value; }
        get { return isPassing; }
    }

    /// <summary>
    /// 現在地のIndexを返す
    /// </summary>
    /// <returns>Index</returns>
    public int GetPointIndex()
    {
        return MapIndex;
    }

    /// <summary>
    /// 隣り合うマップ情報を渡す
    /// </summary>
    /// <returns>隣接マップ</returns>
    public List<MapPoint> GetGroundPoints()
    {
        return groundPoints;
    }
}
