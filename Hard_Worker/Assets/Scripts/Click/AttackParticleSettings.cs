using UnityEngine;

[System.Serializable]
public class AttackParticleSettings
{
    [Header("일반 공격 파티클 설정")]
    public Color normalAttackColor = Color.white;
    public float normalParticleSize = 0.5f;
    public int normalParticleCount = 10;

    [Header("치명타 파티클 설정")]
    public Color criticalAttackColor = Color.red;
    public float criticalParticleSize = 1.0f;
    public int criticalParticleCount = 20;

    // 일반 공격 파티클 시스템 설정
    public static void SetupNormalAttackParticle(ParticleSystem particleSystem, AttackParticleSettings settings)
    {
        var main = particleSystem.main;
        main.startLifetime = 0.5f;
        main.startSpeed = 5f;
        main.startSize = settings.normalParticleSize;
        main.startColor = settings.normalAttackColor;
        main.maxParticles = settings.normalParticleCount;

        var emission = particleSystem.emission;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0.0f, settings.normalParticleCount)
        });

        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.1f;

        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(3f);
    }

    // 치명타 파티클 시스템 설정
    public static void SetupCriticalAttackParticle(ParticleSystem particleSystem, AttackParticleSettings settings)
    {
        var main = particleSystem.main;
        main.startLifetime = 0.8f;
        main.startSpeed = 8f;
        main.startSize = settings.criticalParticleSize;
        main.startColor = settings.criticalAttackColor;
        main.maxParticles = settings.criticalParticleCount;

        var emission = particleSystem.emission;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0.0f, settings.criticalParticleCount)
        });

        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.2f;

        var velocityOverLifetime = particleSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.radial = new ParticleSystem.MinMaxCurve(5f);

        // 치명타는 크기가 시간에 따라 커짐
        var sizeOverLifetime = particleSystem.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0f, 1f);
        sizeCurve.AddKey(0.5f, 1.5f);
        sizeCurve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);
    }
}