using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;

public class WaterInteraction : MonoBehaviour
{
    [Header("Water Settings")]
    public Material waterMaterial;
    public Color[] touchColors = { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta };
    public float colorChangeRadius = 0.5f;
    public float colorBlendSpeed = 2f;
    
    [Header("Touch Detection")]
    public LayerMask fingerLayer = 1;
    
    private Color currentColor;
    private Color targetColor;
    private int currentColorIndex = 0;
    
    void Start()
    {
        // 初期色を設定
        if (waterMaterial != null)
        {
            currentColor = waterMaterial.color;
            targetColor = currentColor;
        }
    }

    void Update()
    {
        // 色の変化をスムーズに適用
        if (waterMaterial != null && currentColor != targetColor)
        {
            currentColor = Color.Lerp(currentColor, targetColor, colorBlendSpeed * Time.deltaTime);
            waterMaterial.color = currentColor;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // 指のコライダーかチェック
        if (IsFingerCollider(other))
        {
            ChangeWaterColor();
        }
    }
    
    private bool IsFingerCollider(Collider collider)
    {
        // Meta XR Hand や Controller の指部分を検出
        return collider.CompareTag("Finger") || 
               collider.name.Contains("Index") || 
               collider.name.Contains("Finger") ||
               collider.name.Contains("HandIndex") ||
               collider.name.Contains("b_r_index") ||
               collider.name.Contains("b_l_index") ||
               ((1 << collider.gameObject.layer) & fingerLayer) != 0;
    }
    
    private void ChangeWaterColor()
    {
        // 次の色に変更
        currentColorIndex = (currentColorIndex + 1) % touchColors.Length;
        targetColor = touchColors[currentColorIndex];
        
        // 触れた時のエフェクト（オプション）
        CreateRippleEffect();
    }
    
    private void CreateRippleEffect()
    {
        // 簡単な波紋エフェクト（シェーダーで実装する場合）
        if (waterMaterial.HasProperty("_RippleTime"))
        {
            waterMaterial.SetFloat("_RippleTime", Time.time);
        }
    }
}
