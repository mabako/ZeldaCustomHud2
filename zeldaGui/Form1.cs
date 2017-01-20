using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace zeldaGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        string currentIconset = @"IconsSets\Defaults";
        string currentBgr = @"None";
        Bitmap bgr;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.Text = "Right Click Here";
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
           
            addItems();
            setDefaultItems();
            
            loadLayout();
            loadIconsSet(currentIconset);
            if (currentBgr != "None")
            {
                bgr = new Bitmap(currentBgr);
                bgr.MakeTransparent(Color.Fuchsia);
            }
            drawIcons();
            if (checkUpdate)
            {
                if (Version.CheckUpdate() == true)
                {
                    var window = MessageBox.Show("There is a new version avaiable update?", "Update Avaible", MessageBoxButtons.YesNo);
                    if (window == DialogResult.Yes)
                    {
                        Help.ShowHelp(null, @"https://zarby89.github.io/ZeldaHud/");
                    }
                }
            }

        }
        int nbrIcons = 74;
        public static Bitmap[] iconSet;

        public void loadIconsSet(string data)
        {
            nbrIcons = Directory.GetFiles(data).Length;

            iconSet = new Bitmap[nbrIcons];
            for (int i = 0;i<nbrIcons;i++)
            {
                if (File.Exists(data + "\\" + i.ToString("D4") + ".png"))
                {
                    iconSet[i] = new Bitmap(data + "\\" + i.ToString("D4") + ".png");
                    iconSet[i].MakeTransparent(Color.Fuchsia);
                }
            }
            if (currentBgr != "None")
            {
                bgr = new Bitmap(currentBgr);
                bgr.MakeTransparent(Color.Fuchsia);
            }
            currentIconset = data;
        }

        public void drawIcons()
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.Clear(clearColor);

            ColorMatrix colorMatrix = new ColorMatrix(
            new float[][]
            {
                new float[] {.15f, .15f, .15f, 0, 0},
                new float[] {.16f, .16f, .16f, 0, 0},
                new float[] {.06f, .06f, .06f, 0, 0},
                new float[] {0, 0, 0,1f, 0},
                new float[] {0, 0, 0, 0f, 0f}
            });

            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(colorMatrix);

            for (int x = 0; x < widthIcons; x++)
            {
                for (int y = 0; y < heightIcons; y++)
                {
                    if (itemsArray[x, y] != null)
                    {
                        try
                        {
                            if (currentBgr != "None")
                            {
                                if (bgr != null)
                                {
                                    g.DrawImage(bgr, new Rectangle(x * 32, y * 32, 32, 32), 0, 0, 32, 32, GraphicsUnit.Pixel);
                                }
                            }
                            if (itemsArray[x, y].on == true)
                            {
                                g.DrawImage(iconSet[itemsArray[x, y].iconsId[itemsArray[x, y].level]], new Rectangle(x * 32, y * 32, 32, 32), 0, 0, 32, 32, GraphicsUnit.Pixel);
                            }
                            else
                            {
                                g.DrawImage(iconSet[itemsArray[x, y].iconsId[0]], new Rectangle(x * 32, y * 32, 32, 32), 0, 0, 32, 32, GraphicsUnit.Pixel, ia);
                            }
                        }
                        catch (Exception e)
                        {
                           
                        }
                    }
                }
            }
            pictureBox1.Refresh();
        }


        protected void OnTitlebarClick(Point pos)
        {
            contextMenuStrip1.Show(pos);
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xa4)
            {
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                OnTitlebarClick(pos);
                return;
            }
            base.WndProc(ref m);
        }

        Graphics g;
        public static List<CustomItem> itemsList = new List<CustomItem>();
        public static CustomItem[,] itemsArray = new CustomItem[24, 24];
        public static Color clearColor = Color.DarkSlateGray;

        public void addItems()
        {
            /* itemsList.Add(new CustomItem(new byte[] { 0, 48, 49, 49 }, "Bow"));//Bow 0
             itemsList.Add(new CustomItem(new byte[] { 1 }, "Blue Boomerang"));//Blue Boomerang 1
             itemsList.Add(new CustomItem(new byte[] { 2 }, "HookShot"));//Hookshot 2
             itemsList.Add(new CustomItem(new byte[] { 3 }, "Bombs"));//Bombs 3
             itemsList.Add(new CustomItem(new byte[] { 4, 56 }, "Mushroom"));//Mushroom 4
             itemsList.Add(new CustomItem(new byte[] { 5 }, "Fire Rod"));//Fire Rod 5
             itemsList.Add(new CustomItem(new byte[] { 6 }, "Ice Rod"));//Ice Rod 6
             itemsList.Add(new CustomItem(new byte[] { 7 }, "Bombos"));//Bombos 7
             itemsList.Add(new CustomItem(new byte[] { 8 }, "Ether"));//Ether 8 
             itemsList.Add(new CustomItem(new byte[] { 9 }, "Quake"));//Quake 9
             itemsList.Add(new CustomItem(new byte[] { 10 }, "Lamp"));//Lamp 10
             itemsList.Add(new CustomItem(new byte[] { 11 }, "Hammer"));//Hammer  11
             itemsList.Add(new CustomItem(new byte[] { 12, 57 }, "Shovel"));//Shovel 12
             itemsList.Add(new CustomItem(new byte[] { 13 }, "Net"));//Net 13 
             itemsList.Add(new CustomItem(new byte[] { 14 }, "Book"));//Book 14
             itemsList.Add(new CustomItem(new byte[] { 15 }, "Cane Somaria"));//Cane Somaria 15
             itemsList.Add(new CustomItem(new byte[] { 16 }, "Cane of Byrna"));//Cane Byrna 16
             itemsList.Add(new CustomItem(new byte[] { 17 }, "Cape"));//Cape 17
             itemsList.Add(new CustomItem(new byte[] { 18, 18 }, "Mirror"));//Mirror 18
             itemsList.Add(new CustomItem(new byte[] { 19, 43 }, "Power Glove"));//Power Glove 19
             itemsList.Add(new CustomItem(new byte[] { 20 }, "Boots"));//Boots 20
             itemsList.Add(new CustomItem(new byte[] { 21 }, "Flippers"));//Flippers 21
             itemsList.Add(new CustomItem(new byte[] { 22 }, "Moon Pearl"));//MoonPearl 22
             itemsList.Add(new CustomItem(new byte[] { 23, 38, 39, 40 }, "Sword"));//Sword 23
             itemsList.Add(new CustomItem(new byte[] { 24, 44, 45 }, "Shield"));//Shield 24
             itemsList.Add(new CustomItem(new byte[] { 25, 41, 42 }, "Tunic"));//Tunic //25
             itemsList.Add(new CustomItem(new byte[] { 26, 26, 50, 51, 52, 53, 54, 54 }, "Bottle 1",false,true));//Bottle1 26
             itemsList.Add(new CustomItem(new byte[] { 26, 26, 50, 51, 52, 53, 54, 54 }, "Bottle 2",false,true));//Bottle2 27
             itemsList.Add(new CustomItem(new byte[] { 26, 26, 50, 51, 52, 53, 54, 54 }, "Bottle 3",false,true));//Bottle3 28
             itemsList.Add(new CustomItem(new byte[] { 26, 26, 50, 51, 52, 53, 54, 54 }, "Bottle 4",false,true));//Bottle4 29
             itemsList.Add(new CustomItem(new byte[] { 27 }, "Eastern Pendant"));//Eastern (Green) //30
             itemsList.Add(new CustomItem(new byte[] { 28 }, "Desert Pendant"));//Desert (Blue) 31
             itemsList.Add(new CustomItem(new byte[] { 29 }, "Hera Pendant"));//Hera (Red) 32
             itemsList.Add(new CustomItem(new byte[] { 30 }, "Crystal 1"));//Crystal 1  33 pod
             itemsList.Add(new CustomItem(new byte[] { 31 }, "Crystal 2"));//Crystal 2  34 swamp
             itemsList.Add(new CustomItem(new byte[] { 32 }, "Crystal 3"));//Crystal 3 35 sw
             itemsList.Add(new CustomItem(new byte[] { 33 }, "Crystal 4"));//Crystal 4 36 tt
             itemsList.Add(new CustomItem(new byte[] { 34 }, "Crystal 5"));//Crystal 5 37 ice
             itemsList.Add(new CustomItem(new byte[] { 35 }, "Crystal 6"));//Crystal 6 38 mm
             itemsList.Add(new CustomItem(new byte[] { 36 }, "Crystal 7"));//Crystal 7 //39 trock
             itemsList.Add(new CustomItem(new byte[] { 37 }, "Red Boomerang"));//Red Boomerang 40
             itemsList.Add(new CustomItem(new byte[] { 46 }, "Powder"));//Powder 41
             itemsList.Add(new CustomItem(new byte[] { 47 }, "Flute"));//Flute 42
             itemsList.Add(new CustomItem(new byte[] { 55 }, "Agahnim"));//Agahnim //43
             itemsList.Add(new CustomItem(new byte[] { 58, 59 }, "Chest"));//Agahnim //44
             itemsList.Add(new CustomItem(new byte[] { 60 }, "Sanc Heart"));//Agahnim //45
             itemsList.Add(new CustomItem(new byte[] { 61 }, "Item Id 61"));//Agahnim //46
             itemsList.Add(new CustomItem(new byte[] { 62 }, "Item Id 62"));//Agahnim //47
             itemsList.Add(new CustomItem(new byte[] { 63 }, "Item Id 63"));//Agahnim //48
             itemsList.Add(new CustomItem(new byte[] { 64 }, "Item Id 64"));//Agahnim //49
             itemsList.Add(new CustomItem(new byte[] { 65 }, "Item Id 65"));//Agahnim //50
             itemsList.Add(new CustomItem(new byte[] { 66,67,68,69 }, "Heart Pieces",true));//Agahnim //51
             itemsList.Add(new CustomItem(new byte[] { 70, 71, 72, 73 }, "Bottle Counter"));//Agahnim //52
             itemsList.Add(new CustomItem(new byte[] { 26, 71, 72, 73 }, "Bottle Counter2"));//Agahnim //53*/

            //itemsList[51].on = true;

            string[] s = File.ReadAllLines("itemlist.txt");

            foreach(string line in s)
            {
                string name = "";
                byte[] order = new byte[0];
                bool loop = false;
                bool bottle = false;
                //Read character
                if (line.Length > 1)
                {
                    if (line[0] == '/' && line[1] == '/')//skip line
                    {

                    }
                    else
                    {
                        if (line[0] == ':')
                        {
                            for (int i = 1; i < line.Length; i++)//name
                            {
                                if (line[i] == ',')
                                {
                                    break;
                                }
                                name += line[i];
                            }
                            int sta = line.IndexOf('{')+1;
                            int end = line.IndexOf('}');
                            string ord = line.Substring(sta, end - sta);
                            string[] ords = ord.Split(',');
                            order = new byte[ords.Length];
                            for (int i = 0; i < ords.Length; i++)
                            {
                                order[i] = Convert.ToByte(ords[i]);
                            }
                            string last = line.Substring(end + 1);
                            string[] loopbottle = last.Split(',');
                            if (loopbottle[0] == "false")
                            {
                                loop = false;
                            }
                            if (loopbottle[0] == "true")
                            {
                                loop = true;
                            }
                            if (loopbottle[1] == "false")
                            {
                                bottle = false;
                            }
                            if (loopbottle[1] == "true")
                            {
                                bottle = true;
                            }
                            itemsList.Add(new CustomItem(order, name, loop, bottle));

                        }
                    }
                }
            }
        }


        public void setDefaultItems()
        {

            byte[] ditems = new byte[] {4 ,0 ,1 ,2 ,3 ,41,30,
                                        40,5 ,6 ,7 ,8 ,9 ,31,
                                        12,10,11,42,13,14,32,
                                        26,27,15,16,17,18,43,
                                        20,19,21,22,23,24,25,
                                        34,37,39,38,35,33,36};
            int i = 0;
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (itemsList.Count >= ditems[i])
                    {
                        itemsArray[x, y] = itemsList[ditems[i]];
                        i++;
                    }
                }
            }
        }


        byte widthIcons = 7;
        byte heightIcons = 6;
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

            //Resize the form to always be a multiple of 32
            int w = (this.Size.Width / 32) * 32;
            int h = (this.Size.Height / 32) * 32;
            if (w >= 24 * 32)
            {
                w = (24 * 32);
            }
            if (h >= 24 * 32)
            {
                h = (24 * 32);
            }
            if (h <= 64)
            {
                h = 64;
            }
            if (w <= 48)
            {
                w = 32;
            }
            this.Size = new Size((w) + 16, (h) + 7);
            widthIcons = (byte)(w / 32);
            heightIcons = (byte)((h / 32) - 1);
            drawIcons();

        }
        bool editMode = false;
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItem1.Checked)
            {
                editMode = true;
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                editMode = false;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
        }
        bool mDown = false;
        bool startDrag = true;
        CustomItem dragged = null;
        CustomItem swapped = null;
        int xdpos = -1;
        int ydpos = -1;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (editMode == true)
            {
                if (mDown == false)
                {
                    uiChanged = true;
                    dragged = itemsArray[(e.X / 32), (e.Y / 32)];
                    startDrag = true;
                    xdpos = (e.X / 32);
                    ydpos = (e.Y / 32);
                    Cursor = Cursors.Hand;
                    mDown = true;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (editMode == true)
            {
                if (mDown == true)
                {
                    if (startDrag == true)
                    {
                        if ((e.X / 32) < 24 && (e.X / 32) >= 0)
                        {
                            if ((e.Y / 32) < 24 && (e.Y / 32) >= 0)
                            {
                                uiChanged = true;
                                swapped = itemsArray[(e.X / 32), (e.Y / 32)];
                                itemsArray[(e.X / 32), (e.Y / 32)] = dragged;
                                itemsArray[xdpos, ydpos] = swapped;
                                swapped = null;
                                dragged = null;
                                startDrag = false;
                                xdpos = -1;
                                ydpos = -1;
                                Cursor = Cursors.Default;
                                mDown = false;
                            }
                        }
                    }
                }
            }
            drawIcons();
        }
        bool uiChanged = false;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int mX = (e.X / 32);
            int mY = (e.Y / 32);
            if (editMode == false)
            {
                if (itemsArray[mX, mY] != null)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (itemsArray[mX, mY].on)
                        {



                            if (itemsArray[mX, mY].level < itemsArray[mX, mY].iconsId.Length - 1)
                            {
                                //if (itemsArray)
                                if (itemsArray[mX, mY].bottle)
                                {
                                    if (itemsArray[mX,mY].level == 0)
                                    {
                                        itemsArray[mX, mY].level += 2;
                                    }
                                    else
                                    {
                                        if (itemsArray[mX, mY].level < 6)
                                        {
                                            itemsArray[mX, mY].level++;
                                        }
                                    }
                                }
                                else
                                {
                                    itemsArray[mX, mY].level++;
                                }
                                //drawIcons();
                            }
                            else
                            {
                                if (itemsArray[mX, mY].level == itemsArray[mX, mY].iconsId.Length - 1)
                                {
                                    if (itemsArray[mX, mY].loop == true)
                                    {
                                        itemsArray[mX, mY].level = 0;
                                    }
                                }
                            }

                        }
                        else
                        {
                            itemsArray[mX, mY].on = true;
                            //drawIcons();
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (itemsArray[mX, mY].on)
                        {
                            if (itemsArray[mX, mY].level > 0)
                            {
                                itemsArray[mX, mY].level--;
                                if (itemsArray[mX, mY].bottle)
                                {
                                    if (itemsArray[mX, mY].level == 1)
                                    {
                                        itemsArray[mX, mY].level--;
                                    }
                                }
                            }
                            else if (itemsArray[mX, mY].level == 0)
                            {
                                if (itemsArray[mX, mY].loop == false)
                                {
                                    itemsArray[mX, mY].on = false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    ItemSelectorForm itemForm = new ItemSelectorForm();
                    if (itemForm.ShowDialog() == DialogResult.OK)
                    {
                        if (itemForm.selectedItem == 255)
                        {
                            itemsArray[mX, mY] = null;
                        }
                        else
                        {
                            itemsArray[mX, mY] = itemsList[itemForm.selectedItem];
                        }
                    }
                }
            }
            drawIcons();
        }
       
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OptionsForm of = new OptionsForm();
            of.checkBox1.Checked = checkUpdate;
            of.iconset = currentIconset;
            of.bgr = currentBgr;
            of.ShowDialog();
            checkUpdate = of.checkBox1.Checked;
            currentBgr = of.bgr;
            loadIconsSet(of.iconset);
            drawIcons();
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TopMost == false)
            {
                TopMost = true;
                topMostToolStripMenuItem.Checked = true;
            }
            else
            {
                TopMost = false;
                topMostToolStripMenuItem.Checked = false;
            }
        }
        bool autoUpdate = false;
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                toolStripMenuItem3.Checked = false;
                timer1.Enabled = false;
            }
            else
            {
                toolStripMenuItem3.Checked = true;
                autoUpdate = true;
                timer1.Enabled = true;
            }
            
        }
        
        private void Form1_FormClosing(object sender, CancelEventArgs e)
        {
            if (uiChanged == true)
            {
                if (MessageBox.Show("Your Layout has changed do you want to save?", "Warning",
                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //e.Cancel = true;
                    save();
                }
            }
        }





        public void autoUpdateHud()
        {
            if (autoUpdate == true)
            {
                if (File.Exists(openFileDialog1.FileName))
                {
                    try
                    {
                        byte[] buffer = new byte[255];
                        FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                        fs.Position = 0x1E00;
                        fs.Read(buffer, 0, 255);
                        fs.Close();

                        Update(buffer);
                    }
                    catch (Exception e)
                    {
                        this.Text = e.Message.ToString();
                    }
                }
            }
        }

        public void Update(byte[] buffer)
        {
            // bow and arrows are set below
            // 2 - 14 hookshot to book of mudora
            for (int i = 2; i < 15; i++)
            {
                // Only the bombs have 'levels', i.e. how many bombs you actually have
                itemsList[i].on = buffer[i] != 0;
            }

            // 15 selected bottle, not used here

            // 16 - 23 somaria to moon pearl
            for (int i = 16; i < 24; i++)
            {
                itemsList[i - 1].on = buffer[i] != 0;
                itemsList[i - 1].level = (byte)(buffer[i] - 1);
            }

            // Isn't item 24 kinda... really really unused?
            // 25: Sword
            // 26: Shield
            // 27: Armor
            // 28, 29, 30, 31: "real" bottles, not just the selected one
            for (int i = 25; i < 32; i++)
            {
                if (i == 27)
                {
                    // Link is never nude
                    itemsList[i - 2].on = true;
                    itemsList[i - 2].level = buffer[i];
                }
                else
                {
                    itemsList[i - 2].on = buffer[i] != 0;
                    itemsList[i - 2].level = (byte)(buffer[i] - 1);
                }
            }

            // Heart Piece (0-3 of 4)
            // FIXME why is this 53?
            itemsList[53].on = true;
            itemsList[53].level = buffer[43];

            // Randomizer allows to keep some items despite not normally having access to them
            // This logic totally overwrites any stuff set before, i.e. whether you have the shovel/mushroom which disappear in normal gameplay
            byte randomizer = buffer[210], randomizer2 = buffer[212];
            itemsList[42].on = (randomizer & 3) > 0; // & 2 for fake flute, & 1 for working flute
            itemsList[12].on = (randomizer & 4) == 4; // Shovel
            itemsList[41].on = (randomizer & 0x10) == 0x10; // Magic Powder
            itemsList[4].on = (randomizer & 0x20) == 0x20; // Mushroom

            // Red OR Blue Boomerang
            itemsList[1].on = (randomizer & 0xC0) > 0;
            itemsList[1].level = (byte)((randomizer & 0x40) == 0x40 ? 1 : 0);

            // Red boomerang
            itemsList[40].on = (randomizer & 0x40) == 0x40;

            // Bow
            itemsList[0].on = (randomizer2 & 0xC0) > 0;
            itemsList[0].level = (byte)((randomizer2 & 0x40) == 0x40 ? 2 : 1);

            // Pendants
            itemsList[32].on = (buffer[52] & 1) == 1;
            itemsList[31].on = (buffer[52] & 2) == 2;
            itemsList[30].on = (buffer[52] & 4) == 4;

            // Did we murder Agahnim yet?
            itemsList[50].on = (buffer[133]) == 3;

            // Crystals
            itemsList[33].on = (buffer[58] & 1) == 1;
            itemsList[34].on = (buffer[58] & 2) == 2;
            itemsList[35].on = (buffer[58] & 4) == 4;
            itemsList[36].on = (buffer[58] & 8) == 8;
            itemsList[37].on = (buffer[58] & 16) == 16;
            itemsList[38].on = (buffer[58] & 32) == 32;
            itemsList[39].on = (buffer[58] & 64) == 64;

            drawIcons();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Autoupdate
            autoUpdateHud();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://zarby89.github.io/ZeldaHud/");
        }
        public bool checkUpdate = true;
        private void save()
        {
            string[] config = new string[10];
            config[0] = "width=" + widthIcons;
            config[1] = "height=" + heightIcons;
            config[2] = "items=" + stringitems();
            byte b = 0;
            if (TopMost)
            { b = 1; }
            else
            { b = 0; }
            config[3] = "topmost=" + b;
            config[4] = "winposx=" + this.Location.X;
            config[5] = "winposy=" + this.Location.Y;
            config[6] = "color=" + clearColor.ToArgb();
            config[7] = "iconset=" + currentIconset;
            if (checkUpdate)
            { b = 1; }
            else
            { b = 0; }
            config[8] = "checkupdate=" + b;
            config[9] = "background=" + currentBgr;

            File.WriteAllLines("layout.config", config);
        }
        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
            uiChanged = false;
        }

        public void loadLayout()
        {
            if (File.Exists("Layout.config"))
            {
                string[] s = File.ReadAllLines("Layout.config");
                string[] itl = s[2].Split('=')[1].Split(',');
                int w = Convert.ToInt32(s[0].Split('=')[1]);
                int h = Convert.ToInt32(s[1].Split('=')[1]);
                this.Location = new Point(Convert.ToInt32(s[4].Split('=')[1]), Convert.ToInt32(s[5].Split('=')[1]));
                int b = Convert.ToInt32(s[3].Split('=')[1]);
                if (b == 1)
                { TopMost = true;topMostToolStripMenuItem.Checked = true; }
                else
                { TopMost = false; topMostToolStripMenuItem.Checked = false; }
                b = Convert.ToInt32(s[8].Split('=')[1]);
                if (b == 1)
                { checkUpdate = true; }
                else
                { checkUpdate = false; }
                clearColor = Color.FromArgb(Convert.ToInt32(s[6].Split('=')[1]));
                currentIconset = s[7].Split('=')[1];
                if (s.Length >= 10)
                {
                    currentBgr = s[9].Split('=')[1];
                }



                int p = 0;
                for (int x = 0; x < 24; x++)
                {
                    for (int y = 0; y < 24; y++)
                    {
                        if (Convert.ToInt32(itl[p]) != -1)
                        {
                            itemsArray[x, y] = itemsList[Convert.ToInt32(itl[p])];
                        }
                        else
                        {
                            itemsArray[x, y] = null;
                        }
                        p++;
                    }
                }
                this.Size = new Size((w * 32) + 16, ((h + 1) * 32) + 7);
                widthIcons = (byte)(w);
                heightIcons = (byte)((h));
                drawIcons();
            }
        }


        private string stringitems()
        {
            string s = "";
            int p = 0;
            for (int x = 0; x < 24; x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    if (itemsArray[x,y] != null)
                    {
                        s+=itemsList.FindIndex(i => i.Equals(itemsArray[x,y])).ToString();
                        s += ",";
                    }
                    else
                    {
                        s += "-1,";
                    }
                    p++;
                }
            }
            return s;
        }

        private void importOldLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {
                string[] s = File.ReadAllLines(openFileDialog2.FileName);

                int w = Convert.ToInt32(s[0].Split('=')[1]);
                int h = Convert.ToInt32(s[1].Split('=')[1]);
                string[] itl = s[3].Split('=')[1].Split(',');
                int p = 0;
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        if (Convert.ToInt32(itl[p]) != 254)
                        {
                            itemsArray[x, y] = itemsList[Convert.ToInt32(itl[p])];
                        }
                        else
                        {
                            itemsArray[x, y] = null;
                        }
                        p++;
                    }
                }
                        this.Size = new Size((w*32) + 16, ((h+1)*32) + 7);
                widthIcons = (byte)(w);
                heightIcons = (byte)((h) - 1);
                drawIcons();
            }
        }

        private void clearItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int x = 0;x<24;x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    if (itemsArray[x, y] != null)
                    {
                        itemsArray[x, y].on = false;
                        itemsArray[x, y].level = 0;
                    }
                }
            }
            drawIcons();
        }
    }
}
