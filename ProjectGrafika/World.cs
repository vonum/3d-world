using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ProjectGrafika 
{
    using Tao.OpenGl;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using Tao.Platform.Windows;
    using System.Windows.Forms;
    using System.IO;
    using System.Reflection;

    class World : IDisposable
    {

        #region Attributes

        private enum TextureObjects { WOOD = 0, DOOR, ROOF, METAL, GRASS, LANE };
        private int texture_count = Enum.GetNames(typeof(TextureObjects)).Length;
        private int[] texture_id = null;

        public int world_height;
        public int world_width;

        private AssimpScene scene;

        Glu.GLUquadric gluObject = Glu.gluNewQuadric();
        
        public Box box;
        
        private Font font;
        private int font_id = -1;

        public bool door_open;
        public bool door_closed;
        public bool door_opening;
        public bool door_closing;

        public float eyeX = 0.0f;
        public float eyeY = 0.0f;
        public float eyeZ = 50.0f;
        public float angleH = 2.5f;
        public float angleV = 0.0f;
        public float r = 50.0f;

        public bool app_locked;
        public bool balloon_moving_forward;
        public bool balloon_moving_upward;
        public bool balloon_in_air;

        public String[] text = { "Predmet : Racunarska grafika", "Sk. godina: 2015/16.", "Ime : Milan", "Prezime: Keca", "Sifra zad: 8.1" };
        public String[] texture_urls = { "..//..//images//wood.jpg", "..//..//images//door.jpg", "..//..//images//roof.jpg", "..//..//images//metal.jpg", "..//..//images//grass.jpg", "..//..//images//lane.jpg" };

        public float anthena_height = 60.0f;

        public float rotation_x = 0.0f;
        public float rotation_y = 0.0f;
        public float distance = -200;
        public float trans_y = -50;

        public float door_rotation = 0.0f;
        public float rotation_step = 15.0f;

        public float balloon_x = 0.0f;
        public float balloon_y = 0.0f;
        public float balloon_z = 0.0f;

        private float[] lightPosPoint;
        private float[] lightPosRef;
        private float[] refDirection = { 0.0f, -1.0f, 0.0f };

        #endregion

        #region Constructors

        public World(int world_height, int world_width)
        {
            door_open = false;
            door_closed = true;
            door_opening = false;
            door_closing = false;

            app_locked = false;
            balloon_moving_forward = false;
            balloon_moving_upward = false;
            balloon_in_air = false;

            this.world_height = world_height;
            this.world_width = world_width;
            box = new Box(100, 90, 70);
            font = new Font("Ariel", 10, FontStyle.Bold);
            createFont();
            scene = new AssimpScene(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Model"), "Hot_Air_Balloon.obj");
            Glu.gluQuadricNormals(gluObject, Glu.GLU_SMOOTH);

            texture_id = new int[texture_count];

            lightPosPoint = new float[] { -200.0f, 0.0f, 0.0f, 1.0f };
            lightPosRef = new float[] { 0.0f, (float)box.Height*1.5f, 0.0f, 1.0f };

            this.Initialize();
        }

        #endregion

        #region DrawingMethods
        public void Draw()
        {
            //begin
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glViewport(0, 0, world_width, world_height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluPerspective(60, (double)world_width / world_height, 1, 1000);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            //pozicija kamere
            Gl.glPushMatrix();

            //pomeranje da ne bi kamera bila u sceni
            Gl.glTranslatef(0.0f, trans_y, distance);

            calculateCameraPos();
            Glu.gluLookAt(eyeX, eyeY, eyeZ, 0, 0, 0, 0, 1, 0);

            //pomeranje reflektorskog svetla
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, lightPosRef);

            /*Gl.glPushMatrix();
            Gl.glTranslatef(0.0f, (float)box.Height * 1.2f, 0.0f);
            Glu.gluSphere(gluObject, 5, 128, 128);
            Gl.glPopMatrix();*/

            Gl.glColor3f(0.0f, 1.0f, 0.0f);

            //crtanje balona
            Gl.glPushMatrix();

                //Gl.glTranslatef(balloon_x, balloon_y, balloon_z);
                //Gl.glScalef(0.007f, 0.007f, 0.007f);
                //pomeranje balona
                Gl.glTranslatef(balloon_x - 23.5f, balloon_y, balloon_z + 10.0f);
                Gl.glScalef(0.09f, 0.09f, 0.09f);           //0.007
                //Glu.gluSphere(gluObject, 200.0f, 128, 128);
                Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
                scene.Draw();

            Gl.glPopMatrix();

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_DECAL);
            //crtanje podloge
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_id[(int)TextureObjects.GRASS]);

            //skaliranje texture trave
            Gl.glMatrixMode(Gl.GL_TEXTURE);
            Gl.glLoadIdentity();
            Gl.glScalef(10.0f, 10.0f, 10.0f);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            Gl.glColor3ub(0, 145, 45);
            drawGrass();

            //vracanje na pocetnu matricu za teksture
            Gl.glMatrixMode(Gl.GL_TEXTURE);
            Gl.glLoadIdentity();
            Gl.glMatrixMode(Gl.GL_MODELVIEW);


            //crtanje staze - GL_BLEND da bi se ocuvala boja staze
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_BLEND);


            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_id[(int)TextureObjects.LANE]);
            Gl.glColor3ub(92, 92, 138);
            drawLane();

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);

            //crtanje hangara
            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            Gl.glPushMatrix();

                Gl.glTranslatef(0.0f, (float)box.Height / 2, 0.0f);
                Gl.glDisable(Gl.GL_CULL_FACE);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_id[(int)TextureObjects.WOOD]);
                box.Draw();
                Gl.glEnable(Gl.GL_CULL_FACE);

            Gl.glPopMatrix();

            //crtanje krova
            Gl.glColor3ub(180, 45, 45);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_id[(int)TextureObjects.ROOF]);
            //Gl.glPushMatrix();
            drawRoof();
            //Gl.glPopMatrix();

            //crtanje vrata 
            Gl.glColor3ub(70, 5, 5);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_id[(int)TextureObjects.DOOR]);
            drawDoor();
            
            //crtanje antene            //visina cilindra je 70
            Glu.gluQuadricTexture(gluObject, 1);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_id[(int)TextureObjects.METAL]);
            drawAnthenna();


            Gl.glPopMatrix();

            //ispis teksta
            Gl.glViewport(0, 0, world_width, world_height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluOrtho2D(-world_width / 2.0, world_width / 2.0, -world_height / 2.0, world_height / 2.0);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            Gl.glColor3ub(255, 255, 255);
            //iskljucivanje osvetljenja zbog boje teksta
            Gl.glDisable(Gl.GL_LIGHTING);
            drawText();
            Gl.glEnable(Gl.GL_LIGHTING);

            //end
            Gl.glFlush();
        }

        private void drawRoof()
        {
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);

            Gl.glNormal3fv(Lighting.FindFaceNormal(0.0f, (float)box.Height + (float)box.Height / 3, 0.0f,
                                                (float)-box.Width / 2, (float)box.Height, -(float)box.Depth / 2,
                                                (float)-box.Width / 2, (float)box.Height, (float)box.Depth / 2));
            Gl.glTexCoord2f(0.5f, 1.0f);
            Gl.glVertex3f(0.0f, (float)box.Height + (float)box.Height / 3, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3f((float)-box.Width / 2, (float)box.Height, -(float)box.Depth / 2);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3f((float)-box.Width / 2, (float)box.Height, (float)box.Depth / 2);

            Gl.glNormal3fv(Lighting.FindFaceNormal(0.0f, (float)box.Height + (float)box.Height / 3, 0.0f,
                                                (float)-box.Width / 2, (float)box.Height, (float)box.Depth / 2,
                                                (float)box.Width / 2, (float)box.Height, (float)box.Depth / 2));
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3f((float)box.Width / 2, (float)box.Height, (float)box.Depth / 2);
            Gl.glNormal3fv(Lighting.FindFaceNormal(0.0f, (float)box.Height + (float)box.Height / 3, 0.0f,
                                                (float)box.Width / 2, (float)box.Height, (float)box.Depth / 2,
                                                (float)box.Width / 2, (float)box.Height, -(float)box.Depth / 2));
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3f((float)box.Width / 2, (float)box.Height, -(float)box.Depth / 2);
            Gl.glNormal3fv(Lighting.FindFaceNormal(0.0f, (float)box.Height + (float)box.Height / 3, 0.0f,
                            (float)box.Width / 2, (float)box.Height, -(float)box.Depth / 2,
                            (float)-box.Width / 2, (float)box.Height, -(float)box.Depth / 2));
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3f((float)-box.Width / 2, (float)box.Height, -(float)box.Depth / 2);
            Gl.glEnd();
        }

        private void drawDoor()
        {
            //crtanje vrata - lefa
            Gl.glPushMatrix();

                Gl.glTranslatef(-(float)box.Width / 4, 0.0f, (float)box.Depth / 2 + 0.01f);
                if (true)
                {
                    Gl.glRotatef(-door_rotation, 0.0f, 1.0f, 0.0f);
                }
                Gl.glDisable(Gl.GL_CULL_FACE);
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glNormal3f(0.0f, 0.0f, 1.0f);
                Gl.glTexCoord2f(0.0f, 0.0f);
                Gl.glVertex3f(0.0f, 0.0f, 0.0f);
                Gl.glTexCoord2f(0.5f, 0.0f);
                Gl.glVertex3f((float)box.Width / 4, 0.0f, 0.0f);
                Gl.glTexCoord2f(0.5f, 1.0f);
                Gl.glVertex3f((float)box.Width / 4, (float)box.Height * 5 / 6, 0.0f);
                Gl.glTexCoord2f(0.0f, 1.0f);
                Gl.glVertex3f(0, (float)box.Height * 5 / 6, 0.0f);

                Gl.glEnd();
                Gl.glEnable(Gl.GL_CULL_FACE);

            Gl.glPopMatrix();

            //crtanje vrata - desna
            Gl.glPushMatrix();

                Gl.glTranslatef((float)box.Width / 4, 0.0f, (float)box.Depth / 2 + 0.01f);
                if (true)
                {
                    Gl.glRotatef(door_rotation, 0.0f, 1.0f, 0.0f);
                }

                Gl.glDisable(Gl.GL_CULL_FACE);
                Gl.glBegin(Gl.GL_QUADS);
                //Gl.glNormal3f(0.0f, 0.0f, 1.0f);
                Gl.glTexCoord2f(1.0f, 0.0f);
                Gl.glVertex3f(0.0f, 0.0f, 0.0f);
                Gl.glTexCoord2f(1.0f, 1.0f);
                Gl.glVertex3f(0.0f, (float)box.Height * 5 / 6, 0.0f);
                Gl.glTexCoord2f(0.5f, 1.0f);
                Gl.glVertex3f(-(float)box.Width / 4, (float)box.Height * 5 / 6, 0.0f);
                Gl.glTexCoord2f(0.5f, 0.0f);
                Gl.glVertex3f(-(float)box.Width / 4, 0.0f, 0.0f);
                Gl.glEnd();
                Gl.glEnable(Gl.GL_CULL_FACE);

            Gl.glPopMatrix();
        }

        private void drawAnthenna()
        {
            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            Gl.glPushMatrix();

                Gl.glTranslatef(25.0f, (float)box.Height, 0.0f);
                Gl.glRotatef(-90.0f, 1.0f, 0.0f, 0.0f);
                Glu.gluCylinder(gluObject, 1.5f, 1.0f, anthena_height, 128, 128);

            Gl.glPopMatrix();

            //crtanje manjeg cilindra
            Gl.glPushMatrix();

                Gl.glTranslatef(25.0f, (float)box.Height + anthena_height, 0.0f);
                Gl.glRotatef(-45.0f, 1.0f, 0.0f, 0.0f);
                Glu.gluCylinder(gluObject, 1.0f, 0.8f, 10.0f, 128, 128);

            Gl.glPopMatrix();

            //crtanje manje lopte na vrhu antene
            Gl.glColor3ub(255, 102, 51);
            Gl.glPushMatrix();

                Gl.glTranslatef(25.0f, (float)box.Height + anthena_height + 1.41f * 5.0f, 1.41f * 5.0f);
                Glu.gluSphere(gluObject, 2.0f, 128, 128);

            Gl.glPopMatrix();

            //crtanje vece lopte kod diska
            Gl.glPushMatrix();

                Gl.glTranslatef(25.0f, (float)box.Height + anthena_height, 0.0f);
                Glu.gluSphere(gluObject, 4.0f, 128, 128);

            Gl.glPopMatrix();

            //crtanje diska
            Gl.glPushMatrix();

                Gl.glDisable(Gl.GL_CULL_FACE);
                Gl.glTranslatef(25.0f, (float)box.Height + anthena_height, 0.0f);
                Gl.glRotatef(15.0f, 1.0f, 0.0f, 0.0f);
                Glu.gluDisk(gluObject, 0.0f, 14.0f, 128, 128);
                Gl.glEnable(Gl.GL_CULL_FACE);

            Gl.glPopMatrix();
        }

        private void drawLane()
        {
            Gl.glBegin(Gl.GL_QUADS);

                Gl.glNormal3f(0.0f, 1.0f, 0.0f);
                Gl.glTexCoord2d(0, 1);
                Gl.glVertex3f(-(float)box.Width / 4, 0.0f, (float)box.Depth / 2 + 10.0f);
                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3f(-(float)box.Width / 4, 0.0f, 200.0f);
                Gl.glTexCoord2d(1, 1);
                Gl.glVertex3f((float)box.Width / 4, 0.0f, 200.0f);
                Gl.glTexCoord2d(1, 0);
                Gl.glVertex3f((float)box.Width / 4, 0.0f, (float)box.Depth / 2 + 10.0f);

                //oko kuce, za 10.0f
                //Gl.glNormal3f(0.0f, 1.0f, 0.0f);
                Gl.glTexCoord2d(0, 0);
                Gl.glVertex3f(-(float)box.Width / 2 - 10.0f, -0.005f, (float)box.Depth / 2 + 10.0f);
                Gl.glTexCoord2d(1, 0);
                Gl.glVertex3f((float)box.Width / 2 + 10.0f, -0.005f, (float)box.Depth / 2 + 10.0f);
                Gl.glTexCoord2d(1, 1);
                Gl.glVertex3f((float)box.Width / 2 + 10.0f, -0.005f, -(float)box.Depth / 2 - 10.0f);
                Gl.glTexCoord2d(0, 1);
                Gl.glVertex3f(-(float)box.Width / 2 - 10.0f, -0.005f, -(float)box.Depth / 2 - 10.0f);

            Gl.glEnd();
        }

        private void drawGrass()
        {
            Gl.glBegin(Gl.GL_QUADS);

            Gl.glNormal3f(0.0f, 1.0f, 0.0f);
            Gl.glTexCoord2d(0, 0);
            Gl.glVertex3f(-400.0f, -0.02f, 400.0f);
            Gl.glTexCoord2d(1, 0);
            Gl.glVertex3f(400.0f, -0.02f, 400.0f);
            Gl.glTexCoord2d(1, 1);
            Gl.glVertex3f(400.0f, -0.02f, -400.0f);
            Gl.glTexCoord2d(0, 1);
            Gl.glVertex3f(-400.0f, -0.02f, -400.0f);

            Gl.glEnd();
        }

        private void drawText()
        {
            Gl.glRasterPos2f(world_width / 2 - CalculateTextWidth(text[0]) + 10, -world_height / 2 + 4 * font.Height);
            drawText(text[0]);
            Gl.glRasterPos2f(world_width / 2 - CalculateTextWidth(text[0]) + 10, -world_height / 2 + 3 * font.Height);
            drawText(text[1]);
            Gl.glRasterPos2f(world_width / 2 - CalculateTextWidth(text[0]) + 10, -world_height / 2 + 2 * font.Height);
            drawText(text[2]);
            Gl.glRasterPos2f(world_width / 2 - CalculateTextWidth(text[0]) + 10, -world_height / 2 + font.Height);
            drawText(text[3]);
            Gl.glRasterPos2f(world_width / 2 - CalculateTextWidth(text[0]) + 10, -world_height / 2);
            drawText(text[4]);
        }


        #endregion

        private void Initialize()
        {
            float[] whiteLight = { 1.0f, 1.0f, 1.0f, 1.0f };

            // Vrednosti svetlosnih komponenti
            float[] ambientLight = { 0.3f, 0.3f, 0.3f, 1.0f }; 
            float[] diffuseLight = { 0.7f, 0.7f, 0.7f, 1.0f };
            float[] specularLight = { 0.5f, 0.5f, 0.5f, 1.0f };

            float[] specularMaterial = { 1.0f, 1.0f, 1.0f, 1.0f };

            float[] red = { 1.0f, 0.0f, 0.0f, 1.0f };

            Gl.glEnable(Gl.GL_CULL_FACE);
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            // Ukljuci color tracking
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            // Podesi na koje parametre materijala se odnose pozivi glColor funkcije
            Gl.glColorMaterial(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE);

            // Ukjuci proracun osvetljenja
            Gl.glEnable(Gl.GL_LIGHTING);
            //Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, ambientLight);

            // Specifikuj i ukljuci svetlosni izvor 0(tackasti svetlosni izvor)
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, ambientLight);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, diffuseLight);
            Gl.glEnable(Gl.GL_LIGHT0);

            // Reflektorski svetlosni izvor
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, red);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, diffuseLight);
            Gl.glLightf(Gl.GL_LIGHT1, Gl.GL_SPOT_CUTOFF, 25.0f);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPOT_DIRECTION, refDirection);
            Gl.glEnable(Gl.GL_LIGHT1);

            // Definisemo belu spekularnu komponentu materijala sa jakim odsjajem
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, specularMaterial);
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, 128.0f);

            // Ukljuci automatsku normalizaciju nad normalama
            Gl.glEnable(Gl.GL_NORMALIZE);

            Gl.glShadeModel(Gl.GL_SMOOTH);

            loadTextures();

            // Predji u rezim rada sa 2D teksturama
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            // Podesi nacin blending teksture
            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);
            Gl.glLightModeli(Gl.GL_LIGHT_MODEL_COLOR_CONTROL, Gl.GL_SEPARATE_SPECULAR_COLOR);

            // Boja pozadine je bela 
            Gl.glClearColor(0.0f, 0.0f, 0.2588f, 1.0f);

            resize();

        }

        public void resize()
        {
            Glu.gluSphere(gluObject, 200.0f, 128, 128);

            Gl.glViewport(0, 0, world_width, world_height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluPerspective(60, (double)world_width / world_height, 1, 500);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // Pozicioniranje svetlosnog izvora, koji je neosetljiv na transformacije
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightPosPoint);

        }

        #region FontMethods

        public void createFont()
        {
            font_id = Gl.glGenLists(256);

            Gdi.SelectObject(Wgl.wglGetCurrentDC(), font.ToHfont());

            Wgl.wglUseFontBitmapsW(Wgl.wglGetCurrentDC(),                                   // aktivni DC
                                                  0,								        // pocetni karakter
                                                  255,							            // broj DL koji se kreiraju
                                                  font_id);						            // DL identifikator

            // Deselektuj aktivni font
            Gdi.SelectObject(Wgl.wglGetCurrentDC(), IntPtr.Zero);

        }

        public void drawText(String text)
        {
            if (text.Length != 0)
            {
                Gl.glPushAttrib(Gl.GL_LIST_BIT);     // sacuvamo stanje DL steka
                Gl.glListBase(font_id);  		           // pozicioniraj se na pocetak DL
                Gl.glCallLists(text.Length,
                               Gl.GL_UNSIGNED_SHORT, // STRING JE UNICODE, pa mora 2 bajta!
                               text);	               // ispis DL teksta

                Gl.glPopAttrib();                    // sacuvamo stanje DL steka
            }

        }

        public float CalculateTextWidth(String text)
        {
            Size textSize = TextRenderer.MeasureText(text, font);

            return textSize.Width;
        }

        public float CalculateTextHeight(String text)
        {
            Size textSize = TextRenderer.MeasureText(text, font);

            return textSize.Height;
        }

        #endregion

        #region DisposeMethods

        ~World()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Oslodi managed resurse
            }

            // Oslobodi unmanaged resurse
            scene.Dispose();
            font.Dispose();
            Glu.gluDeleteQuadric(gluObject);
        }

        #endregion

        #region Textures

        private void loadTextures()
        {
            // Ucitaj slike i kreiraj teksture
            Gl.glGenTextures(texture_count, texture_id);
            for (int i = 0; i < texture_count; ++i)
            {
                // Pridruzi teksturu odgovarajucem identifikatoru
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture_id[i]);

                // Ucitaj sliku i podesi parametre teksture
                Bitmap image = new Bitmap(texture_urls[i]);
                // rotiramo sliku zbog koordinantog sistema opengl-a
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                // RGBA format (dozvoljena providnost slike tj. alfa kanal)
                BitmapData imageData = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                                                      System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, (int)Gl.GL_RGBA8, image.Width, image.Height, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, imageData.Scan0);
                // Podesi parametre teksture
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);

                image.UnlockBits(imageData);
                image.Dispose();
            }

        }

        #endregion

        #region AnimationMethods

        public void openDoor()
        {
            if (door_rotation + rotation_step < 120.0f)
            {
                door_rotation += rotation_step;
            }
            else
            {
                door_rotation = 120.0f;
                door_open = true;
                door_opening = false;
            }
        }

        public void closeDoor()
        {
            if (door_rotation - rotation_step > 0)
            {
                door_rotation -= rotation_step;
            }
            else
            {
                door_rotation = 0.0f;
                door_closed = true;
                door_open = false;
                door_closing = false;
            }
        }

        public void moveBalloonForward()
        {
            if (!door_open)
            {
                openDoor();
            }
            else
            {
                if (balloon_z + 5 < 100)
                {
                    balloon_z += 5;
                }
                else
                {
                    balloon_z = 100;
                    balloon_moving_forward = false;
                    balloon_moving_upward = true;
                }
            }
        }

        public void moveBalloonUpward(Form1 form)
        {
            if (balloon_y + 5 < 120)
            {
                balloon_y += 5;
            }
            else
            {
                balloon_moving_upward = false;
                balloon_in_air = true;
                app_locked = false;
                form.configureInput(true);
            }
        }

        public void resetBalloonPosition()
        {
            balloon_y = 0;
            balloon_z = 0;
        }


        #endregion

        public void calculateCameraPos()
        {
            eyeX = (float)(r * -Math.Sin(angleH) * Math.Cos(angleV));
            eyeY = (float)(r * -Math.Sin(angleV));
            eyeZ = (float)(-r * Math.Cos(angleH) * Math.Cos(angleV));
        }
    }
}
