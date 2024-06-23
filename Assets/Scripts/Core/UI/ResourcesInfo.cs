using TMPro;
using UnityEngine;

public class ResourcesInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _crystalsText;
    [SerializeField] TextMeshProUGUI _ticketsText;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;
        _appData.OnResourcesChanged += _appData_OnResourcesChanged;
        _appData_OnResourcesChanged(_appData.GetCrystals(), _appData.GetTickets());
    }

    private void _appData_OnResourcesChanged(int crystals, int tickets)
    {
        _crystalsText.text = crystals.ToString();
        _ticketsText.text = tickets.ToString();
    }
}
