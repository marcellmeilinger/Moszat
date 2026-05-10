using UnityEngine;

/// <summary>
/// A háttér parallax hatásáért felelős osztály, amely mélységérzetet ad a 2D játéktérnek
/// azáltal, hogy a hátteret a kamerához képest eltérő sebességgel mozgatja.
/// </summary>
public class Parallax : MonoBehaviour
{
    private float length, startPos;
    public GameObject cam;
    public float parallaxEffect; 

    /// <summary>
    /// Kezdeti beállítások elvégzése, a kamera megkeresése és a sprite szélességének meghatározása.
    /// </summary>
    void Start()
    {
        if (cam == null) cam = GameObject.Find("Main Camera");

        startPos = transform.position.x;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    /// <summary>
    /// A háttér pozíciójának frissítése minden fizikai lépésnél, a kamera mozgása alapján.
    /// Végtelen görgetést (loop) is biztosít, ha a kamera túl messze megy.
    /// </summary>
    void FixedUpdate()
    {
        if (cam != null)
        {
            float temp = (cam.transform.position.x * (1 - parallaxEffect));

            float dist = (cam.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

            if (temp > startPos + length)
            {
                startPos += length;
            }
            else if (temp < startPos - length)
            {
                startPos -= length;
            }
        }
    }
}