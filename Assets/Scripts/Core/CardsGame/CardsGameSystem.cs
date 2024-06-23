using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsGameSystem : MonoBehaviour
{
    [SerializeField] Sprite cardBackSprite;
    [SerializeField] Sprite _cardFlippedSprite;
    [SerializeField] List<Image> _itemsImages;
    [SerializeField] List<CardItem> _cardItems;
    [SerializeField] List<Button> _cardButtons;
    [SerializeField] Button _startButton;
    [SerializeField] PopupScreen _popupScreen;
    [SerializeField] int _price = 5;

    [SerializeField] AudioClip _applyClip;
    [SerializeField] AudioClip _flipCardClip;

    [SerializeField] Button _closeButton;

    private int _selectedCardIndex = -1;
    private CardItem[] _originalCardItems; 
    private bool _gameInProgress = false;

    private void Start()
    {
        //cardButtons = GetComponentsInChildren<Button>();
        _originalCardItems = new CardItem[_cardButtons.Count];
        AssignRewards();
        ActivateCardButtons(false);

        _startButton.onClick.AddListener(StartGame);
    }

    private void AssignRewards()
    {
        for (int i = 0; i < _cardButtons.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _cardItems.Count);
            _originalCardItems[i] = _cardItems[randomIndex];

            Image buttonImage = _cardButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = cardBackSprite;
            }
            int index = i;
            _cardButtons[i].onClick.AddListener(() => OnCardSelected(index));
        }
    }

    private void StartGame()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        int tickets = ApplicationData.Instance.GetTickets();
        if (tickets >= _price)
        {
            _closeButton.interactable = false;
            ApplicationData.Instance.AddResourceTickets(-_price);
            _gameInProgress = true;
            _startButton.interactable = false;

            ActivateCardButtons(true);
        }
        else
        {
            _popupScreen.ShowMessage($"NOT ENOUGHT TICKETS");
        }
    }

    private void OnCardSelected(int cardIndex)
    {
        if (_gameInProgress && _selectedCardIndex == -1)
        {
            _selectedCardIndex = cardIndex;
            StartCoroutine(RevealCard(cardIndex));
            AudioManager.Instance.PlayOneShotSound(_flipCardClip);
        }
    }

    IEnumerator RevealCard(int cardIndex)
    {
        yield return new WaitForSeconds(0.2f);
        Image buttonImage = _cardButtons[cardIndex].GetComponent<Image>();

        if (buttonImage != null)
        {
            buttonImage.sprite = _cardFlippedSprite;
        }

        //var itemImage = _cardButtons[cardIndex].gameObject.GetComponentInChildren<Image>();

        _itemsImages[cardIndex].gameObject.SetActive(true);
        _itemsImages[cardIndex].sprite = _originalCardItems[cardIndex].Sprite;
        _itemsImages[cardIndex].GetComponentInChildren<TextMeshProUGUI>().text = $"x{_originalCardItems[cardIndex].Reward}";

        yield return new WaitForSeconds(3.5f);
        CheckPrize(cardIndex);
        ResetGame(); 
    }

    private void CheckPrize(int prizeIndex)
    {
        CardItem wheelItem = _originalCardItems[prizeIndex];

        switch (wheelItem.Type)
        {
            case CardItem.RewardType.Crystal:
                ApplicationData.Instance.AddResourceCrystals(wheelItem.Reward);
                break;
            case CardItem.RewardType.Ticket:
                ApplicationData.Instance.AddResourceTickets(wheelItem.Reward);
                break;
            default:
                break;
        }

    }

    private void ResetGame()
    {
        _gameInProgress = false;
        _selectedCardIndex = -1;
        AssignRewards();
        _startButton.interactable = true;
        ActivateCardButtons(false);
        _closeButton.interactable = true;
    }

    private void ActivateCardButtons(bool activate)
    {
        foreach (Button button in _cardButtons)
        {
            button.interactable = activate;
        }

        foreach (var images in _itemsImages)
        {
            images.gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class CardItem
{
    public Sprite Sprite;
    public int Reward;
    public RewardType Type;

    public enum RewardType
    {
        Crystal,
        Ticket
    }
}