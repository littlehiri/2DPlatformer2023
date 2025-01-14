using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Para cambiar entre escenas

public class LevelManager : MonoBehaviour
{
    //Tiempo antes de respawnear
    public float waitToRespawn;

    //Variable para el contador de gemas
    public int gemCollected;

    //Variable para guardar el nombre del nivel al que queremos ir
    public string levelToLoad;

    //Hacemos el Singleton de este script
    public static LevelManager sharedInstance;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //M�todo para respawnear al jugador cuando muere
    public void RespawnPlayer()
    {
        //Llamamos a la corrutina que respawnea al jugador
        StartCoroutine(RespawnPlayerCo());
    }

    //Corrutina para respawnear al jugador
    public IEnumerator RespawnPlayerCo()
    {
        //Desactivamos al jugador
        PlayerController.sharedInstance.gameObject.SetActive(false);
        //Llamamos al sonido de muerte
        AudioManager.sharedInstance.PlaySFX(8);
        //Esperamos un tiempo determinado
        yield return new WaitForSeconds(waitToRespawn);
        //Llamamos al m�todo que hace fundido a negro
        UIController.sharedInstance.FadeToBlack();
        //Esperamos un tiempo determinado
        yield return new WaitForSeconds(waitToRespawn);
        //Llamamos al m�todo que hace fundido a transparente
        UIController.sharedInstance.FadeFromBlack();
        //Activamos de nuevo al jugador
        PlayerController.sharedInstance.gameObject.SetActive(true);
        //Lo ponemos en la posici�n de respawn
        PlayerController.sharedInstance.transform.position = CheckpointController.sharedInstance.spawnPoint;
        //Ponemos la vida del jugador al m�ximo
        PlayerHealthController.sharedInstance.currentHealth = PlayerHealthController.sharedInstance.maxHealth;
        //Actualizamos la UI
        UIController.sharedInstance.UpdateHealthDisplay();
    }

    //M�todo para terminar un nivel
    public void ExitLevel()
    {
        //Llamamos a la corrutina
        StartCoroutine(ExitLevelCo());
    }

    //Corrutina de terminar un nivel
    private IEnumerator ExitLevelCo()
    {
        //Paramos los inputs del jugador
        PlayerController.sharedInstance.stopInput = true;
        //Paramos el movimiento del jugador
        PlayerController.sharedInstance.StopPlayer();
        //Paramos la m�sica del nivel
        AudioManager.sharedInstance.bgm.Stop();
        //Reproducimos la m�sica de ganar el nivel
        AudioManager.sharedInstance.levelEndMusic.Play();
        //Mostramos el cartel de haber finalizado el nivel
        UIController.sharedInstance.levelCompleteText.gameObject.SetActive(true);
        //Esperamos un tiempo determinado
        yield return new WaitForSeconds(1.5f);
        //Fundido a negro
        UIController.sharedInstance.FadeToBlack();
        //Esperamos un tiempo determinado
        yield return new WaitForSeconds(1.5f);

        //Ir a la pantalla de carga
        SceneManager.LoadScene(levelToLoad);
    }
}
