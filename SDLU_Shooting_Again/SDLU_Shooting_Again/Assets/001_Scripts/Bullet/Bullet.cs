using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Bullet : MonoBehaviour
{
    event System.Action _onCollision; // 물체와 충돌 시 호출
    Rigidbody rigid;
    int damage = 5;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>(); // RequireComponent 으로 null 발생 불가능
    }

    private void OnEnable()
    {
        Invoke(nameof(DisableBullet), 10.0f);
    }

    /// <summary>
    /// 총알을 발사합니다.
    /// </summary>
    /// <param name="velocity">발사 힘</param>
    /// <param name="onCollision">물체와 충돌 시 실행될 Callback</param>
    public void Fire(Vector3 velocity, System.Action onCollision)
    {
        rigid.velocity = Vector3.zero;
        
        gameObject.SetActive(true);

        rigid.AddForce(velocity, ForceMode.Impulse);

        _onCollision = onCollision;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        RemotePlayer otherPlayer = other.gameObject.GetComponent<RemotePlayer>();
        otherPlayer.Damaged(damage);
        //TODO : 0바꾸기
        SocketClient.Instance.Send(new DataVO("damage", JsonUtility.ToJson(new DamageVO(otherPlayer.ID, damage))));
        //TODO : 1, 10 바꾸기
        _onCollision?.Invoke();

        gameObject.SetActive(false);
    }


    private void DisableBullet()
    {
        rigid.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
