using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    GameManager manager => GameManager.instance;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    [SerializeField] private InputActionReference triggerReference = null;
    [SerializeField] private InputActionReference gameInfoReference = null;

    private List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField] private int bulletPoolCapacity = 15;

    void Awake()
    {
        triggerReference.action.started += PullTheTrigger;
        gameInfoReference.action.started += ManageGameStatus;
    }

    private void OnDestroy()
    {
        triggerReference.action.started -= PullTheTrigger;
        gameInfoReference.action.started -= ManageGameStatus;
    }

    /// <summary>
    /// Method that instantiates the bullets until we fill the pool and then we just recycle them.
    /// </summary>
    /// <param name="context"></param>
    public void PullTheTrigger(InputAction.CallbackContext context)
    {
        if (!manager.isPaused)
        {
            manager.audioController.PlayOneShotSound(manager.audioController.gunAudioSource, manager.audioController.bulletShot);

            if(bulletPool.Count < bulletPoolCapacity)
            {
                BulletGenerator();
            }
            else
            {
                BulletPoolRecycle();
            }
        }
    }

    /// <summary>
    /// Method that handles the game status (resume or pause the game).
    /// </summary>
    /// <param name="context"></param>
    public void ManageGameStatus(InputAction.CallbackContext context)
    {
        manager.SetPauseOrResume();
    }


    private void BulletGenerator()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint);
        newBullet.name = $"Bullet {bulletPool.Count}";
        newBullet.GetComponent<Renderer>().material.color = Color.green;
        bulletPool.Add(newBullet);
    }

    private void BulletPoolRecycle()
    {
        GameObject bullet = bulletPool[0];
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        bullet.SetActive(true);

        bulletPool.RemoveAt(0);
        bulletPool.Add(bullet);
    }
}
