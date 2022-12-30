using UnityEngine;

public interface IDamage
{
    void OnDamage(float damage, Vector3 hitPoint);
}

public interface IAttack
{
    void Attack();
}