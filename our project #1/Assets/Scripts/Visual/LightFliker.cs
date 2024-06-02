using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFliker : MonoBehaviour
{
    public float maxIntesity;
    public float minIntesity;
    public Gradient randomColor;
    public float changeAmount;
    Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Light());
    }

    IEnumerator Light()
    {
        light.intensity = Random.Range(minIntesity, maxIntesity);
        light.color = randomColor.Evaluate(Random.Range(.02f, 1));
        yield return new WaitForSeconds(changeAmount);
    }
}
