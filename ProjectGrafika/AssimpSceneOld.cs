// -----------------------------------------------------------------------
// <file>AssimpScene.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2013.</copyright>
// <author>Zoran Milicevic, Stefan Negovanovic</author>
// <summary>Klasa enkapsulira programski kod za ucitavanje modela pomocu na AssimpNet biblioteke i prikazivanje modela uz uslonac na TaoFramework biblioteku.</summary>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assimp;
using Assimp.Configs;
using Tao.OpenGl;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ProjectGrafika
{
    /// <summary>
    /// Klasa enkapsulira programski kod za ucitavanje modela pomocu AssimpNet biblioteke i prikazivanje modela uz uslonac na TaoFramework biblioteku.
    /// </summary>
    public class AssimpSceneOld : IDisposable
    {
        #region Atributi

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        private Scene m_scene;

        /// <summary>
        ///	 Putanja do foldera u kojem se nalaze podaci o sceni.
        /// </summary>
        private String m_scenePath;

        /// <summary>
        ///	 Naziv fajla u kojem se nalaze podaci o sceni.
        /// </summary>
        private String m_sceneFileName;

        /// <summary>
        ///	 Identifikator mesh modela.
        /// </summary>
        private int m_modelDL;

        /////<summary>
        ///// Generator slucajnih brojeva, koji sluzi za generisanje boje poligona.
        /////</summary>
        //private Random m_random;

        #endregion

        #region Properties

        /// <summary>
        ///	 Scena koja se prikazuje.
        /// </summary>
        public Scene Scene
        {
            get { return m_scene; }
            private set { m_scene = value; }
        }

        #endregion

        #region Konstruktori

        /// <summary>
        ///  Konstruktor klase AssimpScene.
        /// </summary>
        /// <param name="scenePath">Putanja do foldera u kojem se nalaze podaci o sceni.</param>
        /// <param name="sceneFileName">Naziv fajla u kojem se nalaze podaci o sceni.</param>
        public AssimpSceneOld(String scenePath, String sceneFileName)
        {
            this.m_scenePath = scenePath;
            this.m_sceneFileName = sceneFileName;
            //this.m_random = new Random();
            LoadScene();
            Initialize();
        }

        /// <summary>
        ///  Destruktor klase AssimpScene.
        /// </summary>
        ~AssimpSceneOld()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public metode

        /// <summary>
        ///  Iscrtavanje scene.
        /// </summary>
        public void Draw()
        {
            Gl.glCallList(m_modelDL);
        }

        /// <summary>
        ///  Osvezavanje scene, koje je potrebno izvrsiti nakon izmene Scene objekta.
        /// </summary>
        public void Update()
        {
            // Oslobadjanje postojece DL liste.
            Gl.glDeleteLists(m_modelDL, 1);

            // Kreiranje nove DL liste i iscrtavanje scene. Promenljive stanja se cuvaju pre i restauriraju posle iscrtavanja.
            m_modelDL = Gl.glGenLists(1);
            Gl.glNewList(m_modelDL, Gl.GL_COMPILE);
            RenderNode(m_scene.RootNode);
            Gl.glEndList();
        }

        #endregion

        #region Private metode

        /// <summary>
        ///  Ucitavanje podataka o sceni iz odgovarajuceg fajla.
        /// </summary>
        private void LoadScene()
        {
            // Instanciranje klase za ucitavanje podataka o sceni.
            AssimpImporter importer = new AssimpImporter();

            // Definisanje callback delegata za belezenje poruka u toku ucitavanja podataka o sceni.
            LogStream logstream = new LogStream(delegate(String msg, String userData)
            {
                Console.WriteLine(msg);
            });
            importer.AttachLogStream(logstream);

            // Ucitavanje podataka o sceni iz odgovarajuceg fajla.
            m_scene = importer.ImportFile(Path.Combine(m_scenePath, m_sceneFileName));

            // Oslobadjanje resursa koriscenih za ucitavanje podataka o sceni.
            importer.Dispose();
        }

        /// <summary>
        ///  Inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        private void Initialize()
        {
            // Kreiranje nove DL liste i iscrtavanje scene.
            m_modelDL = Gl.glGenLists(1);
            Gl.glNewList(m_modelDL, Gl.GL_COMPILE);
            RenderNode(m_scene.RootNode);
            Gl.glEndList();
        }

        /// <summary>
        ///  Rekurzivna metoda zaduzena za iscrtavanje objekata u sceni koji su reprezentovani cvorovima. 
        ///  U zavisnosti od karakteristika objekata podesavaju se odgovarajuce promenjive stanja (GL_LIGHTING, GL_COLOR_MATERIAL, GL_TEXTURE_2D).
        /// </summary>
        /// <param name="node">Cvor koji ce biti iscrtan.</param>
        private void RenderNode(Node node)
        {
            Gl.glPushMatrix();

            // Primena tranformacija, definisanih za dati cvor.
            float[] matrix = new float[16] { node.Transform.A1, node.Transform.B1, node.Transform.C1, node.Transform.D1, node.Transform.A2, node.Transform.B2, node.Transform.C2, node.Transform.D2, node.Transform.A3, node.Transform.B3, node.Transform.C3, node.Transform.D3, node.Transform.A4, node.Transform.B4, node.Transform.C4, node.Transform.D4 };
            Gl.glMultMatrixf(matrix);

            // Iscrtavanje objekata u sceni koji su reprezentovani datim cvorom.
            if (node.HasMeshes)
            {
                foreach (int meshIndex in node.MeshIndices)
                {
                    Mesh mesh = m_scene.Meshes[meshIndex];

                    bool hasColors = mesh.HasVertexColors(0);

                    // Iscrtavanje primitiva koji cine dati objekat.
                    // U zavisnosti od broja temena, moguce je iscrtavanje tacaka, linija, trouglova ili poligona.
                    foreach (Face face in mesh.Faces)
                    {
                        switch (face.IndexCount)
                        {
                            case 1:
                                Gl.glBegin(Gl.GL_POINTS);
                                break;
                            case 2:
                                Gl.glBegin(Gl.GL_LINES);
                                break;
                            case 3:
                                Gl.glBegin(Gl.GL_TRIANGLES);
                                break;
                            default:
                                Gl.glBegin(Gl.GL_POLYGON);
                                break;
                        }

                        for (int i = 0; i < face.IndexCount; i++)
                        {
                            uint vertexIndex = face.Indices[i];

                            // Definisanje boje temena.
                            if (hasColors)
                                Gl.glColor4f(mesh.GetVertexColors(0)[vertexIndex].R, mesh.GetVertexColors(0)[vertexIndex].G, mesh.GetVertexColors(0)[vertexIndex].B, mesh.GetVertexColors(0)[vertexIndex].A);

                            // Permutacija boje poligona u zavisnosti od parnosti indeksa
                            if (vertexIndex % 2 == 0)
                                Gl.glColor3f(0.3f, 0.3f, 0.3f);
                            else
                                Gl.glColor3f(0.4f, 0.4f, 0.4f);
                            
                            // Generisanje boje poligona pomoću generatora slučajnih brojeva
                            //float blue = (float)m_random.NextDouble();

                            //if (blue < 0.15f)
                            //    blue += 0.4f;
                            //else if (blue > 0.85f)
                            //    blue -= 0.4f;

                            //Gl.glColor3f(0, 0, blue);

                            // Definisanje temena primitive.
                            Gl.glVertex3f(mesh.Vertices[vertexIndex].X, mesh.Vertices[vertexIndex].Y, mesh.Vertices[vertexIndex].Z);
                        }
                        Gl.glEnd();
                    }
                }
            }

            // Rekurzivno scrtavanje podcvorova datog cvora.
            for (int i = 0; i < node.ChildCount; i++)
            {
                RenderNode(node.Children[i]);
            }

            Gl.glPopMatrix();
        }

        /// <summary>
        ///  Metoda za oslobadjanje resursa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    // Oslodi managed resurse.
            //}

            // Oslobodi unmanaged resurse.
            Gl.glDeleteLists(m_modelDL, 1);
        }

        #endregion Private metode

        #region IDisposable metode

        /// <summary>
        ///  Implementacija IDisposable interfejsa.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable metode
    }
}
