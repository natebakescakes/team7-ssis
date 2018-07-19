using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.ViewModels
{
    public class OrderItem
    {
        private Item item;
        private int quantity;

        public Item Item
        {
            get { return item; }
            set { item = value; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        } 
    }
}