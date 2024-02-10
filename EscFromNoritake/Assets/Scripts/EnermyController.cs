using UnityEngine;
using System;
using UnityEngine.AI;

public class EnermyController : MonoBehaviour
{
    public NavMeshAgent Noritake;
    public GameObject Player;//Playerを自動で追尾する　Playerは迫りくるノリタケを交わし続けなければならない
    public GameObject Siya;//普段はPlayerよりも遅く移動するが、視野内（設定してね）に入ると移動速度が2倍になる　視野はcircle coliderで
    void Start ()
    {
        Noritake  = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update () 
    {
        if (Player != null) 
        {
            Noritake.destination  = Player.transform.position;//https://qiita.com/aimy-07/items/d1fea617ab9cbb3bd1ed 詳しくはこ↑こ↓
        }
        
    }
    void OnTriggerStay(Collider other)
	{
		if( other.tag == "player")
		{
			//速度２倍　https://qiita.com/Amatsuki/items/af28a5f3aa370923def0 https://styly.cc/ja/tips/unity-navmesh-ai/ 
            //https://qiita.com/Butterfly-Dream/items/1ad47dd2bd75bfc71ce2 
		}
	}
}