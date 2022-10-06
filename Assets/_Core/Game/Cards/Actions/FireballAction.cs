using Common;
using PureAnimator;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Actions/Create New Card")]
public class FireballAction : BaseActions
{
    public TargetEffect target;
    public int damage;
    public float speed;
    public ParticleSystem particleSystemPrefab;
    public ParticleSystem explosionPrefab;

    public override void Cast(BaseCharacter self, BaseCharacter[] other)
    {
        var effect = Object.Instantiate(particleSystemPrefab, self.frontEffectSpawn.position, Quaternion.identity);

        var dist = Vector3.Distance(self.transform.position, other[0].transform.position);
        var shortDist = Vector3.Distance(
            new Vector3(self.transform.position.x, 0, self.transform.position.z),
            new Vector3(self.frontEffectSpawn.position.x, 0, self.frontEffectSpawn.position.z)
            );
        var endPoint = self.frontEffectSpawn.position + self.transform.forward * (dist - shortDist);

        Services<PureAnimatorController>
            .Get()
            .GetPureAnimator()
            .Play(dist / speed, progress =>
            {
                effect.transform.position = Vector3.Lerp(
                    self.frontEffectSpawn.position,
                    endPoint,
                    progress);
                return default;
            }, () => 
            {
                other[0].Damage(damage);
                Destroy(effect.gameObject);
                var explosion = Object.Instantiate(explosionPrefab, endPoint, Quaternion.identity);
                Services<PureAnimatorController>
                    .Get()
                    .GetPureAnimator()
                    .Play(explosion.main.startLifetime.constant, progress =>
                    {
                        return default;
                    }, () =>
                    {
                        Destroy(explosion.gameObject);
                        FinishedCast();
                    });
            });
    }

    public override void End()
    {
        
    }

    public override void Tick(BaseCharacter self, BaseCharacter other)
    {
        
    }
}
