using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public enum ChatType { SYSTEM, PRESIDENT }
    [SerializeField]
    GameObject _chatSystemItemPrefab, _presendentItemPrefab, _chatParent;

    public ChatManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Say(ChatType chatType, string message)
    {
        switch (chatType)
        {
            case ChatType.SYSTEM:
                Instantiate(_chatSystemItemPrefab, _chatParent.transform).GetComponent<ChatMessage>().SetMessage(message);
                break;
            case ChatType.PRESIDENT:
                Instantiate(_presendentItemPrefab, _chatParent.transform).GetComponent<ChatMessage>().SetMessage(message);
                break;
        }
    }
}
