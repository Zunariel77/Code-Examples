using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour {

	public PlayerController controller;
	public Transform pool;
	public int maxHealth = 100;
	public int health;
	public Slider healthBar;
	public float hitTimer;
	public Material mat;
	public Color originalColor;
	public Color signalColor;

	[System.Serializable]
	public class Canon
	{
		public Transform pos;
		public GameObject prefab;
		public float attackrate;
		public float aTimer;
		public float aimAngle;

	}

	public List<Canon> canonList = new List<Canon>();

	void Start () {
		controller = GetComponent<PlayerController> ();
		pool = GameObject.Find ("Projectile_Pool").transform;

		health = maxHealth;

		healthBar = GameObject.Find ("Healthbar").GetComponent<Slider> ();
		healthBar.minValue = 0;
		healthBar.maxValue = maxHealth;
		healthBar.value = health;

        foreach(Canon can in canonList)
        { 			
			can.attackrate = can.prefab.GetComponent<Projectile> ().attackrate;
			can.pos.Rotate (0, 0, can.aimAngle);
		}
		originalColor = mat.color;
	}
	

	void Update () {
		if (Input.GetMouseButton (0)) {
            foreach(Canon can in canonList)
            {				
				if (can.aTimer <= 0) {
					can.aTimer = can.attackrate;
					if (can.prefab != null) {
						Quaternion rot = controller.rot;
						rot = can.pos.rotation;
						GameObject obj = Instantiate (can.prefab, can.pos.position, rot);
						obj.transform.parent = pool;
					}
				}
			}
		}

        foreach (Canon can in canonList)
        {
            if (can.aTimer > 0) {
				can.aTimer -= 1 * Time.deltaTime;
			}
		}

		if (hitTimer > 0) {
			hitTimer -= 1 * Time.deltaTime;
		} else {
			mat.color = originalColor;
		}			
	}

	public void Damage(int dmg)
	{
		health -= dmg;
		mat.color = signalColor;
		hitTimer = 0.2f;
			
		healthBar.value = health;

		if (health <= 0) {
			mat.color = originalColor;
			Death ();
		}
	}

	public void Death()
	{
		Destroy (gameObject);
		GameMaster gm = GameObject.Find ("GameMaster").GetComponent < GameMaster> ();
		AsteroidPool aP = GameObject.Find ("GameMaster").GetComponent < AsteroidPool> ();

		gm.GameOver ();
		aP.over = true;
	}
}
