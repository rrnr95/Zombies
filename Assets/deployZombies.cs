using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class deployZombies : MonoBehaviour {

	public GameObject zombiePrefab;
	public float respawnTime;

	
	// coordenadas de um ponto no plano: (x, y)
	public class CartesianCoord{
		public double x;
		public double y; 
		
		public CartesianCoord(double x, double y){
			this.x = x;
			this.y = y;
		}
		
		public float getX(){
			return Convert.ToSingle(this.x);
		}
	
		public float getY(){
			return Convert.ToSingle(this.y);
		}
	}

    // Start is called before the first frame update
    public void Start() {
		StartCoroutine(zombieWave());
	}

	private float zombiesUniTempo(){
		System.Random r = new System.Random();
		double a = Math.Exp(-0.80); 	// λ = 0.85
		double b = 1;
		int i;
		for( i=0; b >= a; i++ ){
			double u = r.NextDouble(); 	// U(0, 1)
			b *= u;
		}
		if( i > 6 ) return Convert.ToSingle(5); 	// Xmax = 5
		else{
			if( i <= 1) return Convert.ToSingle(1);
			
			else return Convert.ToSingle((i-1));
		}
	}
	
	private CartesianCoord posSpawnZombie(double xMin, double xMax, double yMin, double yMax){
		System.Random r = new System.Random();
		double x0 = 0.5 * ( xMin + xMax );
		double y0 = 0.5 * ( yMin + yMax );
		double a = 0.5 * ( xMax - xMin );
		double b = 0.5 * ( yMax - yMin );
		double x = 1;
		double y = 1;
		while( x * x + y * y > 1 ){
			x = r.NextDouble()*(1-(-1)) - 1; 	// Ux(-1., 1.)
			y = r.NextDouble()*(1-(-1)) - 1; 	// Uy(-1., 1.)
		}
		CartesianCoord p = new CartesianCoord(x0 + a * x, y0 + b * y);
		return p;
	}
	
	
	
	private void spawnZombie(){
		GameObject z = Instantiate(zombiePrefab) as GameObject;
		CartesianCoord pos = posSpawnZombie(0.05f, 0.95f, 0.05f, 0.95f);
		Vector3 vectorPos = new Vector3(pos.getX(), pos.getY(), 10.0f);
		z.transform.position = Camera.main.ViewportToWorldPoint(vectorPos);
	}
	
	IEnumerator zombieWave(){
		while(true){
			respawnTime = zombiesUniTempo();
			yield return new WaitForSeconds(respawnTime);
			spawnZombie();
		}
	}
	
    
}
