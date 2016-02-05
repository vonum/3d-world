using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProjectGrafika
{
    using System.Windows.Forms;
    using System;

    public partial class Form1 : Form
    {
        private World world;


        public Form1()
        {
            InitializeComponent();
            openglControll.InitializeContexts();

            world = new World(openglControll.Height, openglControll.Width);
        }

        private void openglControll_Paint(object sender, PaintEventArgs e)
        {
            world.Draw();
        }

        private void openglControll_Resize(object sender, EventArgs e)
        {
            world.world_height = openglControll.Height;
            world.world_width = openglControll.Width;
            world.resize();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void openglControll_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F10: this.Close(); break;

                case Keys.W:
                    if (world.angleV - 0.05f < -1.55f)
                    {
                        world.angleV = -1.55f;
                    }
                    else
                    {
                        world.angleV -= 0.05f;
                    }

                    break;
                
                case Keys.S:

                    if (world.angleV + 0.05f > 0.15f)
                    {
                        world.angleV = 0.15f;
                    }
                    else
                    {
                        world.angleV += 0.05f;
                    }

                    break;

                case Keys.A: //world.rotation_y -= 5.0f; 

                    world.angleH -= 0.05f;
                    
                    break;
                
                case Keys.D: //world.rotation_y += 5.0f; 
                    
                    world.angleH += 0.05f;
                    
                    break;
                
                case Keys.P:

                    if (!world.app_locked)
                    {
                        if (world.door_open)
                        {
                            world.door_closing = true;
                            world.door_open = false;
                        }
                        else if (world.door_closed)
                        {
                            world.door_opening = true;
                            world.door_closed = false;
                        }
                    }
                        
                    break;
                
                case Keys.Add: 

                    if(world.distance < -120)
                    {
                        world.distance += 10;
                    }
                    break;

                case Keys.Subtract: 

                    if(world.distance > -280)
                    {
                        world.distance -= 10;
                    }
                    break;

                case Keys.X:
                    
                    if(!world.balloon_in_air)
                    {
                        world.balloon_moving_forward = true;
                        world.app_locked = true;
                        configureInput(false);
                    }
                    break;

                case Keys.Z:
                    
                    if(world.balloon_in_air)
                    {
                        world.resetBalloonPosition();
                        world.balloon_in_air = false;
                    }
                    break;

                case Keys.F2: this.Close(); break;

            }

            this.Refresh();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (world.door_opening)
            {
                world.openDoor();
            }
            else if (world.door_closing)
            {
                world.closeDoor();
            }

            if (world.balloon_moving_forward)
            {
                world.moveBalloonForward();
            }
            else if (world.balloon_moving_upward)
            {
                world.moveBalloonUpward(this);
            }


            this.Refresh();
        }

        private void hangar_input_ValueChanged(object sender, EventArgs e)
        {
            world.box.Height = (double)this.hangar_input.Value;
        }

        private void anthenna_input_ValueChanged(object sender, EventArgs e)
        {
            world.anthena_height = (float)this.anthenna_input.Value;
        }

        private void doorspeed_input_ValueChanged(object sender, EventArgs e)
        {
            world.rotation_step = (float)this.doorspeed_input.Value;
        }

        public void configureInput(bool value)
        {
            //this.hangar_input.Increment = value;
            //this.anthenna_input.Increment = value;
            //this.doorspeed_input.Increment = value;
            this.hangar_input.Enabled = value;
            this.anthenna_input.Enabled = value;
            this.doorspeed_input.Enabled = value;
        }
    }
}
