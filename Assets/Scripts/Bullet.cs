using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    int reflectionCount = 0;

    private void Start()
    {
        transform.parent = null;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "targetBox")
        {
            other.GetComponent<TargetBox>().HitRecieved();
            //other.gameObject.SetActive(false);
            gameObject.SetActive(false);

            GameManager.instance.IncreaseScore();
        }
        else if(other.tag == "boxWall")
        {
            if(reflectionCount < 2)
            {
                BulletReflection();

                GameManager.instance.audioController.PlaySound(GetComponent<AudioSource>(), GameManager.instance.audioController.bulletBounce);
            }
            else
            {
                gameObject.SetActive(false);
                reflectionCount = 0;
            }
        }
        else if (other.tag == "startButton")
        {
            gameObject.SetActive(false);
            GameManager.instance.audioController.PlayOneShotSound(GameManager.instance.audioController.gunAudioSource, GameManager.instance.audioController.menuSelection);
            GameManager.instance.ActivateGame();
        }
        else if (other.tag == "exitButton")
        {
            gameObject.SetActive(false);
            Application.Quit();
        }
    }

    /// <summary>
    /// Method that calculate the bullet reflection with current direction and the normal direction.
    /// </summary>
    private void BulletReflection()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 15f))
        {
            Vector3 reflectV = Vector3.Reflect(transform.forward, hit.normal);

            transform.position = hit.point;
            transform.rotation = Quaternion.LookRotation(reflectV);
            reflectionCount++;
        }
    }
}
