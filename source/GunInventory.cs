using UnityEngine;
using System.Collections;

/**
 * Menage changing to another owned weapons.
 */

public class GunInventory : MonoBehaviour {
	public GameObject[] gunsList = new GameObject[4];
	public bool[] haveGuns = new bool[] {false, true, false, false, false, false, false, false, false, false}; 
	private KeyCode[] keys = new KeyCode[] {KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2,
                                            KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
                                            KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8,
                                            KeyCode.Alpha9};
	private int maxGuns = 1;
	private int currentGun = 1;
	//private bool[] haveSomeAmmnition = new bool[] {false, true, false, false};

	void Update(){
		for (int i = 1; i < gunsList.Length; i++) {
			if (Input.GetKeyDown (keys [i]) && haveGuns[i]) {
                hideGuns();
                setGun(i);
			}
		}
	}

	public void addWeapon(int number)
	{
        Debug.Log(number);
        Debug.Log(haveGuns[number]);
        if (haveGuns [number] == false) {
			haveGuns [number] = true;
			//haveSomeAmmnition [number] = true;
			maxGuns++;

			hideGuns ();  
			gunsList [number].SetActive (true);
		}
	}

	public void setGun(int number){
			hideGuns ();
			gunsList[number].SetActive (true);
			currentGun = number;
		}

	private void hideGuns(){

		for(int i = 1; i< gunsList.Length; i++) { //=
			gunsList[i].SetActive(false);
		}
	}

	void addAmmo(Vector2 ammoData){ //x = ammunition, y = gunType
        int gunNumber = (int)ammoData.y;
        if (gunNumber != currentGun)
            gunsList[gunNumber].SetActive(false);

        //if (gunsList[gunNumber].GetComponent<Shoot>().canGetAmmo() ) {
            gunsList[gunNumber].SetActive(true);
            gunsList[gunNumber].SendMessage("addAmmo", ammoData);
		//}
		setGun(currentGun);
	}

}
