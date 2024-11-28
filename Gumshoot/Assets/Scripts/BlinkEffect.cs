using UnityEngine;
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour
{
    public string tagToBlink = "Blink"; 
    public Color color1 = Color.white;
    public Color color2 = Color.red;
    public float blinkSpeed = 1.0f;

    private Image[] images;

    void Start()
    {
       
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tagToBlink);
        images = new Image[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            images[i] = objects[i].GetComponent<Image>();
        }
    }

    void Update()
    {
        if (images == null) return;

        float lerp = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);
        foreach (var image in images)
        {
            if (image != null)
            {
                image.color = Color.Lerp(color1, color2, lerp);
            }
        }
    }
}


