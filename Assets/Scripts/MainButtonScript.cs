using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Networking;
using UnityEngine.Networking;


public class MainButtonScript : MonoBehaviour
{

    public Animator animOptionButton;
    public Animator animSoundButton;
    public Animator animTraductorButton;
    public Animator arbolAnimator;
    public GameObject soundButton;
    public GameObject traductorButton;
    public GameObject panelTextTraduction;
    public GameObject rosa;
    public GameObject textBox;
    public InputField inField;
    public string oracion;
    string[] words;
    string[] oracionEstructurada;
    string[] oracionEstructuradaSecond;
    string[] tiposDePalabras;
    string[] newOracionEstructurada;
    string[] newTiposDePalabras;
    public int[] ordenAnimaciones;
    public int contador;


    // Start is called before the first frame update
    void Start()
    {
        panelTextTraduction = GameObject.Find("PanelTW");
        panelTextTraduction.gameObject.SetActive(false);
        //StartCoroutine(GetText());
        oracion = "";
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void showOptions()
    {
        animOptionButton = GetComponent<Animator>(); 
        soundButton = GameObject.Find("SoundButton");
        traductorButton = GameObject.Find("TraductorButton");
        animSoundButton = soundButton.GetComponent<Animator>();
        animTraductorButton = traductorButton.GetComponent<Animator>();

        bool valueSB = animSoundButton.GetBool("DESPLEGAR");
        bool valueOB = animOptionButton.GetBool("DESPLEGAR");
        bool valueTB = animTraductorButton.GetBool("DESPLEGAR");


        if (valueOB == true) 
        {
            animOptionButton.SetBool("DESPLEGAR", false);
            animTraductorButton.SetBool("DESPLEGAR", false);
            animSoundButton.SetBool("DESPLEGAR", false);
        }
        else
        {
            animOptionButton.SetBool("DESPLEGAR", true);
            animTraductorButton.SetBool("DESPLEGAR", true);
            animSoundButton.SetBool("DESPLEGAR", true);
        }
    }

    public void showTraductorWindow()
    {
        panelTextTraduction.gameObject.SetActive(true);
    }

    public void cancelTraductorWindow() 
    {
        panelTextTraduction.gameObject.SetActive(false);
    }

    public void traduction()
    {
        textBox = GameObject.Find("InputField");
        inField = textBox.GetComponent<InputField>();
        oracion = inField.text;
        words = oracion.Split(' ');
        oracionEstructurada = oracion.Split(' ');
        tiposDePalabras = oracion.Split(' ');
        newOracionEstructurada = oracion.Split(' ');
        newTiposDePalabras = oracion.Split(' ');
        panelTextTraduction.gameObject.SetActive(false);
        StartCoroutine(getGeneralitation());
    }

    public IEnumerator GetText()
    {
        int i = 0;

        ordenAnimaciones = new int[newOracionEstructurada.Length + 1];

        foreach (string word in newOracionEstructurada)
        {
            using (UnityWebRequest request = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=1&palabras=" + word))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError) // Error
                {
                    Debug.Log(request.error);
                }
                else // Success
                {
                    ordenAnimaciones[i] = System.Convert.ToInt32(request.downloadHandler.text);
                    i++;

                }
            }
        }

        ordenAnimaciones[newOracionEstructurada.Length] = 0;

        contador = 0;
        rosa = GameObject.Find("Rosa");
        arbolAnimator = rosa.GetComponent<Animator>();
        arbolAnimator.SetInteger("INDICADOR", ordenAnimaciones[contador]);

        StartCoroutine(knn());

    }

    public IEnumerator knn()
    {
        int i = 0;

        string[] arrayAux = new string[newOracionEstructurada.Length];

        foreach (string word in newOracionEstructurada)
        {

            using (UnityWebRequest request = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=4&palabras=" + word))
            {
                yield return request.SendWebRequest();
                if (request.isNetworkError) // Error
                {
                    Debug.Log(request.error);
                }
                else // Success
                {
                    if (request.downloadHandler.text == "1")
                    {
                        arrayAux[i] = word;
                        i++;
                    }
                    
                }

            }

        }

        int flag = 0;
        int flagAux = 0;

        foreach (string word in arrayAux)
        {
            if(word != null)
            flag++;
        }

        string[] response;

        foreach (string word in arrayAux)
        {

            if (word != null)
            {
                using (UnityWebRequest request = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=5&palabras=" + word))
                {
                    yield return request.SendWebRequest();
                    if (request.isNetworkError) // Error
                    {
                        Debug.Log(request.error);
                    }
                    else // Success
                    {
                        if (request.downloadHandler.text != "ERROR")
                        {
                            response = (request.downloadHandler.text).Split(' ');

                            foreach (string category in arrayAux)
                            {
                                if (category != null )
                                {
                                    string[] responseAux;

                                    using (UnityWebRequest requestAux = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=5&palabras=" + category))
                                    {
                                        yield return requestAux.SendWebRequest();
                                        if (requestAux.isNetworkError) // Error
                                        {
                                            Debug.Log(requestAux.error);
                                        }
                                        else // Success
                                        {
                                            Debug.Log("(" + requestAux.downloadHandler.text + ")");

                                            if (requestAux.downloadHandler.text == "ERROR" && flag != flagAux)
                                            {
                                                int vua = 0;
                                                int vul = 0;
                                                int vug = 0;
                                                int punto = 0;

                                                if (response[5] == "general")
                                                {
                                                    vug = 1;
                                                    punto = 16;
                                                }
                                                if (response[5] == "academica")
                                                {
                                                    vua = 1;
                                                    punto = 0;

                                                }
                                                if (response[5] == "laboral")
                                                {
                                                    vul = 1;
                                                    punto = -16;
                                                }
                                                using (UnityWebRequest requestAuxTemp = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=6&palabras=" + category + "&vu=" + 1 + "&vua=" + vua + "&vul=" + vul + "&vug=" + vug + "&categoria=" + response[5] + "&punto=" + punto))
                                                {
                                                    yield return requestAuxTemp.SendWebRequest();
                                                    if (requestAuxTemp.isNetworkError) // Error
                                                    {
                                                        Debug.Log(requestAuxTemp.error);
                                                    }
                                                    else
                                                    {
                                                        Debug.Log(requestAuxTemp.downloadHandler.text);
                                                        flagAux++;
                                                    }
                                                }
                                            }
                                            else
                                            {

                                                if(flagAux != flag)
                                                {

                                                    responseAux = (requestAux.downloadHandler.text).Split(' ');

                                                    int mayor1 = 0;
                                                    int mayor2 = 0;
                                                    int posicion1 = 0;
                                                    int posicion2 = 0;

                                                    if (responseAux[2] == responseAux[3] && responseAux[3] == responseAux[4])
                                                    {
                                                        mayor1 = System.Convert.ToInt32(responseAux[2]);
                                                        mayor2 = System.Convert.ToInt32(responseAux[3]);
                                                        posicion1 = 2;
                                                        posicion2 = 3;
                                                    }
                                                    else
                                                    {
                                                            if (responseAux[2] == responseAux[3] || responseAux[2] == responseAux[4] || responseAux[4] == responseAux[3])
                                                            {
                                                                if (responseAux[2] == responseAux[3])
                                                                {
                                                                    if (System.Convert.ToInt32(responseAux[2]) > System.Convert.ToInt32(responseAux[4]))
                                                                    {
                                                                        mayor1 = System.Convert.ToInt32(responseAux[2]);
                                                                        mayor2 = System.Convert.ToInt32(responseAux[3]);
                                                                        posicion1 = 2;
                                                                        posicion2 = 3;
                                                                    }
                                                                    else
                                                                    {
                                                                        mayor1 = System.Convert.ToInt32(responseAux[4]);
                                                                        mayor2 = System.Convert.ToInt32(responseAux[2]);
                                                                        posicion1 = 4;
                                                                        posicion2 = 2;
                                                                    }
                                                                }

                                                                if (responseAux[2] == responseAux[4])
                                                                {
                                                                    if (System.Convert.ToInt32(responseAux[2]) > System.Convert.ToInt32(responseAux[3]))
                                                                    {
                                                                        mayor1 = System.Convert.ToInt32(responseAux[2]);
                                                                        mayor2 = System.Convert.ToInt32(responseAux[4]);
                                                                        posicion1 = 2;
                                                                        posicion2 = 4;
                                                                    }
                                                                    else
                                                                    {
                                                                        mayor1 = System.Convert.ToInt32(responseAux[3]);
                                                                        mayor2 = System.Convert.ToInt32(responseAux[2]);
                                                                        posicion1 = 3;
                                                                        posicion2 = 2;
                                                                    }
                                                                }

                                                                if (responseAux[4] == responseAux[3])
                                                                {
                                                                    if (System.Convert.ToInt32(responseAux[4]) > System.Convert.ToInt32(responseAux[2]))
                                                                    {
                                                                        mayor1 = System.Convert.ToInt32(responseAux[4]);
                                                                        mayor2 = System.Convert.ToInt32(responseAux[3]);
                                                                        posicion1 = 4;
                                                                        posicion2 = 3;
                                                                    }
                                                                    else
                                                                    {
                                                                        mayor1 = System.Convert.ToInt32(responseAux[2]);
                                                                        mayor2 = System.Convert.ToInt32(responseAux[3]);
                                                                        posicion1 = 2;
                                                                        posicion2 = 3;
                                                                    }
                                                                }
                                                            }

                                                            if (responseAux[2] != responseAux[3] && responseAux[4] != responseAux[3] && responseAux[2] != responseAux[4])
                                                            {
                                                                if (System.Convert.ToInt32(responseAux[2]) > System.Convert.ToInt32(responseAux[3]) && System.Convert.ToInt32(responseAux[2]) > System.Convert.ToInt32(responseAux[4]))
                                                                {
                                                                    mayor1 = System.Convert.ToInt32(responseAux[2]);
                                                                    posicion1 = 2;
                                                                    if (System.Convert.ToInt32(responseAux[3]) > System.Convert.ToInt32(responseAux[4]))
                                                                    {
                                                                        mayor2 = System.Convert.ToInt32(responseAux[3]);
                                                                        posicion2 = 3;
                                                                    }
                                                                    else
                                                                    {
                                                                        mayor2 = System.Convert.ToInt32(responseAux[4]);
                                                                        posicion2 = 4;
                                                                    }
                                                                }

                                                                if (System.Convert.ToInt32(responseAux[3]) > System.Convert.ToInt32(responseAux[2]) && System.Convert.ToInt32(responseAux[3]) > System.Convert.ToInt32(responseAux[4]))
                                                                {
                                                                    mayor1 = System.Convert.ToInt32(responseAux[3]);
                                                                    posicion1 = 3;
                                                                    if (System.Convert.ToInt32(responseAux[2]) > System.Convert.ToInt32(responseAux[4]))
                                                                    {
                                                                        mayor2 = System.Convert.ToInt32(responseAux[2]);
                                                                        posicion2 = 2;
                                                                    }
                                                                    else
                                                                    {
                                                                        mayor2 = System.Convert.ToInt32(responseAux[4]);
                                                                        posicion2 = 4;
                                                                    }
                                                                }

                                                                if (System.Convert.ToInt32(responseAux[4]) > System.Convert.ToInt32(responseAux[3]) && System.Convert.ToInt32(responseAux[4]) > System.Convert.ToInt32(responseAux[2]))
                                                                {
                                                                    mayor1 = System.Convert.ToInt32(responseAux[4]);
                                                                    posicion1 = 4;
                                                                    if (System.Convert.ToInt32(responseAux[2]) > System.Convert.ToInt32(responseAux[3]))
                                                                    {
                                                                        mayor2 = System.Convert.ToInt32(responseAux[2]);
                                                                        posicion2 = 2;
                                                                    }
                                                                    else
                                                                    {
                                                                        mayor2 = System.Convert.ToInt32(responseAux[3]);
                                                                        posicion2 = 3;
                                                                    }
                                                                }
                                                            }
                                                    }

                                                    int inicio = 0;
                                                    int resultado = 0;

                                                    if (posicion1 == 2)
                                                    {
                                                        inicio = 0;
                                                        if (posicion2 == 3)
                                                        {
                                                            resultado = mayor2 - mayor1;
                                                        }
                                                        else
                                                        {
                                                            resultado = mayor1 - mayor2;
                                                        }
                                                    }

                                                    if (posicion1 == 3)
                                                    {
                                                        inicio = -16;

                                                        if (posicion2 == 2)
                                                        {
                                                            resultado = mayor1 - mayor2;
                                                        }
                                                        else
                                                        {
                                                            resultado = mayor1 - mayor2;
                                                        }
                                                    }

                                                    if (posicion1 == 4)
                                                    {
                                                        inicio = 16;

                                                        if (posicion2 == 2)
                                                        {
                                                            resultado = mayor2 - mayor1;
                                                        }
                                                        else
                                                        {
                                                            resultado = mayor2 - mayor1;
                                                        }
                                                    }

                                                    inicio = inicio + resultado;

                                                    int vecesUsada = System.Convert.ToInt32(responseAux[1]) + 1;

                                                    string categoria = "";
                                                    int vecesUsadaA = 0, vecesUsadaL = 0, vecesUsadaG = 0;

                                                    if (inicio < -8)
                                                    {
                                                        categoria = "laboral";
                                                        vecesUsadaL = System.Convert.ToInt32(responseAux[3]) + 1;
                                                        Debug.Log(inicio + "");
                                                    }
                                                    if (inicio >= -8 && inicio <= 8)
                                                    {
                                                        categoria = "academica";
                                                        vecesUsadaA = System.Convert.ToInt32(responseAux[2]) + 1;
                                                        Debug.Log(inicio + "");
                                                    }
                                                    if (inicio > 8)
                                                    {
                                                        categoria = "general";
                                                        vecesUsadaG = System.Convert.ToInt32(responseAux[4]) + 1;
                                                        Debug.Log(inicio + "");
                                                    }


                                                    using (UnityWebRequest requestAuxTempTwo = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=7&palabras=" + category + "&id=" + responseAux[0] + "&vu=" + vecesUsada + "&vua=" + vecesUsadaA + "&vul=" + vecesUsadaL + "&vug=" + vecesUsadaG + "&categoria=" + categoria + "&punto=" + inicio))
                                                    {
                                                        yield return requestAuxTempTwo.SendWebRequest();
                                                        if (requestAuxTempTwo.isNetworkError) // Error
                                                        {
                                                            Debug.Log(requestAuxTempTwo.error);
                                                        }
                                                        else
                                                        {
                                                            Debug.Log(requestAuxTempTwo.downloadHandler.text);
                                                            flagAux++;
                                                        }
                                                    }
                                                }

                                                
                                            }
                                        }
                                    }
                                }

                             }
                        }

                    }

                }
            }

        }



    }

    public IEnumerator getGeneralitation()
    {
        string result = ""; 

        if (oracion != "")
        {
            int i = 0;
            foreach (string word in words)
            {
                int j = 0;
                do
                {
                    string tmp = "";

                    if (word.Length == 2 || word.Length == 1)
                    {
                        tmp = word;
                        j = 0;
                    }
                    else
                    {
                        
                        j++;
                        tmp = (word.Remove(word.Length - j));
                    }

                    

                    if(tmp != "es" && tmp != "a")
                    {

                        using (UnityWebRequest request = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=2&palabras=" + tmp))
                        {

                            yield return request.SendWebRequest();

                            if (request.isNetworkError) // Error
                            {
                                Debug.Log(request.error);
                            }
                            else // Success
                            {
                                result = request.downloadHandler.text;
                            }
                        }
                    }
                    else
                    {
                        result = "transicion";
                    }
                    
                } while (result == "ERROR");

                oracionEstructurada[i] = result;
                i++;
                
            }

            oracionEstructuradaSecond = oracionEstructurada;

            int error = -1;
            
            bool flag = false;

            foreach (string palabra in oracionEstructurada)
            {
                using (UnityWebRequest request = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=2&palabras=" + palabra))
                {

                    yield return request.SendWebRequest();

                    if (request.isNetworkError) // Error
                    {
                        Debug.Log(request.error);
                    }
                    else // Success
                    {
                        result = request.downloadHandler.text;
                        error++;
                    }
                }

                if (result == "ERROR")
                {

                    flag = true;

                    using (UnityWebRequest requestTwo = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=8&palabras=" + words[error]))
                    {

                        yield return requestTwo.SendWebRequest();

                        if (requestTwo.isNetworkError) // Error
                        {
                            Debug.Log(requestTwo.error);
                        }
                        else // Success
                        {
                            if (requestTwo.downloadHandler.text == "ERROR")
                            {
                                using (UnityWebRequest requestThree = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=9&palabras=" + words[error] + "&solicitada=" + 1))
                                {

                                    yield return requestThree.SendWebRequest();

                                    if (requestThree.isNetworkError) // Error
                                    {
                                        Debug.Log(requestThree.error);
                                    }
                                    else // Success
                                    {
                                        Debug.Log(requestThree.downloadHandler.text);
                                    }
                                }
                            } else
                            {
                                using (UnityWebRequest requestFour = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=10&palabras=" + words[error] + "&solicitada=" + (System.Convert.ToInt32(requestTwo.downloadHandler.text) + 1)))
                                {

                                    yield return requestFour.SendWebRequest();

                                    if (requestFour.isNetworkError) // Error
                                    {
                                        Debug.Log(requestFour.error);
                                    }
                                    else // Success
                                    {
                                        Debug.Log(requestFour.downloadHandler.text);
                                    }
                                }
                            }

                        }
                    }
                }

                    
            }

            if(flag == false)
            StartCoroutine(typeOfWord());

        }
            

    }

    public void reestructuration()
    {

        int i = 0;
        int j = -1;
        //newOracionEstructurada = oracionEstructurada;
        //newTiposDePalabras = tiposDePalabras;

        string complete2 = "";
        foreach (string word in oracionEstructurada)
        {
            complete2 = complete2 + word + " ";
        }
        Debug.Log(complete2);

        foreach (string word in tiposDePalabras)
        {
            j++;
            if(word == "tiempo ")
            {
                newOracionEstructurada[i] = oracionEstructurada[j];
                newTiposDePalabras[i] = word;
                i++;
            }
            if(word == "posesivo ")
            {
                if (i > 0)
                {
                    if(newTiposDePalabras[i - 1] != "posesivo ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                        
                    }
                }
                else
                {
                    if (oracionEstructurada[i] != "el ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                    }
                       
                }
            }
        }

        j = -1;

        foreach (string word in tiposDePalabras)
        {
            j++;
            if (word == "lugar ")
            {
                newOracionEstructurada[i] = oracionEstructurada[j];
                newTiposDePalabras[i] = word;
                i++;
            }
            if (word == "posesivo ")
            {
                if (i > 0)
                {
                    if (newTiposDePalabras[i - 1] != "posesivo ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                    }
                }
            }
        }

        j = -1;

        foreach (string word in tiposDePalabras)
        {
            j++;

            if (word == "posesivo " && oracionEstructurada[j] != "no ")
            {
                if (i > 0)
                {
                    if (newTiposDePalabras[i - 1] != "posesivo ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                    }
                }
            }
        }

        j = -1;

        foreach (string word in tiposDePalabras)
        {
            j++;

            if (word == "sujeto ")
            {
                newOracionEstructurada[i] = oracionEstructurada[j];
                newTiposDePalabras[i] = word;
                i++;
            }
            if (word == "posesivo ")
            {
                if (i > 0)
                {
                    if (newTiposDePalabras[i - 1] != "posesivo ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                    }
                }
            }
        }

        j = -1;

        foreach (string word in tiposDePalabras)
        {
            j++;
            if (word == "objeto ")
            {
                newOracionEstructurada[i] = oracionEstructurada[j];
                newTiposDePalabras[i] = word;
                i++;
            }
            if (word == "posesivo ")
            {
                if (i > 0)
                {
                    if (newTiposDePalabras[i - 1] != "posesivo ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                    }
                }
            }
        }

        j = -1;

        foreach (string word in tiposDePalabras)
        {
            j++;
            if (word == "verbo ")
            {
                
                newOracionEstructurada[i] = oracionEstructurada[j];
                
                newTiposDePalabras[i] = word;
                i++;
            }
            if (word == "posesivo ")
            {
                if (i > 0)
                {
                    if (newTiposDePalabras[i - 1] != "posesivo ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                    }
                }
            }
        }

        j = -1;

        foreach (string word in tiposDePalabras)
        {
            j++;
            if (word == "complemento ")
            {

                newOracionEstructurada[i] = oracionEstructurada[j];

                newTiposDePalabras[i] = word;
                i++;
            }
            if (word == "posesivo ")
            {
                if (i > 0)
                {
                    if (newTiposDePalabras[i - 1] != "posesivo ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                    }
                }
            }
        }

        j = -1;

        foreach (string word in tiposDePalabras)
        {
            j++;
            if (word == "auxiliar ")
            {

                newOracionEstructurada[i] = oracionEstructurada[j];

                newTiposDePalabras[i] = word;
                i++;
            }
            if (word == "posesivo ")
            {
                if (i > 0)
                {
                    if (newTiposDePalabras[i - 1] != "posesivo ")
                    {
                        newOracionEstructurada[i] = oracionEstructurada[j];
                        newTiposDePalabras[i] = word;
                        tiposDePalabras[j] = "NULL";
                        i++;
                    }
                }
            }
        }

        if (newOracionEstructurada[newOracionEstructurada.Length - 2] == (newOracionEstructurada[newOracionEstructurada.Length - 1] + " "))
        {
            newOracionEstructurada[newOracionEstructurada.Length - 1] = " ";
        }

        string complete = "";
        foreach(string word in newOracionEstructurada)
        {
            complete = complete + word + " ";
        }
        Debug.Log(complete);

        StartCoroutine(GetText());


    }

    public IEnumerator typeOfWord()
    {
        int i = 0;
        string[] arrayAux = oracionEstructurada;

        foreach (string word in arrayAux)
        {

            using (UnityWebRequest request = UnityWebRequest.Get("https://talktolsmex.000webhostapp.com/?function=3&palabras=" + word))
                {
                yield return request.SendWebRequest();
                if (request.isNetworkError) // Error
                    {
                        Debug.Log(request.error);
                }
                    else // Success
                    {
                    tiposDePalabras[i] = request.downloadHandler.text;
                    newTiposDePalabras[i] = request.downloadHandler.text;
                    i++;
                }

                }

        }

        reestructuration();

    }

    }
