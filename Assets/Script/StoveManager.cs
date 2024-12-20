using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoveManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Slider temperatureSlider;
    [SerializeField] private TMP_Text temperatureText;
    [SerializeField] private GameObject reelTemperatureButton;

    private int temperature;

    // Start is called before the first frame update
    void Start()
    {
        temperatureSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= 2)
        {
            temperatureSlider.gameObject.SetActive(true);
        }
        else
        {
            temperatureSlider.gameObject.SetActive(false);
        }
    }

    public void OnTemperatureChange()
    {
        temperature = (int)temperatureSlider.value;
        temperatureText.text = temperature + "°C";
        reelTemperatureButton.transform.rotation = Quaternion.Euler(0,0,temperature * 3);
    }
}
