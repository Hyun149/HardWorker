using UnityEngine;

/// <summary>
/// 칼질 공격과 관련된 파티클 설정을 정의하는 클래스입니다.
/// - 일반 공격과 크리티컬 공격에 대한 색상, 크기, 수량 등을 설정할 수 있습니다.
/// - 해당 설정을 기반으로 ParticleSystem에 세부 속성을 적용하는 정적 메서드를 제공합니다.
/// </summary>
[System.Serializable]
public class AttackParticleSettings
{
    [Header("일반 칼질 파티클 설정")]
    [SerializeField] private Color normalAttackColor = new Color(0.9f, 0.9f, 0.9f, 1f); // 은빛 칼날
    [SerializeField] private float normalParticleSize = 0.3f;
    [SerializeField] private int normalParticleCount = 15;

    [Header("완벽한 손질 파티클 설정")]
    [SerializeField] private Color criticalAttackColor = new Color(1f, 0.9f, 0.4f, 1f); // 황금빛 반짝임
    [SerializeField] private float criticalParticleSize = 0.5f;
    [SerializeField] private int criticalParticleCount = 30;

    /// <summary>
    /// 일반 공격 시 파티클 시스템의 설정을 적용합니다.
    /// </summary>
    /// <param name="particleSystem">설정을 적용할 파티클 시스템</param>
    /// <param name="settings">사용할 파티클 설정 데이터</param>
    public static void SetupNormalAttackParticle(ParticleSystem particleSystem, AttackParticleSettings settings)
    {
        var main = particleSystem.main;
        main.startLifetime = 0.3f;
        main.startSpeed = 8f;
        main.startSize = settings.normalParticleSize;
        main.startColor = settings.normalAttackColor;
        main.maxParticles = settings.normalParticleCount;

        var emission = particleSystem.emission;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0.0f, settings.normalParticleCount)
        });

        // 칼날이 지나가는 듯한 모양
        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Rectangle;
        shape.scale = new Vector3(0.8f, 0.1f, 0f);
        shape.rotation = new Vector3(0f, 0f, 45f); // 대각선 각도

        // 칼날의 빠른 움직임 표현 - 수정된 부분
        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;

        // 모든 축을 동일한 모드로 설정
        AnimationCurve xCurve = AnimationCurve.Linear(0, 3f, 1, 3f);
        AnimationCurve yCurve = AnimationCurve.Linear(0, 0f, 1, 0f);
        AnimationCurve zCurve = AnimationCurve.Linear(0, 0f, 1, 0f);

        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(1f, xCurve);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(1f, yCurve);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(1f, zCurve);

        // 빠르게 사라지는 효과
        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.white, 0.0f),
                new GradientColorKey(Color.white, 0.5f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorOverLifetime.color = gradient;
    }

    /// <summary>
    /// 완벽한 손질 시 파티클 시스템의 설정을 적용합니다.
    /// </summary>
    /// <param name="particleSystem">설정을 적용할 파티클 시스템</param>
    /// <param name="settings">사용할 파티클 설정 데이터</param>
    public static void SetupCriticalAttackParticle(ParticleSystem particleSystem, AttackParticleSettings settings)
    {
        var main = particleSystem.main;
        main.startLifetime = 0.6f;
        main.startSpeed = 10f;
        main.startSize = settings.criticalParticleSize;
        main.startColor = settings.criticalAttackColor;
        main.maxParticles = settings.criticalParticleCount;

        var emission = particleSystem.emission;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0.0f, settings.criticalParticleCount)
        });

        // 십자 모양의 완벽한 칼질
        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.1f;

        // 빛나는 파티클이 사방으로 퍼지는 효과 - 수정된 부분
        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.space = ParticleSystemSimulationSpace.Local;
        velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(8f);

        // 모든 축을 0으로 설정
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0f);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);

        // 황금빛 반짝임
        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 1f, 1f), 0.0f),
                new GradientColorKey(new Color(1f, 0.9f, 0.4f), 0.3f),
                new GradientColorKey(new Color(1f, 0.8f, 0.2f), 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.5f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorOverLifetime.color = gradient;

        // 크기가 시간에 따라 변화 (반짝임 강조)
        var sizeOverLifetime = particleSystem.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0f, 0.5f);
        sizeCurve.AddKey(0.2f, 1.5f);
        sizeCurve.AddKey(0.4f, 1.2f);
        sizeCurve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        // 회전하는 빛 효과
        var rotationOverLifetime = particleSystem.rotationOverLifetime;
        rotationOverLifetime.enabled = true;
        rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(180f, 360f);
    }
}