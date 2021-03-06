using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatHandler : MonoBehaviour
{
    void Start()
    {
        BufferHandler.Instance.AddHandler("chat", (msg) => {
            Debug.Log(msg);
            ChatManager.Instance.CreateChatPref(msg, false);
        });
    }
}
