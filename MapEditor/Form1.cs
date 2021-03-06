﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MapEditor
{
    public partial class Form1 : Form
    {
        // attributes
        private BinaryReader bR;
        private BinaryWriter bW;
        private Stream bRStream;
        private Stream bWStream;
        private int type;
        private List<PictureBox> images;
        private List<int> types;
        private int x, y, width;
        bool changed;

        // Mouse States
        bool mouseDown = false;

        public Form1()
        {
            InitializeComponent();

            // initializing attributes
            images = new List<PictureBox>();
            types = new List<int>();
            x = 16;
            textBox1.Text = x.ToString();
            y = 201;
            textBox3.Text = y.ToString();
            width = 800;
            textBox2.Text = width.ToString();
            changed = false;

            pictureBox3.Parent = pictureBox1;
            pictureBox3.Location = new Point(x - 2, y - pictureBox3.Image.Height + 2);

            FormClosing += new FormClosingEventHandler(closeButton_Click);

            // allowing preview to scroll vertically and horizontally
            panel1.AutoScroll = true;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PictureBox newImage = new PictureBox();
            bool twoPlayers = false;
            switch (type)
            {
                case 0:
                {
                    newImage.Image = buildingOne.Image;
                    newImage.ClientSize = buildingOne.Image.Size;
                    break;
                }
                case 1:
                {
                    newImage.Image = buildingTwo.Image;
                    newImage.ClientSize = buildingTwo.Image.Size;
                    break;
                }
                case 2:
                {
                    twoPlayers = types.Contains(2);
                    if (!twoPlayers)
                    {
                        newImage.Image = player.Image;
                        newImage.ClientSize = player.Image.Size;
                    }
                    break;
                }
                case 3:
                {
                    newImage.Image = ZombieOne.Image;
                    newImage.ClientSize = ZombieOne.Image.Size;
                    break;
                }
                case 4:
                {
                    newImage.Image = zombieTwo.Image;
                    newImage.ClientSize = zombieTwo.Image.Size;
                    break;
                }
                case 5:
                {
                    newImage.Image = buildingThree.Image;
                    newImage.ClientSize = buildingThree.Image.Size;
                    break;
                }
                case 6:
                {
                    newImage.Image = buildingFour.Image;
                    newImage.ClientSize = buildingFour.Image.Size;
                    break;
                }
                case 7:
                {
                    newImage.Image = buildingFive.Image;
                    newImage.ClientSize = buildingFive.Image.Size;
                    break;
                }
                case 8:
                {
                    newImage.Image = buildingSix.Image;
                    newImage.ClientSize = buildingSix.Image.Size;
                    break;
                }
                case 9:
                {
                    newImage.Image = bossZombie.Image;
                    newImage.ClientSize = bossZombie.Image.Size;
                    break;
                }
            }

            // adding the current image to the list of images
            int i = 0;
            if (!twoPlayers)
            {
                images.Add(newImage);
                i = images.Count - 1;
                types.Add(type);
            }
            else
            {
                i = types.IndexOf(2);
            }

            images[i].Name = i.ToString();
            if (!twoPlayers)
            {
                images[i].MouseDown += new MouseEventHandler(images_MouseDown);
                images[i].MouseUp += new MouseEventHandler(images_MouseUp);
                images[i].MouseMove += new MouseEventHandler(images_MouseMove);
                images[i].KeyDown += new KeyEventHandler(images_KeyDown);
            }
            images[i].BackColor = Color.Transparent;
            images[i].Parent = pictureBox1;
            images[i].Location = new Point(x, (y - images[i].Height));
            if(type < 2 || type > 4 && type != 9)
            {
                images[i].SendToBack();
            }
            else
            {
                images[i].BringToFront();
            }
            changed = true;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        // when user clicks "Save" in File section of toolbar
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {        
            // opening save dialog
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                if(saveFileDialog1.FileName == null)
                {
                    Close();
                }

                bWStream = File.OpenWrite(saveFileDialog1.FileName); // now saving file with user's input file name
                bWStream.SetLength(0);
                bW = new BinaryWriter(bWStream);
                bW.Write(width);
                for(int i = 0; i < images.Count; i++)
                {
                    // If the data is completely outside of the level, it's useless and unreachable; don't bother saving it.
                    // If the type doesn't exist, then it's corrupt; get rid of it
                    if (images[i].Location.X / 2 < width && (images[i].Location.X + images[i].Width) / 2 > 0 && (images[i].Location.Y + images[i].Height) / 2 > 0 && images[i].Location.Y / 2 < 450 && types[i] >= 0 && types[i] <= 9)
                    {
                        bW.Write(types[i]);
                        bW.Write(images[i].Location.X);
                        bW.Write(images[i].Location.Y);
                    }
                }
                bW.Close();
                changed = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // setting type
            type = 0;
            typeBox.Text = type.ToString();
        }

        private void buildingTwo_Click(object sender, EventArgs e)
        {
            // setting type
            type = 1;
            typeBox.Text = type.ToString();
        }

        private void player_Click(object sender, EventArgs e)
        {
            // setting type
            type = 2;
            typeBox.Text = type.ToString();
        }
        private void ZombieOne_Click(object sender, EventArgs e)
        {
            // setting type
            type = 3;
            typeBox.Text = type.ToString();
        }

        private void zombieTwo_Click(object sender, EventArgs e)
        {
            // setting type
            type = 4;
            typeBox.Text = type.ToString();
        }

        private void buildingThree_Click(object sender, EventArgs e)
        {
            type = 5;
            typeBox.Text = type.ToString();
        }

        private void buildingFour_Click(object sender, EventArgs e)
        {
            type = 6;
            typeBox.Text = type.ToString();
        }
        private void buildingFive_Click(object sender, EventArgs e)
        {
            type = 7;
            typeBox.Text = type.ToString();
        }

        private void buildingSix_Click(object sender, EventArgs e)
        {
            type = 8;
            typeBox.Text = type.ToString();
        }
        private void bossZombie_Click(object sender, EventArgs e)
        {
            type = 9;
            typeBox.Text = type.ToString();
        }



        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void closeButton_Click(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                DialogResult save = MessageBox.Show("Would you like to save your work?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (save == DialogResult.Yes)
                {
                    // opening save dialog
                    if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
                    {
                        if (saveFileDialog1.FileName == null)
                        {
                            Close();
                        }

                        bWStream = File.OpenWrite(saveFileDialog1.FileName); // now saving file with user's input file name
                        bWStream.SetLength(0);
                        bW = new BinaryWriter(bWStream);
                        bW.Write(width);
                        for (int i = 0; i < images.Count; i++)
                        {
                            if (images[i].Location.X < width && images[i].Location.X + images[i].Width > 0 && images[i].Location.Y + images[i].Height > 0 && images[i].Location.Y < 450 && types[i] >= 0 && types[i] <= 9)
                            {
                                bW.Write(types[i]);
                                bW.Write(images[i].Location.X);
                                bW.Write(images[i].Location.Y);
                            }
                        }
                        bW.Close();
                    }
                    e.Cancel = false;
                }
                else if (save == DialogResult.No)
                {
                    e.Cancel = false;
                }
                else if (save == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!mouseDown)
            {
                int.TryParse(textBox3.Text, out y);
                y = y * 2;
                pictureBox3.Location = new Point(x - 2, y - pictureBox3.Image.Height + 2);
                pictureBox3.BringToFront();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool cancel = false;
            // opening open dialog
            if (changed)
            {
                DialogResult save = MessageBox.Show("Would you like to save your work?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (save == DialogResult.Yes)
                {
                    // opening save dialog
                    if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
                    {
                        if (saveFileDialog1.FileName == null)
                        {
                            Close();
                        }

                        bWStream = File.OpenWrite(saveFileDialog1.FileName); // now saving file with user's input file name
                        bWStream.SetLength(0);
                        bW = new BinaryWriter(bWStream);
                        bW.Write(width);
                        for (int i = 0; i < images.Count; i++)
                        {
                            if (images[i].Location.X < width && images[i].Location.X + images[i].Width > 0 && images[i].Location.Y + images[i].Height > 0 && images[i].Location.Y < 450 && types[i] >= 0 && types[i] <= 9)
                            {
                                bW.Write(types[i]);
                                bW.Write(images[i].Location.X);
                                bW.Write(images[i].Location.Y);
                            }
                        }
                        bW.Close();
                    }
                }
                else if (save == DialogResult.Cancel)
                {
                    cancel = true;
                }
            }
            if (!cancel)
            {
                if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    if (openFileDialog1.FileName == null)
                    {
                        Close();
                    }

                    for (int i = 0; i < images.Count; i++)
                    {
                        images[i].Dispose();
                    }
                    images.Clear();
                    types.Clear();
                    bRStream = File.OpenRead(openFileDialog1.FileName); // now loading file with user's input file name
                    bR = new BinaryReader(bRStream);
                    width = bR.ReadInt32();
                    pictureBox1.ClientSize = new Size(width * 2, pictureBox1.ClientSize.Height);
                    textBox2.Text = width.ToString();
                    for (int i = 0; bRStream.Position < bRStream.Length; i++)
                    {
                        types.Add(bR.ReadInt32());
                        images.Add(new PictureBox());
                        images[i].Name = i.ToString();
                        images[i].BackColor = Color.Transparent;
                        images[i].Parent = pictureBox1;
                        switch (types[i])
                        {
                            case 0:
                                {
                                    images[i].Image = buildingOne.Image;
                                    images[i].ClientSize = buildingOne.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 1:
                                {
                                    images[i].Image = buildingTwo.Image;
                                    images[i].ClientSize = buildingTwo.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 2:
                                {
                                    images[i].Image = player.Image;
                                    images[i].ClientSize = player.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 3:
                                {
                                    images[i].Image = ZombieOne.Image;
                                    images[i].ClientSize = ZombieOne.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 4:
                                {
                                    images[i].Image = zombieTwo.Image;
                                    images[i].ClientSize = zombieTwo.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 5:
                                {
                                    images[i].Image = buildingThree.Image;
                                    images[i].ClientSize = buildingThree.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 6:
                                {
                                    images[i].Image = buildingFour.Image;
                                    images[i].ClientSize = buildingFour.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 7:
                                {
                                    images[i].Image = buildingFive.Image;
                                    images[i].ClientSize = buildingFive.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 8:
                                {
                                    images[i].Image = buildingSix.Image;
                                    images[i].ClientSize = buildingSix.Image.Size;
                                    images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                    break;
                                }
                            case 9:
                            {
                                images[i].Image = bossZombie.Image;
                                images[i].ClientSize = bossZombie.Image.Size;
                                images[i].Location = new Point(bR.ReadInt32(), bR.ReadInt32());
                                break;
                            }
                        }
                        images[i].MouseDown += new MouseEventHandler(images_MouseDown);
                        images[i].MouseUp += new MouseEventHandler(images_MouseUp);
                        images[i].MouseMove += new MouseEventHandler(images_MouseMove);
                        images[i].KeyDown += new KeyEventHandler(images_KeyDown);
                        images[i].BringToFront();
                    }
                    bR.Close();
                    changed = false;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool cancel = false;
            // opening open dialog
            if (changed)
            {
                DialogResult save = MessageBox.Show("Would you like to save your work?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (save == DialogResult.Yes)
                {
                    // opening save dialog
                    if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
                    {
                        if (saveFileDialog1.FileName == null)
                        {
                            Close();
                        }

                        bWStream = File.OpenWrite(saveFileDialog1.FileName); // now saving file with user's input file name
                        bWStream.SetLength(0);
                        bW = new BinaryWriter(bWStream);
                        bW.Write(width);
                        for (int i = 0; i < images.Count; i++)
                        {
                            if (images[i].Location.X < width && images[i].Location.X + images[i].Width > 0 && images[i].Location.Y + images[i].Height > 0 && images[i].Location.Y < 450 && types[i] >= 0 && types[i] <= 9)
                            {
                                bW.Write(types[i]);
                                bW.Write(images[i].Location.X);
                                bW.Write(images[i].Location.Y);
                            }
                        }
                        bW.Close();
                    }
                }
                else if (save == DialogResult.Cancel)
                {
                    cancel = true;
                }
            }
            if (!cancel)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    images[i].Dispose();
                }
                images.Clear();
                types.Clear();
                changed = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(textBox2.Text, out width);
            if (width != 0)
            {
                pictureBox1.ClientSize = new Size(width * 2, pictureBox1.ClientSize.Height);
                changed = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!mouseDown)
            {
                int.TryParse(textBox1.Text, out x);
                x = x * 2;
                pictureBox3.Location = new Point(x - 2, y - pictureBox3.Image.Height + 2);
                pictureBox3.BringToFront();
            }
        }

        private void images_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            PictureBox picBox = sender as PictureBox;
            Point newPosition = new Point(PointToClient(MousePosition).X - panel1.Location.X, PointToClient(MousePosition).Y - panel1.Location.Y);
            picBox.Focus();
            pictureBox3.Location = new Point(newPosition.X - 2, newPosition.Y - pictureBox3.ClientSize.Height + 2);
            changed = true;
        }

        private void images_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void images_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseDown)
            {
                PictureBox picBox = sender as PictureBox;
                Point newPosition = new Point(((PointToClient(MousePosition).X - panel1.AutoScrollPosition.X - panel1.Location.X) / 2) * 2, ((PointToClient(MousePosition).Y - panel1.Location.Y) / 2) * 2);
                pictureBox3.Location = new Point(((newPosition.X - 2) / 2) * 2, ((newPosition.Y - pictureBox3.ClientSize.Height + 2) / 2) * 2);
                picBox.Location = new Point(newPosition.X, newPosition.Y - picBox.ClientSize.Height);
                textBox1.Text = (picBox.Location.X / 2).ToString();
                int.TryParse(textBox1.Text, out x);
                x = x * 2;
                textBox3.Text = ((picBox.Location.Y + picBox.ClientSize.Height) / 2).ToString();
                int.TryParse(textBox3.Text, out y);
                y = y * 2;
            }
        }

        private void images_KeyDown(object sender, KeyEventArgs e)
        {
            PictureBox picBox = sender as PictureBox;
            if (e.KeyCode == Keys.Delete && !mouseDown && picBox.Focused)
            {
                int i;
                int.TryParse(picBox.Name, out i);
                picBox.Dispose();
                images.RemoveAt(i);
                types.RemoveAt(i);
                for(; i < images.Count; i++)
                {
                    images[i].Name = i.ToString();
                }
                changed = true;
            }
        }
    }
}
