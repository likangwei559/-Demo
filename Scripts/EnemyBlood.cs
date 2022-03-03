using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlood : MonoBehaviour
{	
	public Texture2D blood_red;
	public Texture2D blood_black;

	private Camera camera;
	private string name;
	private StateManager sm;
	
	private Vector3 playerPos;
	private Vector3 playerFor;
	private Vector3 enemyPos;
	private int blood_width;
	float enemyHeight;
	
	public float HP = 20;
	public float HPMax = 20;

	void Start()
	{
		
		camera = Camera.main;
		name = transform.name;
		sm = GetComponent<StateManager>();

		float size_y = GetComponent<Collider>().bounds.size.y;		
		float scal_y = transform.localScale.y;
		enemyHeight = (size_y * scal_y);
							
	}

	void Update()
	{
		
			playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
			playerFor = GameObject.FindGameObjectWithTag("Player").transform.forward;
			//print(playerFor);
			enemyPos = transform.position;
			
	}

	void OnGUI()
	{
		Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + enemyHeight, transform.position.z);
		Vector2 position = camera.WorldToScreenPoint(worldPosition);
		position = new Vector2(position.x, Screen.height - position.y);

		//如果在玩家的视野范围内
		if ((Vector3.Angle((enemyPos - playerPos), playerFor) < 60) && (Vector3.Distance(enemyPos,playerPos) < 10))
		{
			Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red));
			if (transform.tag == "Monster")//怪物类的血量不通过sm扣除
			{
				 blood_width = blood_red.width * (int)HP / (int)HPMax;
			}
			else
			{				
				 blood_width = blood_red.width * (int)sm.HP / (int)sm.HPMax;
			}			
			GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, bloodSize.x, bloodSize.y), blood_black);
			GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - bloodSize.y, blood_width, bloodSize.y), blood_red);


			Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(name));
			GUI.color = Color.yellow;
			GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y - bloodSize.y, nameSize.x, nameSize.y), name);
		}
		

	}

}
