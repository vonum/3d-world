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
    public class AssimpScene : IDisposable
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

        /// <summary>
        ///	 Identifikator tekstura.
        /// </summary>
        private int[] m_texIds;

        /// <summary>
        ///	 Mapiranje teksture na njen identifikator.
        /// </summary>
        private Dictionary<TextureSlot, int> m_texMappings;

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
        public AssimpScene(String scenePath, String sceneFileName)
        {
            this.m_scenePath = scenePath;
            this.m_sceneFileName = sceneFileName;
            this.m_texMappings = new Dictionary<TextureSlot, int>();

            LoadScene();
            Initialize();
        }

        /// <summary>
        ///  Destruktor klase AssimpScene.
        /// </summary>
        ~AssimpScene()
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
            // Scena se iscrtava pozivom odgovarajuce DL liste. Promenljive stanja se cuvaju pre i restauriraju posle poziva.

            Gl.glPushAttrib(Gl.GL_LIGHTING_BIT);
            Gl.glPushAttrib(Gl.GL_ENABLE_BIT);
            Gl.glPushAttrib(Gl.GL_TEXTURE_BIT);
            Gl.glPushAttrib(Gl.GL_POLYGON_BIT);
            Gl.glPushAttrib(Gl.GL_CURRENT_BIT);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_CULL_FACE);
            Gl.glFrontFace(Gl.GL_CCW);
            Gl.glColor3f(0.0f, 0.0f, 0.0f);
            Gl.glCallList(m_modelDL);
            Gl.glPopAttrib();
            Gl.glPopAttrib();
            Gl.glPopAttrib();
            Gl.glPopAttrib();
        }

        /// <summary>
        ///  Osvezavanje scene, koje je potrebno izvrsiti nakon izmene Scene objekta.
        /// </summary>
        public void ReloadScene()
        {
            // Oslobadjanje postojece DL liste.
            Gl.glDeleteLists(m_modelDL, 1);

            // Kreiranje nove DL liste i iscrtavanje scene. Promenljive stanja se cuvaju pre i restauriraju posle iscrtavanja.
            m_modelDL = Gl.glGenLists(1);
            Gl.glNewList(m_modelDL, Gl.GL_COMPILE);

            Gl.glPushAttrib(Gl.GL_LIGHTING_BIT);
            Gl.glPushAttrib(Gl.GL_ENABLE_BIT);
            Gl.glPushAttrib(Gl.GL_TEXTURE_BIT);
            Gl.glPushAttrib(Gl.GL_POLYGON_BIT);
            Gl.glPushAttrib(Gl.GL_CURRENT_BIT);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_CULL_FACE);
            Gl.glFrontFace(Gl.GL_CCW);
            Gl.glColor3f(0.0f, 0.0f, 0.0f);
            RenderNode(m_scene.RootNode);
            Gl.glPopAttrib();
            Gl.glPopAttrib();
            Gl.glPopAttrib();
            Gl.glPopAttrib();
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
            m_scene = importer.ImportFile(Path.Combine(m_scenePath, m_sceneFileName), PostProcessPreset.TargetRealTimeMaximumQuality);

            // Oslobadjanje resursa koriscenih za ucitavanje podataka o sceni.
            importer.Dispose();
        }

        /// <summary>
        ///  Inicijalizacija i podesavanje OpenGL parametara.
        /// </summary>
        private void Initialize()
        {
            try
            {
                // Ucitavanje tekstura.
                LoadTextures();

                // Kreiranje nove DL liste i iscrtavanje scene. Promenjive stanja se cuvaju pre i restauriraju posle iscrtavanja.
                m_modelDL = Gl.glGenLists(1);
                Gl.glNewList(m_modelDL, Gl.GL_COMPILE);

                Gl.glPushAttrib(Gl.GL_LIGHTING_BIT);
                Gl.glPushAttrib(Gl.GL_ENABLE_BIT);
                Gl.glPushAttrib(Gl.GL_TEXTURE_BIT);
                Gl.glPushAttrib(Gl.GL_POLYGON_BIT);
                Gl.glPushAttrib(Gl.GL_CURRENT_BIT);
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                Gl.glEnable(Gl.GL_CULL_FACE);
                Gl.glFrontFace(Gl.GL_CCW);
                RenderNode(m_scene.RootNode);
                Gl.glPopAttrib();
                Gl.glPopAttrib();
                Gl.glPopAttrib();
                Gl.glPopAttrib();
                Gl.glEndList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///  Icrtavanje objekata u sceni koji su reprezentovani datim cvorom, kao i njegovim pod-cvorovima. 
        ///  U zavisnosti od karakteristika objekata podesavaju se odgovarajuce promenjive stanja (GL_LIGHTING, GL_COLOR_MATERIAL, GL_TEXTURE_2D).
        /// </summary>
        /// <param name="node">Cvor koji ce biti iscrtan.</param>
        private void RenderNode(Node node)
        {
            Gl.glPushMatrix();

            // Primena tranformacija vezanih za dati cvor.
            float[] matrix = new float[16] { node.Transform.A1, node.Transform.B1, node.Transform.C1, node.Transform.D1, node.Transform.A2, node.Transform.B2, node.Transform.C2, node.Transform.D2, node.Transform.A3, node.Transform.B3, node.Transform.C3, node.Transform.D3, node.Transform.A4, node.Transform.B4, node.Transform.C4, node.Transform.D4 };
            Gl.glMultMatrixf(matrix);

            // Iscrtavanje objekata u sceni koji su reprezentovani datim cvorom.
            if (node.HasMeshes)
            {
                foreach (int index in node.MeshIndices)
                {
                    Mesh mesh = m_scene.Meshes[index];
                    Material material = m_scene.Materials[mesh.MaterialIndex];

                    // Primena komponenti materijala datog objekta.
                    ApplyMaterial(material);

                    // Primena teksture u slucaju da je ista definisana za dati materijal.
                    if (material.GetAllTextures().Length > 0)
                        Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_texMappings[material.GetAllTextures()[0]]);

                    // Podesavanje proracuna osvetljenja za dati objekat.
                    bool hasNormals = mesh.HasNormals;
                    if (hasNormals)
                        Gl.glEnable(Gl.GL_LIGHTING);
                    else
                        Gl.glDisable(Gl.GL_LIGHTING);

                    // Podesavanje color tracking mehanizma za dati objekat.
                    bool hasColors = mesh.HasVertexColors(0);
                    if (hasColors)
                        Gl.glEnable(Gl.GL_COLOR_MATERIAL);
                    else
                        Gl.glDisable(Gl.GL_COLOR_MATERIAL);

                    // Podesavanje rezima mapiranja na teksture.
                    bool hasTexCoords = material.GetAllTextures().Length > 0 && mesh.HasTextureCoords(0);
                    if (hasTexCoords)
                        Gl.glEnable(Gl.GL_TEXTURE_2D);
                    else
                        Gl.glDisable(Gl.GL_TEXTURE_2D);

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
                            uint indice = face.Indices[i];

                            // Definisanje boje temena.
                            if (hasColors)
                                Gl.glColor4f(mesh.GetVertexColors(0)[indice].R, mesh.GetVertexColors(0)[indice].G, mesh.GetVertexColors(0)[indice].B, mesh.GetVertexColors(0)[indice].A);

                            // Definisanje normale temena.
                            if (hasNormals)
                                Gl.glNormal3f(mesh.Normals[indice].X, mesh.Normals[indice].Y, mesh.Normals[indice].Z);

                            // Definisanje koordinata teksture temena.
                            if (hasTexCoords)
                                Gl.glTexCoord2f(mesh.GetTextureCoords(0)[indice].X, 1 - mesh.GetTextureCoords(0)[indice].Y);

                            // Definisanje temena primitive.
                            Gl.glVertex3f(mesh.Vertices[indice].X, mesh.Vertices[indice].Y, mesh.Vertices[indice].Z);
                        }
                        Gl.glEnd();
                    }
                }
            }

            // Rekurzivno iscrtavanje cvorova potomaka tekuceg cvora
            for (int i = 0; i < node.ChildCount; i++)
            {
                RenderNode(node.Children[i]);
            }

            Gl.glPopMatrix();
        }

        /// <summary>
        ///  Primena razlicitih komponenti datog materijala (ambijentalna, difuzna, spekularna, emisiona, sjaj).
        /// </summary>
        /// <param name="material">Materijal cije ce karakteristike biti primenjene.</param>
        private void ApplyMaterial(Material material)
        {
            // Primena ambijentalne komponente datog materijala. U slucaju da ista nije definisana, koristi se podrazumevana vrednost.
            float[] ambientColor = material.HasColorAmbient ? new float[] { material.ColorAmbient.R, material.ColorAmbient.G, material.ColorAmbient.B, material.ColorAmbient.A } : new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, ambientColor);

            // Primena difuzne komponente datog materijala. U slucaju da ista nije definisana, koristi se podrazumevana vrednost.
            float[] diffuseColor = material.HasColorDiffuse ? new float[] { material.ColorDiffuse.R, material.ColorDiffuse.G, material.ColorDiffuse.B, material.ColorDiffuse.A } : new float[] { 0.8f, 0.8f, 0.8f, 1.0f };
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, diffuseColor);

            // Primena spekularne komponente datog materijala. U slucaju da ista nije definisana, koristi se podrazumevana vrednost.
            float[] specularColor = material.HasColorSpecular ? new float[] { material.ColorSpecular.R, material.ColorSpecular.G, material.ColorSpecular.B, material.ColorSpecular.A } : new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, specularColor);


            // Primena emisione komponente datog materijala. U slucaju da ista nije definisana, koristi se podrazumevana vrednost.
            float[] emissiveColor = material.HasColorEmissive ? new float[] { material.ColorEmissive.R, material.ColorEmissive.G, material.ColorEmissive.B, material.ColorEmissive.A } : new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, emissiveColor);

            // Primena sjaja materijala. U slucaju da ista nije definisana, koristi se podrazumevana vrednost.
            float shininess = material.HasShininess ? material.Shininess : 1.0f;
            float strength = material.HasShininessStrength ? material.ShininessStrength : 1.0f;
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, shininess * strength);
        }

        /// <summary>
        ///  Ucitavanje tekstura.
        /// </summary>
        /// <param name="textureSlot">Informacije o teksturi koja ce biti ucitana.</param>
        private void LoadTextures()
        {
            int texCount = 0;
            foreach (Material material in m_scene.Materials)
            {
                foreach (TextureSlot texSlot in material.GetAllTextures())
                {
                    texCount++;
                }
            }

            //if (texCount > 0)
            //{
            m_texIds = new int[texCount];
            Gl.glGenTextures(texCount, m_texIds);

            int index = 0;
            foreach (Material material in m_scene.Materials)
            {
                foreach (TextureSlot texSlot in material.GetAllTextures())
                {
                    m_texMappings[texSlot] = m_texIds[index];

                    // Pridruzi teksturu odgovarajucem identifikatoru.
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, m_texIds[index]);

                    // Formiranje putanje do fajla koji predstavlja teksturu.
                    String fileName = Path.Combine(m_scenePath, texSlot.FilePath.StartsWith("/") ? texSlot.FilePath.Substring(1) : texSlot.FilePath);
                    if (!File.Exists(fileName))
                        throw new ArgumentException();

                    // Ucitavanje teksture iz datog fajla.
                    Bitmap textureBitmap = new Bitmap(fileName);
                    BitmapData textureData = textureBitmap.LockBits(new Rectangle(0, 0, textureBitmap.Width, textureBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA8, textureData.Width, textureData.Height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, textureData.Scan0);

                    // Podesavanje filtriranja teksture.
                    Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                    Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);

                    // Podesavanje ponavljanja teksture za dati materijal. 
                    if (texSlot.WrapModeU == TextureWrapMode.Clamp)
                        Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP);
                    if (texSlot.WrapModeV == TextureWrapMode.Clamp)
                        Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP);
                    if (texSlot.WrapModeU == TextureWrapMode.Wrap)
                        Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
                    if (texSlot.WrapModeV == TextureWrapMode.Wrap)
                        Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);

                    // Oslobadjanje resursa teksture.
                    textureBitmap.UnlockBits(textureData);
                    textureBitmap.Dispose();

                    index++;
                }
            }
        }

        /// <summary>
        ///  Metoda za oslobadjanje resursa.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Oslodi managed resurse.
            }

            // Oslobodi unmanaged resurse.
            Gl.glDeleteLists(m_modelDL, 1);
            Gl.glDeleteTextures(m_texIds.Length, m_texIds);
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
