using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreUIText, countUIText;
    public static ScoreManager instance;
    [SerializeField] GameObject[] coleccionables;
    [SerializeField] int count = 0;
    public int puntaje = 0;
    string mensaje = "Puntaje: ";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        coleccionables = GameObject.FindGameObjectsWithTag("Coleccionable");
        scoreUIText.text = mensaje + puntaje;
        countUIText.text = count + "/" + coleccionables.Length;
    }
    public void ColeccionableNormal()
    {
        puntaje++;
        count++;
        scoreUIText.text = mensaje + puntaje;
        countUIText.text = count + "/" + coleccionables.Length;
        if (count == coleccionables.Length)
        {
            ReloadScene();
        }
        Debug.Log("Puntaje:" + puntaje);
    }
    public void ColeccionableEspecial()
    {
        puntaje += 3;
        scoreUIText.text = mensaje + puntaje;
        Debug.Log("Puntaje:" + puntaje);
    }
    public void ColeccionableTrampa()
    {
        puntaje--;
        scoreUIText.text = mensaje + puntaje;
        Debug.Log("Puntaje:" + puntaje);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
