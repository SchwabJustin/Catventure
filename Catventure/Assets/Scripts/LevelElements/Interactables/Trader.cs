using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Trader : MonoBehaviour
{
    public KeyCode openShopKey = KeyCode.F;
    public GameObject shopItemPrefab;
    public List<EquipmentSO> allEquipmentsSO;
    public List<GameObject> allEquipmentsGO;
    public List<EquipmentSO> equipmentsOnSale;
    private PlayerManager playerManager;
    private GameObject shopParentObject;
    public GameObject currentEquipment;
    void Start()
    {
        allEquipmentsSO = Resources.LoadAll<EquipmentSO>("Prefabs/ScriptableObjects/Equipment").ToList();
        GameObject shopContent = GameObject.Find("ShopContent");
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        allEquipmentsSO.Sort((x, y) => x.name.CompareTo(y.name));
        if (shopContent.transform.childCount != 0) return;
        foreach (EquipmentSO equipment in allEquipmentsSO)
        {
            currentEquipment = Instantiate(shopItemPrefab, shopContent.transform).gameObject;
            currentEquipment.name = equipment.name;
            currentEquipment.transform.Find("CookiePrice").GetComponent<TMP_Text>().text = equipment.cookieCost + " Cookies";
            currentEquipment.transform.Find("Image").GetComponent<Image>().sprite = equipment.equipmentSprite;
            switch (equipment.equipmentType)
            {
                case EquipmentType.Head:
                    currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyHead(equipment); });
                    break;
                case EquipmentType.Body:
                    currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyBody(equipment); });
                    break;
                case EquipmentType.Weapon:
                    currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyWeapon(equipment); });
                    break;
                case EquipmentType.Arms:
                    currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyArms(equipment); });
                    break;
                case EquipmentType.Legs:
                    currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyLegs(equipment); });
                    break;
                default:
                    break;
            }
            allEquipmentsGO.Add(currentEquipment);
            currentEquipment.SetActive(false);
        }
        shopParentObject = GameObject.Find("Shop");
        shopParentObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag != "Player" || !Input.GetKeyDown(openShopKey)) return;
        playerManager.gameObject.GetComponent<PlayerMovement>().enabled = false;
        Debug.Log("OpenShop");
        shopParentObject.SetActive(true);
        foreach (EquipmentSO equipment in equipmentsOnSale)
        {
            allEquipmentsGO.Find(go => go.name == equipment.name).SetActive(true);
        }
    }
}
