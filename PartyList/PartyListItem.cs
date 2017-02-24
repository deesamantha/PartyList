using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyList
{
    class PartyListItem
    {
        // give the item a name
        string name;
        // how many do you have
        int quantity;

        //constructor
        public PartyListItem(string n, int q)
        {
            //set beginning quantity and name
            this.Quantity = q;
            this.Name = n;
        }

        //Create get and set for name and quantity
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        // return list item to string
        public override string ToString()
        {
            return quantity + " " + name;
        }
        
    }
}
