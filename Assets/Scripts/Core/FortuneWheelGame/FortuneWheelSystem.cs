using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class FortuneWheelSystem : MonoBehaviour
{
    [SerializeField] Transform _wheelTransform;
    [SerializeField] List<Image> _itemPos;
    [SerializeField] List<WheelItem> _items;
    [SerializeField] Button _spinButton;
    [SerializeField] PopupScreen _popupScreen;

    [SerializeField] float maxSpinSpeed = 500f;
    [SerializeField] int _spinPrice = 10;

    [SerializeField] AudioClip _applyClip;
    [SerializeField] AudioClip _wheelClip;
    [SerializeField] AudioClip _prizeClip;

    [SerializeField] Button _closeButton;

    private float[] _prizeAngles;
    private float _targetAngle;
    private bool _isSpinning = false;

    void Start()
    {
        int segmentCount = _items.Count;
        _prizeAngles = new float[segmentCount];
        float angleStep = 360.0f / segmentCount;

        for (int i = 0; i < segmentCount; i++)
        {
            _prizeAngles[i] = i * angleStep;
            _itemPos[i].sprite = _items[i].Sprite;
            _itemPos[i].GetComponentInChildren<TextMeshProUGUI>().text = $"x{_items[i].Reward}";
        }
        _spinButton.onClick.AddListener(SpinWheel);
    }

    private void SpinWheel()
    {
        AudioManager.Instance.PlayOneShotSound(_applyClip);
        if (!_isSpinning)
        {
            var tickets = ApplicationData.Instance.GetTickets();
            if (tickets >= _spinPrice)
            {
                _closeButton.interactable = false;
                ApplicationData.Instance.AddResourceTickets(-_spinPrice);
                _isSpinning = true;

                
                int prizeIndex = UnityEngine.Random.Range(0, _items.Count);
                _targetAngle = 360 * 5 - _prizeAngles[prizeIndex];
                StartCoroutine(SpinCoroutine(_targetAngle, prizeIndex));
            }
            else
            {
                _popupScreen.ShowMessage($"NOT ENOUGH TICKETS");
                _closeButton.interactable = true;
            }
        }
    }

    IEnumerator SpinCoroutine(float targetAngle, int prizeIndex)
    {
        AudioManager.Instance.PlayOneShotSound(_wheelClip);
        _isSpinning = true;
        float elapsedTime = 0.0f;
        float currentAngle = _wheelTransform.eulerAngles.z;
        float startAngle = currentAngle;

        while (elapsedTime < _wheelClip.length)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _wheelClip.length;
            float angle = Mathf.Lerp(startAngle, targetAngle, t);
            _wheelTransform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }

        _wheelTransform.eulerAngles = new Vector3(0, 0, targetAngle % 360);
        _isSpinning = false;

        CheckPrize(prizeIndex);
        _closeButton.interactable = true;
    }

    private void CheckPrize(int prizeIndex)
    {
        AudioManager.Instance.PlayOneShotSound(_prizeClip);
        WheelItem wheelItem = _items[prizeIndex];

        switch (wheelItem.Type)
        {
            case WheelItem.WheelItemType.Crystal:
                ApplicationData.Instance.AddResourceCrystals(wheelItem.Reward);
                break;
            case WheelItem.WheelItemType.Ticket:
                ApplicationData.Instance.AddResourceTickets(wheelItem.Reward);
                break;
            default:
                break;
        }
        
    }
}

[Serializable]
public class WheelItem
{
    public Sprite Sprite;
    public int Reward;
    public float SectorAngle;
    public WheelItemType Type;

    public enum WheelItemType
    {
        Crystal,
        Ticket
    }
}
