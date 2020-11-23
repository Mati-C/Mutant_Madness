using UnityEngine;
using UnityEngine.UI;

public class AnimationControllerCustom : MonoBehaviour
{
    public AnimationInfo[] animaciones;
    AnimationInfo animActual;

    float t;
    Image im;
    SpriteRenderer sr;
    int frame;

    void Start()
    {
        if (animaciones[0].isUI)
            im = GetComponent<Image>();
        else
            sr = GetComponent<SpriteRenderer>();
        CambiarAnim("Default");
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t < 1 / animActual.frameRate) return;
        frame++;
        t = 0;
        SetFrame();
    }

    public void CambiarAnim(string sigAnimacion)
    {
        for (int i = 0; i < animaciones.Length; i++)
            if (sigAnimacion == animaciones[i].nombre)
            {
                animActual = animaciones[i];
                frame = 0;
                SetFrame();
            }
    }

    public void SetFrame()
    {
        if (frame >= animActual.sprites.Length)
        {
            if (animActual.loopeable == true) frame = 0;
            else
            {
                frame--;
                if (animActual.sigAnimacion != "none") CambiarAnim(animActual.sigAnimacion);
            }
        }
        if (im != null)
            im.sprite = animActual.sprites[frame];
        else
            sr.sprite = animActual.sprites[frame];
    }
}

[System.Serializable]
public class AnimationInfo
{
    public string nombre;
    public Sprite[] sprites;
    public bool loopeable;
    public float frameRate;
    public string sigAnimacion = "none";
    public bool isUI;
}
