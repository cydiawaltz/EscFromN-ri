using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public string targetTag = "player";  // ターゲットのタグ
    public AnimationClip animationClip;    // 再生するアニメーションクリップ
    public Vector3 targetPosition;         // 移動先の位置

    private bool hasCollided = false;      // すでに接触したかどうかのフラグ

    private void OnCollisionEnter(Collision collision)
    {
        // 接触したオブジェクトのタグが指定のタグと一致するか確認
        if (collision.gameObject.CompareTag(targetTag) && !hasCollided)
        {
            hasCollided = true;

            // アニメーション再生
            PlayAnimation();

            // 指定の位置へ移動
            MoveToTargetPosition();
        }
    }

    private void PlayAnimation()
    {
        // アニメーション再生
        if (animationClip != null)
        {
            GetComponent<Animation>().Play(animationClip.name);
        }
    }

    private void MoveToTargetPosition()
    {
        // 指定の位置へ移動
        transform.position = targetPosition;
    }
}
