using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DirectionalMeleeWeapon))]
public class DirectionalWeaponVFX : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private Image slashImage;

    [Header("Visual References (Trail)")]
    [SerializeField] private Transform trailPivot;      
    [SerializeField] private Transform trailTip;        
    [SerializeField] private TrailRenderer weaponTrail;

    [Header("Timing & Game Feel")]
    [SerializeField] private float swingDuration = 0.05f; // Lo que tarda en hacer el barrido
    [SerializeField] private AnimationCurve swingCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Space(10)]
    [SerializeField] private float fadeDuration = 0.2f;   // Lo que tarda en disiparse
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

    private DirectionalMeleeWeapon weapon;
    private Coroutine currentVFXCoroutine;

    private void Awake()
    {
        weapon = GetComponent<DirectionalMeleeWeapon>();

        if (slashImage != null)
        {
            Color startColor = slashImage.color;
            startColor.a = 0f;
            slashImage.color = startColor;
        }

        if (weaponTrail != null) weaponTrail.emitting = false;
    }

    private void OnEnable()
    {
        weapon.OnWeaponFired += PlaySlashVFX;
    }

    private void OnDisable()
    {
        weapon.OnWeaponFired -= PlaySlashVFX;
    }

    private void PlaySlashVFX()
    {
        if (slashImage == null) return;

        if (currentVFXCoroutine != null)
        {
            StopCoroutine(currentVFXCoroutine);
        }

        currentVFXCoroutine = StartCoroutine(AnimateSlash());
    }

    /*private IEnumerator AnimateSlash()
    {
        
        // Fill Amount va de 0 a 1. (0 / 360)
        // Scale compensation
        slashImage.fillAmount = weapon.ConeAngle / 360f;

        float targetWorldDiameter = weapon.AttackRadius * 2f;

        float currentScale = slashImage.transform.lossyScale.x;

        float correctedSize = targetWorldDiameter / currentScale;

        slashImage.rectTransform.sizeDelta = new Vector2(correctedSize, correctedSize);

        //// RectTransform controls UI size. Match radius(radius * 2)
        //slashImage.rectTransform.sizeDelta = new Vector2(weapon.AttackRadius * 2f, weapon.AttackRadius * 2f);

        // Rotate to player
        float targetFill = weapon.ConeAngle / 360f;
        float centerAngle = Mathf.Atan2(weapon.FacingDirection.y, weapon.FacingDirection.x) * Mathf.Rad2Deg;

        // Note: Fill amount starts at border, not center
        // Substract half of the cone angle to center
        slashImage.transform.rotation = Quaternion.Euler(0, 0, centerAngle - (weapon.ConeAngle / 2f));

        float startAngle = centerAngle - (weapon.ConeAngle / 2f);
        float endAngle = centerAngle + (weapon.ConeAngle / 2f);

        if (slashTrail != null && trailTip != null)
        {
            slashTrail.Clear(); 
            slashTrail.emitting = true; 
        }

        Color slashColor = slashImage.color;
        slashColor.a = 0.8f;
        slashImage.color = slashColor;

        // Swing
        float swingTimer = 0f;
        while (swingTimer < swingDuration)
        {
            swingTimer += Time.deltaTime;

            // Calculamos el porcentaje de tiempo (de 0.0 a 1.0)
            float timePercent = swingTimer / swingDuration;

            // Evaluamos ese porcentaje en nuestra curva personalizada
            float curveMultiplier = swingCurve.Evaluate(timePercent);

            slashImage.fillAmount = Mathf.Lerp(0f, targetFill, curveMultiplier);

            if (trailTip != null)
            {
                // Calculamos en qué ángulo exacto estamos ahora mismo
                float currentAngle = Mathf.Lerp(startAngle, endAngle, curveMultiplier);

                // Convertimos a Radianes para la trigonometría
                float rad = currentAngle * Mathf.Deg2Rad;

                // Calculamos la posición en el borde del círculo (usamos posición absoluta para evitar problemas de escala)
                Vector3 tipOffset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * weapon.AttackRadius;
                trailTip.position = weapon.transform.position + tipOffset;
            }

            yield return null;
        }


        slashImage.fillAmount = targetFill;
        if (slashTrail != null) slashTrail.emitting = false;

        // Fade out
        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;

            float timePercent = fadeTimer / fadeDuration;
            float curveMultiplier = fadeCurve.Evaluate(timePercent);

            // Multiplicamos el alfa máximo (0.8f) por el valor de la curva
            slashColor.a = 0.8f * curveMultiplier;
            slashImage.color = slashColor;

            yield return null;
        }

        slashColor.a = 0f;
        slashImage.color = slashColor;
    }
    */

    private IEnumerator AnimateSlash()
    {
        float currentScale = slashImage.transform.lossyScale.x;
        float targetWorldDiameter = weapon.AttackRadius * 2f;
        float correctedSize = targetWorldDiameter / currentScale;
        slashImage.rectTransform.sizeDelta = new Vector2(correctedSize, correctedSize);

        // Angles and positions
        float baseAngle = Mathf.Atan2(weapon.FacingDirection.y, weapon.FacingDirection.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - (weapon.ConeAngle / 2f);
        float endAngle = baseAngle + (weapon.ConeAngle / 2f);
        float targetFill = weapon.ConeAngle / 360f;

        // Image initial settings
        slashImage.transform.rotation = Quaternion.Euler(0, 0, startAngle);
        Color slashColor = slashImage.color;
        slashColor.a = 0.8f;
        slashImage.color = slashColor;

        if (trailPivot != null && trailTip != null && weaponTrail != null)
        {
            trailTip.localPosition = new Vector3(weapon.AttackRadius, 0f, 0f);

            trailPivot.rotation = Quaternion.Euler(0, 0, startAngle);

            weaponTrail.Clear();
            weaponTrail.emitting = true;
        }

        float swingTimer = 0f;
        while (swingTimer < swingDuration)
        {
            swingTimer += Time.deltaTime;
            float timePercent = swingTimer / swingDuration;
            float curveMultiplier = swingCurve.Evaluate(timePercent);

            slashImage.fillAmount = Mathf.Lerp(0f, targetFill, curveMultiplier);

            if (trailPivot != null)
            {
                // LerpAngle for 360° rotations
                float currentAngle = Mathf.LerpAngle(startAngle, endAngle, curveMultiplier);
                trailPivot.rotation = Quaternion.Euler(0, 0, currentAngle);
            }

            yield return null;
        }

        slashImage.fillAmount = targetFill;
        if (trailPivot != null) trailPivot.rotation = Quaternion.Euler(0, 0, endAngle);
        if (weaponTrail != null) weaponTrail.emitting = false; 

        
        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float timePercent = fadeTimer / fadeDuration;
            float curveMultiplier = fadeCurve.Evaluate(timePercent);

            slashColor.a = 0.8f * curveMultiplier;
            slashImage.color = slashColor;

            yield return null;
        }

        slashColor.a = 0f;
        slashImage.color = slashColor;
    }

}