using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConversationManager : MonoBehaviour
{
    public static ConversationManager Instance {private set; get;}
    public event UnityAction<Interaction> OnConversationStart;
    public event UnityAction<Interaction> OnConversationNext;
    public event UnityAction OnConversationStop;

    private int mInteractionIndex = 0;
    private Conversation mActiveConversation = null;

    private void Awake()
    {
        Instance = this;
    }

    public void StartConversation(Conversation conversation)
    {
        mActiveConversation = conversation;
        OnConversationStart?.Invoke(
            mActiveConversation.Interactions[mInteractionIndex++]
        );
    }

    public void NextConversation()
    {
        if (mInteractionIndex < mActiveConversation.Interactions.Count)
        {
            OnConversationNext?.Invoke(
                mActiveConversation.Interactions[mInteractionIndex++]
            );
        }else
        {
            StopConversation();
        }
    }

    public void StopConversation()
    {
        mActiveConversation = null;
        mInteractionIndex = 0;
        OnConversationStop?.Invoke();
    }
}
