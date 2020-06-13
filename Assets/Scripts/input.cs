using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using UnityEngine.Networking;

public class input : MonoBehaviour
{
    string temp = ""; 

    void Start()
    {
        StartCoroutine(GetText2(temp));
    }

    public IEnumerator GetText2(string oracion)
    {
        if (oracion != "")
        {
            Debug.Log("hola " + oracion);

            string oracionEstructurada = "palabra = 'padres' OR palabra = 'no' OR palabra = 'comer'";

            using (UnityWebRequest request = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?palabras=" + oracionEstructurada))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError) // Error
                {
                    Debug.Log(request.error);
                }
                else // Success
                {
                    Debug.Log(request.downloadHandler.text);
                }
            }
        }
    }
}