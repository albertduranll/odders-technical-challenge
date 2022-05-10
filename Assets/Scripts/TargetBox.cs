using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBox : MonoBehaviour
{
    public GameObject explosion;

    private void OnEnable()
    {
        explosion.SetActive(false);
    }

    public void HitRecieved()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        GameManager.instance.audioController.PlayOneShotSound(GetComponent<AudioSource>(), null);

        explosion.SetActive(true);

        Invoke("DeactivateGameObject", .4f);
    }

    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
