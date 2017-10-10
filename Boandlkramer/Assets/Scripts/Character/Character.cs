using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour {

	public Slider healthBar;

	public CharacterData data = new CharacterData ();

	public void Attack (Character other) {

		if (Hit (other))
			other.Damage (GetAttackDamage ());
        else
        {
            Debug.Log("Missed...!");
        }
	}

	public float GetAttackSpeed () {

		return .2f;
	}

	public int GetAttackDamage () {

		return 30;
	}

	public int GetAttack () {

		return data.dexterity;
	}

	public int GetDefense () {

		return data.dexterity;
	}

	public void Damage (int dmg) {

		if (dmg > 0) {
			ChangeHealth (-dmg);
			Debug.Log ("Take " + dmg.ToString () + " damage. " + data.health.ToString () + " health remaining.");
		}	
        else
        {
            Debug.Log("Missed :( 0 Damage done!");
        }
		if (data.health <= 0)
			Death ();
	}

	void ChangeHealth (int amount) {
		data.health += amount;
		if (healthBar != null)
			healthBar.value = (float) data.health / (float) data.maxHealth;
	}

	bool Hit (Character other) {

		int rng = Random.Range (1, GetAttack () + other.GetDefense ());
		return rng <= GetAttack ();
	}

	void Death () {
		Debug.Log ("Death!");
		Destroy (gameObject);
	}
}
