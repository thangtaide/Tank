using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Reflection;

public interface ItemEffect
{
    public object Info { set; }
    public void Active();
}


public class ItemController : MonoBehaviour
{
    public string ItemName;

    SubEffectInfo[] itemEffectInfos;

    void Start()
    {
        JSONNode json = JSON.Parse(Resources.Load<TextAsset>("Data/Items/" + ItemName).text);
        JSONArray array = json["data"].AsArray;

        itemEffectInfos = Utils.GetSubEffectInfos(array);
    }

    private void Update()
    {
        if (Player.Instance != null)
        {
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) <= 5f)
            {
                foreach (SubEffectInfo subEffectInfo in itemEffectInfos)
                {
                    ItemEffect subEffect = (ItemEffect)Player.Instance.gameObject.AddComponent(subEffectInfo.type);
                    subEffect.Info = subEffectInfo.data;
                    subEffect.Active();
                }
                Destroy(this.gameObject);
            }
        }
    }
}
