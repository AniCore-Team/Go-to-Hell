using UnityEngine;
using Zenject;


public class FactoriesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        /*Container.BindFactory<GameObject, UIInventory, Factory<UIInventory>>().FromFactory<ZenFactory<UIInventory>>();
        Container.BindFactory<GameObject, ItemObject, Factory<ItemObject>>().FromFactory<ZenFactory<ItemObject>>();
        Container.BindFactory<GameObject, DropItemSlot, Factory<DropItemSlot>>().FromFactory<ZenFactory<DropItemSlot>>();
        Container.BindFactory<GameObject, InventorySlot, Factory<InventorySlot>>().FromFactory<ZenFactory<InventorySlot>>();*/

    }
}


