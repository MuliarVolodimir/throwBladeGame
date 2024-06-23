using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] int _price;
    [SerializeField] int _reward;
    [SerializeField] ResourceType _resourceType;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
    }

    private enum ResourceType
    {
        Crystal,
        Ticket
    }

    public void Buy()
    {
        switch (_resourceType)
        {
            case ResourceType.Crystal:
                BuyCrystals();
                break;
            case ResourceType.Ticket:
                BuyTickets();
                break;
            default:
                break;
        }
    }

    private void BuyCrystals()
    {
        // purchase system logic
        _appData.AddResourceCrystals(_reward);
    }

    private void BuyTickets()
    {
        if (_appData.GetCrystals() >= _price)
        {
            _appData.AddResourceCrystals(-_price);
            _appData.AddResourceTickets(_reward);
        }
    }
}
