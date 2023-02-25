using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Reusable
{
    public class Axe : PermanentItem
    {

        public override void UseItem() {

        }

        public override string GetItemName() {
            return "Axe";
        }

        public override string GetItemDescription()
        {
            return "The axe can help you chop down small trees and split up logs. Somehow Pitbull" +
                   " and Kesha come to mind...";
                   //Remember to yell, 'Timber'! - Otherwise, Pitbull and Ke$ha will be mad...";
        }
    }
}

