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
    public GameObject shopParentObject;
    public GameObject shopContent;
    public GameObject currentEquipment;
    void Awake()
    {
        allEquipmentsSO = Resources.LoadAll<EquipmentSO>("Prefabs/ScriptableObjects/Equipment").ToList();
        shopParentObject = GameObject.Find("Shop");
        shopContent = GameObject.Find("ShopContent");
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        if (shopParentObject == null || shopContent == null)
        {
            shopParentObject = playerManager.shopParentObject;
            shopContent = playerManager.shopContent;
        }

        allEquipmentsSO.Sort((x, y) => x.name.CompareTo(y.name));
        if (shopContent.transform.childCount != 0)
        {
            GetAllShopGameObjects();
        }
        else
        {
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
                        if (playerManager.unlockedHeads.Contains(equipment.name))
                        {
                            currentEquipment.transform.Find("CookiePrice").gameObject.SetActive(false);
                        }

                        if (equipment.name == playerManager.currentHeadName)
                        {
                            playerManager.headSpriteRenderer.sprite = equipment.equipmentSprite;
                        }
                        break;
                    case EquipmentType.Body:
                        currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyBody(equipment); });
                        if (playerManager.unlockedBodies.Contains(equipment.name))
                        {
                            currentEquipment.transform.Find("CookiePrice").gameObject.SetActive(false);
                        }
                        if (equipment.name == playerManager.currentBodyName)
                        {
                            playerManager.bodySpriteRenderer.sprite = equipment.equipmentSprite;
                        }
                        break;
                    case EquipmentType.Weapon:
                        currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyWeapon(equipment); });
                        if (playerManager.unlockedWeapons.Contains(equipment.name))
                        {
                            currentEquipment.transform.Find("CookiePrice").gameObject.SetActive(false);
                        }
                        if (equipment.name == playerManager.currentWeaponName)
                        {
                            playerManager.weaponSpriteRenderer.sprite = equipment.equipmentSprite;
                        }
                        break;
                    case EquipmentType.Arms:
                        currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyArms(equipment); });
                        if (playerManager.unlockedArms.Contains(equipment.name))
                        {
                            currentEquipment.transform.Find("CookiePrice").gameObject.SetActive(false);
                        }
                        if (equipment.name == playerManager.currentArmsName)
                        {
                            playerManager.armsSpriteRenderer[0].sprite = equipment.equipmentSprite;
                            playerManager.armsSpriteRenderer[1].sprite = equipment.equipmentSpriteRightArmOrLeg;
                        }
                        break;
                    case EquipmentType.Legs:
                        currentEquipment.GetComponent<Button>().onClick.AddListener(delegate { playerManager.buyLegs(equipment); });
                        if (playerManager.unlockedLegs.Contains(equipment.name))
                        {
                            currentEquipment.transform.Find("CookiePrice").gameObject.SetActive(false);
                        }
                        if (equipment.name == playerManager.currentLegsName)
                        {
                            playerManager.legsSpriteRenderer[0].sprite = equipment.equipmentSprite;
                            playerManager.legsSpriteRenderer[1].sprite = equipment.equipmentSpriteRightArmOrLeg;
                        }
                        break;
                    default:
                        break;
                }
                allEquipmentsGO.Add(currentEquipment);
                currentEquipment.SetActive(false);
            }
        }
    }
    void Start()
    {
        shopParentObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag != "Player" || !Input.GetKeyDown(openShopKey)) return;

        playerManager.gameObject.GetComponent<PlayerMovement>().enabled = false;
        shopParentObject.SetActive(true);
        foreach (EquipmentSO equipment in equipmentsOnSale)
        {
            allEquipmentsGO.Find(go => go.name == equipment.name).SetActive(true);
        }
    }


    public void GetAllShopGameObjects()
    {

        foreach (Transform equipment in shopContent.transform)
        {
            allEquipmentsGO.Add(equipment.gameObject);
        }
    }
}
