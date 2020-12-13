using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombieScript : MonoBehaviour
{   
    public GameObject player;
	public int type;
	public int maxHealth;
	public float moveSpeed;
    int health;
    Rigidbody2D rb;
    bool isFacingRight;
    SpriteRenderer sRender;

    // Start is called before the first frame update
    void Start()
    {
		type = TipoZombie();
		maxHealth = VidaZombie();
		moveSpeed = VelocidadeZombie();
        isFacingRight = true;
        rb = gameObject.GetComponent<Rigidbody2D>();
        health = maxHealth;
        sRender = gameObject.GetComponent<SpriteRenderer>();
		//Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(player){
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            if(direction.x > 0 & !isFacingRight){
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                isFacingRight = true;
            }
            if(direction.x < 0 & isFacingRight){
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                isFacingRight = false;
            }


            rb.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
        }
    }
	
	public int TipoZombie(){
		double u = new System.Random().NextDouble();
		if (u < 0.5)
			if (0.15 < u) return 2;

			else return 3;

		else return 1;
	}
	
	public int VidaZombie(){
		double u = new System.Random().NextDouble(); 	// U(0, 1)
		
		switch(this.type) {
			case 1:
				// Generate a System.Random integer in range 2 to 6
				return ( (int)(u*(6-2+1) + 2) );
				//break;
			case 2:
				// Generate a System.Random integer in range 7 to 12
				return ( (int)(u*(12-7+1) + 7) );
				//break;
			default:
				// Generate a System.Random integer in range 13 to 20
				return ( (int)(u*(20-13+1) + 13) );
				//break;
		} 
	}
	
	public float VelocidadeZombie(){
		System.Random r = new System.Random();
		double p = 1;
		double p2, p1 = 0;
		while ( p >= 1 ){
			p1 = r.NextDouble()*(1-(-1)) - 1; 	// U1(-1., 1.)
			p2 = r.NextDouble()*(1-(-1)) - 1; 	// U2(-1., 1.)
			p = p1 * p1 + p2 * p2;
		}
		float x = 0;
		switch(this.type) {
			case 1:
				// min >= 3 with a mean of 4
				x = Convert.ToSingle(4 + 0.6 * p1* Math.Sqrt((-2*Math.Log(p)/p)));	 // σ = 0.6
				if( x < 3) return 3; 	// X>=min
				else return x;
				//break;
			case 2:
				// min >= 4 with a mean of 5
				x = Convert.ToSingle(5 + 0.6 * p1* Math.Sqrt((-2*Math.Log(p)/p)));	 // σ = 0.6
				if( x < 4) return 4; 	// X>=min
				else return x;
				//break;
			default:
				// min >= 1 with a mean of 2
				x = Convert.ToSingle(2 + 0.6 * p1* Math.Sqrt((-2*Math.Log(p)/p)));	 // σ = 0.6
				if( x < 1) return 1; 	// X>=min
				else return x;
				//break;
		} 

	}
	
    public void TakeDamage(int a){
        health -= a;
        StartCoroutine(BlinkRed());
        if (health <= 0){
            SFCController.PlaySound("zombieDeath");
            Destroy(gameObject);
        }
    }

    IEnumerator BlinkRed(){
        sRender.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sRender.color = Color.white;
        

    }
    
}
