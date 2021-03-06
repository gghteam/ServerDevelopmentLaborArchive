using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class SocketClient : MonoBehaviour
{
    [SerializeField] private string url = "ws://localhost";
    [SerializeField] private int port = 32000; // <= 선한쌤 서버 사용할 예정이여서 어쩔 수 없이 56789를 버렸습니다. 슬프네요

    public bool instaConnect = false;

    // 웹소켓
    private WebSocket ws;

    // static 접근 용
    static private SocketClient instance = null;

    // 연결 되었을 시 콜백 함수 용
    public delegate void OnConnected();

    private void Awake()
    {
        // 싱글턴 패턴 용도가 아니에요.
        instance = this;
    }

    private void Start()
    {
        if (instaConnect)
        {
            ConnectToServer();
        }
    }

    /// <summary>
    /// 서버에 연결하는 함수
    /// </summary>
    /// <param name="callback"></param>
    static public void ConnectToServer(OnConnected callback = null)
    {
        instance.ws = new WebSocket($"{instance.url}:{instance.port}");
        instance.ws.Connect();

        instance.ws.OnMessage += (sender, e) =>
        {
            instance.ReceiveData((WebSocket)sender, e);
        };

        callback?.Invoke();
    }

    /// <summary>
    /// 서버에 연결하는 함수
    /// </summary>
    /// <param name="ip">서버의 ip<br></br>ws://"이 부분":port</param>
    /// <param name="port">포트<br></br>ws://ip:"이 부분"</param>
    /// <param name="callback"></param>
    static public void ConnectToServer(string ip, int port, OnConnected callback = null)
    {
        instance.url = $"ws://{ip}";
        instance.port = port;

        instance.ws = new WebSocket($"{instance.url}:{instance.port}");
        instance.ws.Connect();

        instance.ws.OnMessage += (sender, e) =>
        {
            instance.ReceiveData((WebSocket)sender, e);
        };

        callback?.Invoke();
    }


    static public void DisconnectToServer(OnConnected callback = null)
    {
        instance.ws.Close();

        callback?.Invoke();
    }

    private void ReceiveData(WebSocket sender, MessageEventArgs e)
    {
        // 들어온 메세지를 DataVO 에 넣어줘요.
        DataVO vo = JsonUtility.FromJson<DataVO>(e.Data);
        // Handler에게 넘겨줘요.
        BufferHandler.HandleBuffer(vo.type, vo.payload);
    }

    /// <summary>
    /// 서버로 메세지를 보내는 함수
    /// </summary>
    /// <param name="data">버퍼</param>
    static public void Send(string data)
    {
        try
        {
            instance.ws.Send(data);
        }
        catch (Exception e)
        {
            Debug.LogError($"서버에 데이터를 보내는 중 문제가 생겼어요.\r\n{e.Message}");
        }
    }

    private void OnDestroy()
    {
        if (ws.ReadyState == WebSocketState.Connecting)
            ws.Close();
    }
}