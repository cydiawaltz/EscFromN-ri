/*
using UnityEngine;

public class Sight : MonoBehaviour
{
    public Transform Player;
    public float movespeed = 1.0f;
    // 自分自身
    [SerializeField] private Transform _self;

    // ターゲット
    [SerializeField] private Transform _target;

    // 視野角（度数法）
    [SerializeField] private float _sightAngle;

    // 視界の最大距離
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    #region Logic

    /// <summary>
    /// ターゲットが見えているかどうか
    /// </summary>
    public bool IsVisible()
    {
        // 自身の位置
        Vector2 selfPos = _self.position;
        // ターゲットの位置
        Vector2 targetPos = _target.position;

        // 自身の向き（正規化されたベクトル）
        // この例では右向きを正面とする
        Vector2 selfDir = _self.right;
        
        // ターゲットまでの向きと距離計算
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector2.Dot(selfDir, targetDir.normalized);

        // 視界判定
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }
    void Update()
    {
        var isVisible = IsVisible();
        if(isVisible == true)
        {
            // プレイヤーの位置を向く
            transform.LookAt(Player);

            // プレイヤーに向かって移動
            transform.Translate(Vector3.forward * movespeed * Time.deltaTime);
        }
    }


    #endregion

    #region Debug

    // 視界判定の結果をGUI出力
    private void OnGUI()
    {
        // 視界判定
        var isVisible = IsVisible();

        // 結果表示
        GUI.Box(new Rect(20, 20, 150, 23), $"isVisible = {isVisible}");
    }

    #endregion
}
//https://nekojara.city/unity-object-sight
*/
using UnityEngine;

public class Homing : MonoBehaviour
{
    Transform playerTr; // プレイヤーのTransform
    [SerializeField] float speed = 2; // 敵の動くスピード

    private void Start() {
        // プレイヤーのTransformを取得（プレイヤーのタグをPlayerに設定必要）
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        
        // プレイヤーとの距離が0.1f未満になったらそれ以上実行しない
        if (Vector2.Distance(transform.position, playerTr.position) < 0.1f)
            return;

        // プレイヤーに向けて進む
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(playerTr.position.x, playerTr.position.y), 
            speed * Time.deltaTime);
    }
}

