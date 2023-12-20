using UnityEngine;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    public Slider slider;

    public Image background;

    public Sprite zero;
    public Sprite two;
    public Sprite four;
    public Sprite six;
    public Sprite eight;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < 0.25)
        {
            background.sprite = zero;
        }
        if (slider.value > 0.25)
        {
            background.sprite = two;
        }
        if (slider.value > 0.5)
        {
            background.sprite = four;
        }
        if (slider.value > 0.75)
        {
            background.sprite = six;
        }
        if (slider.value == 1)
        {
            background.sprite = eight;
        }
    }
}
