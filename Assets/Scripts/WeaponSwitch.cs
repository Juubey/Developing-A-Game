using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    public GameObject weaponsHolder;
    private UnityEngine.KeyCode[] Keycodes = {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, 
    KeyCode.Alpha9, KeyCode.Alpha0}; 
    /*
    int totalWeapons = 1;
    public int selectedWeapon = 0;
    public int currentWeaponIndex;
    public GameObject[] weapons;
    public GameObject weaponsHolder;
    public GameObject currentWeapon;

    public GameObject tome;
    public GameObject spear;

    */


    // Start is called before the first frame update
    void Start()
    {
        /*
        totalWeapons = weaponsHolder.transform.childCount;
        weapons = new GameObject[totalWeapons];

        for (int i = 0; i < totalWeapons; i++){
            weapons[i] = weaponsHolder.transform.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
        currentWeapon = weapons[0];
        currentWeaponIndex = 0;
        */

        selectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f){
            if (selectedWeapon >= transform.childCount -1)
                selectedWeapon = 0;
            else
            selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f){
            if (selectedWeapon <= 0 )
                selectedWeapon = transform.childCount - 1;
            else
            selectedWeapon--;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }

        if(previousSelectedWeapon != selectedWeapon)
        {
            selectWeapon();
        }
        /*
        Vector2 vec = Mouse.current.scroll.ReadValue();
        float y = vec.y;

        //Next Weapon
        if(y < 0)
        {
            if(currentWeaponIndex < totalWeapons - 1)
            {
                weapons[currentWeaponIndex].SetActive(false);

                currentWeaponIndex +=1;
                weapons[currentWeaponIndex].SetActive(true);
                //tomeIns();
            }
        }

        currentWeapon = weapons[currentWeaponIndex];

        //Previous Weapon
        if(y > 0)
        {
            if(currentWeaponIndex < totalWeapons - 1)
            {
                weapons[currentWeaponIndex].SetActive(false);
                
                currentWeaponIndex += 1;

                weapons[currentWeaponIndex].SetActive(true);
                //spearIns();
            }
        }
        */
    }

    void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if(i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
            Instantiate(weapon.gameObject, weaponsHolder.transform);
        }
    }

    private void spearIns(){
        GameObject spear = ObjectPool.instance.GetPooledObject();
        if (spear != null)
        {
            //spear.transform.position = spear.transform.position;
            spear.SetActive(true);
        }
    }


    private void tomeIns(){
        GameObject tome = ObjectPool.instance.GetPooledObject();
        if (tome != null)
        {
            //tome.transform.position = tome.transform.position;
            tome.SetActive(true);
        }
    }
}
