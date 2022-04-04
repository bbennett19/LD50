using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public enum ChatType { SYSTEM, PRESIDENT }
    [SerializeField]
    GameObject _chatSystemItemPrefab, _presendentItemPrefab, _chatParent;
    [SerializeField]
    ScrollRect _scrollRect;

    public static ChatManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Say(ChatType chatType, string message)
    {
        Debug.Log("CHAT: " + chatType.ToString() + ": " + message);
        switch (chatType)
        {
            case ChatType.SYSTEM:
                Instantiate(_chatSystemItemPrefab, _chatParent.transform).GetComponent<ChatMessage>().SetMessage(message);
                StartCoroutine(ScrollToBottom());
                break;
            case ChatType.PRESIDENT:
                Instantiate(_presendentItemPrefab, _chatParent.transform).GetComponent<ChatMessage>().SetMessage(message);
                StartCoroutine(ScrollToBottom());
                break;
        }
    }

    private IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        _scrollRect.verticalNormalizedPosition = 0;
    }
}
