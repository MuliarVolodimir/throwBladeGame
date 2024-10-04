using UnityEngine;
using UnityEngine.Purchasing;

public class ShopItem : MonoBehaviour
{
    [SerializeField] int _price;
    [SerializeField] int _reward;
    [SerializeField] ResourceType _resourceType;
    [SerializeField] PopupScreen _popupScreen;

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
                //BuyCrystals();
                break;
            case ResourceType.Ticket:
                BuyTickets();
                break;
            default:
                break;
        }
    }

    public void OnPurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "com.neytralstudio.throwtheblades.buycrystals":
                BuyCrystals();
                break;
        }
    }

    private void BuyCrystals()
    {
        _appData.AddResourceCrystals(_reward);
        Debug.Log($"buy {_reward} crystals");
    }

    private void BuyTickets()
    {
        if (_appData.GetCrystals() >= _price)
        {
            _appData.AddResourceCrystals(-_price);
            _appData.AddResourceTickets(_reward);
        }
        else
        {
            _popupScreen.ShowMessage("NOT ENOUGHT CRYSTALS");
        }
    }
}
