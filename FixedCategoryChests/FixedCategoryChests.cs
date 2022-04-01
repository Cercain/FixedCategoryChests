using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using RoR2;
using RoR2.Artifacts;
using UnityEngine;
using UnityEngine.Networking;
using PickupIndex = RoR2.PickupIndex;

namespace FixedCategoryChests
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class FixedCategoryChests : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "Cercain";
        public const string PluginName = "FixedCategoryChests";
        public const string PluginVersion = "1.0.0";

        public void OnEnable()
        {
            On.RoR2.BasicPickupDropTable.PassesFilter += PassesFilter;
        }

        public void OnDisable()
        {
            On.RoR2.BasicPickupDropTable.PassesFilter -= PassesFilter;
        }

        bool PassesFilter(On.RoR2.BasicPickupDropTable.orig_PassesFilter orig, RoR2.BasicPickupDropTable self, PickupIndex pickupIndex)
        {
            PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);

            if (pickupDef.itemIndex != ItemIndex.None)
            {
                ItemTag[] tags = ItemCatalog.GetItemDef(pickupDef.itemIndex).tags;
                foreach (var bannedTag in self.bannedItemTags)
                    if (Array.IndexOf(tags, bannedTag) != -1)
                        return false;

                foreach (var requiredTag in self.requiredItemTags)
                    if (Array.IndexOf(tags, requiredTag) == -1)
                        return false;
            }

            return true;
        }

    }
}
