using UnityEngine;

using UnityEngine.UI;
public class ButtonEffect : MonoBehaviour {

    public void OnButtonHoverEnter(GameObject btn)
    {
        RectTransform transform = btn.GetComponent<RectTransform>();
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

        ParticleSystem[] particles = btn.GetComponentsInChildren<ParticleSystem>(true);

        if (particles.Length > 0)
        {
            foreach (ParticleSystem p in particles)
            {
                TrailRenderer tr = p.GetComponentInChildren<TrailRenderer>();
                if (tr)
                {
                    tr.enabled = true;
                    if (!p.isPlaying)
                        p.Play();
                }
            }
        }
    }

    public void OnButtonHoverExit(GameObject btn)
    {
        btn.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        StopAllCoroutines();
        Image img = btn.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);

        ParticleSystem[] particles = btn.GetComponentsInChildren<ParticleSystem>(true);

        if (particles.Length > 0)
        {

            foreach (ParticleSystem p in particles)
            {
                TrailRenderer tr = p.GetComponentInChildren<TrailRenderer>();
                if (tr)
                {
                    tr.enabled = false;
                    if (p.isPlaying)
                        p.Stop();
                }
            }
        }
    }

}
