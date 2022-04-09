using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon {

	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }

	private Movement movement;

	protected SO_AggressiveWeaponData aggressiveWeaponData;

	private List<IDamageable> detectedDamageables = new List<IDamageable>();
	private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();

	protected override void Awake() {
		base.Awake();

		if (weaponData.GetType() == typeof(SO_AggressiveWeaponData)) {
			aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
		} else {
			Debug.LogError("Wrong data for the weapon");
		}
	}

	public override void AnimationActionTrigger() {
		base.AnimationActionTrigger();

		CheckMeleeAttack();
	}

	private void CheckMeleeAttack() {
		WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];

		foreach (IDamageable item in detectedDamageables.ToList()) {
			item.Damage(details.damageAmount);
		}

		foreach (IKnockbackable item in detectedKnockbackables.ToList()) {
			item.Knockback(details.knockbackAngle, details.knockbackStrength, Movement.FacingDirection);
		}
	}

	public void AddToDetected(Collider2D collision) {

		IDamageable damageable = collision.GetComponent<IDamageable>();

		if (damageable != null) {
			detectedDamageables.Add(damageable);
		}

		IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();

		if (knockbackable != null) {
			detectedKnockbackables.Add(knockbackable);
		}
	}

	public void RemoveFromDetected(Collider2D collision) {
		IDamageable damageable = collision.GetComponent<IDamageable>();

		if (damageable != null) {
			detectedDamageables.Remove(damageable);
		}

		IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();

		if (knockbackable != null) {
			detectedKnockbackables.Remove(knockbackable);
		}
	}

}
