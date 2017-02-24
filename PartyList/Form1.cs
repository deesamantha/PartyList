using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PartyList
{
    public partial class Form1 : Form
    {
        //Create lists for needs and haves
        List<PartyListItem> needs = new List<PartyListItem>();
        List<PartyListItem> haves = new List<PartyListItem>();


        public Form1()
        {
            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            //empty label error
            lblErr.Text = "";
            
            // is qty filled out? set qty else qty = 0
            int quantity = textBox1.Text != "qty." ? Convert.ToInt32(textBox1.Text) : 0;
            // is string blank?
            string item = textBox2.Text != "Items" ? textBox2.Text : "";
            
            //error checking
            if (quantity == 0) { lblErr.Text += "\r\nChoose a Quantity!"; }
            if (item == "") { lblErr.Text += "\r\nAdd an item!"; }

            //add new item to the haves
            haves.Add(new PartyListItem(item, quantity));

            //reload the lists
            Reload();

        }

        private void btnToRight_Click(object sender, EventArgs e)
        {
            //is there an item selected in lb1
            if (listBox1.SelectedItem != null)
            {
                //what is the position
                int pos = listBox1.SelectedIndex;
                // add the item to needs
                needs.Add(haves[pos]);
                //remove from haves
                haves.RemoveAt(pos);
                //reload
                Reload();
            }
        }

        private void btnToLeft_Click(object sender, EventArgs e)
        {
            //item selected on list 2?
            if (listBox2.SelectedItem != null)
            {
                //get position
                int pos = listBox2.SelectedIndex;
                //add item to haves
                haves.Add(needs[pos]);
                //remove from needs
                needs.RemoveAt(pos);
                //reload
                Reload();
            }
        }

        public void Reload()
        {
            //empty all fields and redraw
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox1.DataSource = haves;
            listBox2.DataSource = needs;
            listBox1.SelectedItem = null;
            listBox2.SelectedItem = null;
            Invalidate();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //items selected on either list? if so remove them
            if (listBox1.SelectedItem != null)
            {
                haves.RemoveAt(listBox1.SelectedIndex);
            }
            if (listBox2.SelectedItem != null)
            {
                needs.RemoveAt(listBox2.SelectedIndex);
            }
            //reload
            Reload();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create parent party element
            XElement party = new XElement("Party");
            //create haves holder
            XElement haveXML = new XElement("Haves");
            //create needs holder
            XElement needXML = new XElement("Needs");
            // add each have on list to as "have" element
            foreach (PartyListItem item in haves)
            {
                XElement have = new XElement("Have");
                have.Add(new XElement("Name", item.Name));
                have.Add(new XElement("Quantity", item.Quantity));

                haveXML.Add(have);
            }
            //add each need on list as need element
            foreach (PartyListItem item in needs)
            {
                XElement need = new XElement("Need");
                need.Add(new XElement("Name", item.Name));
                need.Add(new XElement("Quantity", item.Quantity));

                needXML.Add(need);
            }


            //create document
            XDocument document = new XDocument();
            //add parent and subs
            party.Add(haveXML);
            party.Add(needXML);
            document.Add(party);

            //create filesave dialog
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            //filter save optionn
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

            //if named
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //save
                document.Save(saveFileDialog1.FileName, SaveOptions.DisableFormatting); //create items.xml file in bin folder
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open load box
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //only show xml and all
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //create xml document from loaded
                XDocument load = XDocument.Load(openFileDialog1.OpenFile());

                //add each have to have ist
                foreach (XElement h in load.Descendants("Have"))
                {
                    haves.Add(new PartyListItem(h.Element("Name").Value, Convert.ToInt32(h.Element("Quantity").Value)));
                }

                //add each need to need list
                foreach (XElement n in load.Descendants("Need"))
                {
                    needs.Add(new PartyListItem(n.Element("Name").Value, Convert.ToInt32(n.Element("Quantity").Value)));
                }
                //reload
                Reload();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //empty err label
            lblErr.Text = "";
            // quantity
            int quantity = textBox4.Text != "qty." ? Convert.ToInt32(textBox4.Text) : 0;
            //item name
            string item = textBox3.Text != "Items" ? textBox3.Text : "";

            //error check
            if (quantity == 0) { lblErr.Text += "\r\nChoose a Quantity!"; }
            if (item == "") { lblErr.Text += "\r\nAdd an item!"; }
            
            //add to needs
            needs.Add(new PartyListItem(item, quantity));

            //reload
            Reload();

        }
    }
}
