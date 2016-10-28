using UnityEngine;
using System.Collections;

public class WebCamMain : MonoBehaviour {
	
	
	GUITexture ShowContentTexture;
	
	WebCamDevice[]  devices ;
	WebCamTexture frontTex;
		
	string frontCamName;
	string backCamName;
	
	const int Cam_width = 300;
	const int  Cam_height = 200;	
	int  framesPerSec=30;	
	
	
	bool bOnOffDisplay;
	public bool bFreezeCube = false;

	#region  Image Pixel Processing  manually
    //private Color[] pixels;
	
	public GameObject PixelCubePrefab;
	public Transform root_PixelCube;
	
	#endregion


	public GameObject[][] Array_PixelCube;
	
	
	// Use this for initialization
	void Start () {
		
		devices  =WebCamTexture.devices;

		bOnOffDisplay = true;
		
		if( devices!= null &&   devices.Length >0 )
		{
			frontTex = new WebCamTexture(devices[0].name, Cam_width, Cam_height, framesPerSec);


			ShowContentTexture = GameObject.Find("WebCamDisplayTexture").GetComponent<GUITexture>();
			ShowContentTexture.texture  = frontTex;
			ShowContentTexture.pixelInset =new  Rect (   (float) (Cam_width/2.0*(-1)), (float)( Cam_height/2.0*(-1)), (float)Cam_width, (float)Cam_height); 
			frontTex.Play();			
		}
		else
		{
			Debug.LogError("faill");
		}
		
//		pixels = frontTex.GetPixels(0,0,Cam_width,Cam_height);
//		Debug.Log("pixels.Length="+ pixels.Length);		
		
		
		Array_PixelCube = new GameObject[Cam_width/10][];
		
		// init the blocks		
		for(int i =0 ; i < Cam_width/10 ; i++)
		{

			Array_PixelCube[i] = new GameObject[ Cam_width/10 ];

			for(int j =0 ; j < Cam_height/10 ; j++)
			{
				PixelCubePrefab.AddComponent<Rigidbody>();
				PixelCubePrefab.GetComponent<Rigidbody>().useGravity = false;

				Array_PixelCube[i][j] = (GameObject)Instantiate( PixelCubePrefab, new Vector3(  i*0.5f, j*0.5f,0f),Quaternion.identity); 
				
				((GameObject)Array_PixelCube[i][j]).name = "Pixel_"+i+"_"+j;
				((GameObject)Array_PixelCube[i][j]).transform.parent = root_PixelCube;
			}
		}
		
			
				root_PixelCube.localScale = new Vector3(2,2,2); 
				root_PixelCube.position = new Vector3(-15,-7);
	}
	
	// Update is called once per frame
	void Update () {
		
		int nCount =0;


		for(int i =0 ; i <Cam_width  ; i++)	
		{

			
			for(int j =0 ; j < Cam_height; j++)
			{
				if( j%10 ==0 && i%10 ==0 ) // sampling 
				{
					Transform obj =  Array_PixelCube[i/10][j/10].transform ;

					if(obj)
					{
							obj.gameObject.GetComponent<Renderer>().material.color=   frontTex.GetPixel( i*4 ,j*3 )  ;
							
						if(bFreezeCube)
							obj.position = new Vector3(obj.position.x,obj.position.y,    obj.gameObject.GetComponent<Renderer>().material.color.grayscale*2 );
					}
				}
			}
		}		
		
	}
	
	
	void OnGUI()
	{
		if( GUI.Button( new Rect(50,0,150,50), "on/close"))
		{
			bOnOffDisplay = !bOnOffDisplay;
			ShowContentTexture.enabled = bOnOffDisplay;
		}


		if( GUI.Button( new Rect(50,100,150,50), "Feeeze the Cube"))
		{
			bFreezeCube = !bFreezeCube;
		}


		if( GUI.Button( new Rect(50,200,150,50), "Open/Close Phys"))
		{
			for(int i =0 ; i < Cam_width/10 ; i++)
			{
				for(int j =0 ; j < Cam_height/10 ; j++)
				{
					bool bSwithGravity = ((GameObject)Array_PixelCube[i][j]).GetComponent<Rigidbody>().useGravity;
					((GameObject)Array_PixelCube[i][j]).GetComponent<Rigidbody>().useGravity = !bSwithGravity;
				}
			}

		}

		
		
	}
	
	
	
}
