using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationFrameUI : MonoBehaviour
{
    private void Start()
    {
        ConversationManager.Instance.OnConversationStart += OnConversationStartDelegate;
        ConversationManager.Instance.OnConversationNext += OnConversationNextDelegate;
        ConversationManager.Instance.OnConversationStop += OnConversationStopDelegate;
        transform.gameObject.SetActive(false);
    }

    private void OnConversationStartDelegate(Interaction interaction)
    {
        transform.gameObject.SetActive(true);
        ShowInteraction(interaction);
    }

    private void ShowInteraction(Interaction interaction)
    {
        transform
            .Find("CharacterImage")
            .Find("Image")
            .GetComponent<Image>().sprite = interaction.CharacterImage;
        transform
            .Find("CharacterName")
            .GetComponent<TextMeshProUGUI>().text = interaction.CharacterName;
        transform
            .Find("CharacterText")
            .GetComponent<TextMeshProUGUI>().text = interaction.CharacterText;

    }

    private void OnConversationNextDelegate(Interaction interaction)
    {
        ShowInteraction(interaction);
    }
    private void OnConversationStopDelegate()
    {
        transform.gameObject.SetActive(false);
    }


}
