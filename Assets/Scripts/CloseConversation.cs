using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseConversation : MonoBehaviour
{
    GameObject chatBox;
    // Start is called before the first frame update
    void Start()
    {
        chatBox = transform.Find("Shopkeeper").gameObject;
    }

    public void CloseNPCConversation()
    {
        Debug.Log(chatBox);
        chatBox.SetActive(false);
    }
}
