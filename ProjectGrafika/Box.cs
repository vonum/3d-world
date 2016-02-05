// -----------------------------------------------------------------------
// <file>Box.cs</file>
// <copyright>Grupa za Grafiku, Interakciju i Multimediju 2012.</copyright>
// <author>Srdjan Mihic</author>
// <summary>Klasa koja enkapsulira OpenGL programski kod za iscrtavanje kvadra sa tezistem u koord.pocetku.</summary>
// -----------------------------------------------------------------------
namespace ProjectGrafika
{
    using Tao.OpenGl;

    /// <summary>
    ///  Klasa enkapsulira OpenGL kod za iscrtavanje kvadra.
    /// </summary>
    public class Box
    {
        #region Atributi

        /// <summary>
        ///	 Visina kvadra.
        /// </summary>
        double m_height = 1.0;

        /// <summary>
        ///	 Sirina kvadra.
        /// </summary>
        double m_width = 1.0;

        /// <summary>
        ///	 Dubina kvadra.
        /// </summary>
        double m_depth = 1.0;

        #endregion Atributi

        #region Properties

        /// <summary>
        ///	 Visina kvadra.
        /// </summary>
        public double Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        ///	 Sirina kvadra.
        /// </summary>
        public double Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        /// <summary>
        ///	 Dubina kvadra.
        /// </summary>
        public double Depth
        {
            get { return m_depth; }
            set { m_depth = value; }
        }

        #endregion Properties

        #region Konstruktori

        /// <summary>
        ///		Konstruktor.
        /// </summary>
        public Box()
        {
        }

        /// <summary>
        ///		Konstruktor sa parametrima.
        /// </summary>
        /// <param name="width">Sirina kvadra.</param>
        /// <param name="height">Visina kvadra.</param>
        /// <param name="depth"></param>
        public Box(double width, double height, double depth)
        {
            this.m_width = width;
            this.m_height = height;
            this.m_depth = depth;
        }

        #endregion Konstruktori

        #region Metode

        public void Draw()
        {
            Gl.glBegin(Gl.GL_QUADS);

            // Zadnja
            Gl.glNormal3f(0.0f, 0.0f, -1.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);    //FL
            Gl.glVertex3d(-m_width / 2, -m_height / 2, -m_depth / 2);
            Gl.glTexCoord2f(0.0f, 1.0f);    //BL
            Gl.glVertex3d(-m_width / 2, m_height / 2, -m_depth / 2);
            Gl.glTexCoord2f(1.0f, 1.0f);    //BR
            Gl.glVertex3d(m_width / 2, m_height / 2, -m_depth / 2);
            Gl.glTexCoord2f(1.0f, 0.0f);    //FR
            Gl.glVertex3d(m_width / 2, -m_height / 2, -m_depth / 2);

            // Desna
            Gl.glNormal3f(1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3d(m_width / 2, -m_height / 2, -m_depth / 2);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, m_height / 2, -m_depth / 2);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3d(m_width / 2, m_height / 2, m_depth / 2);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3d(m_width / 2, -m_height / 2, m_depth / 2);

            // Prednja
            Gl.glNormal3f(0.0f, 0.0f, 1.0f);
                // Desna strana
                Gl.glTexCoord2f(1.0f, 0.0f);
                Gl.glVertex3d(m_width / 2, -m_height / 2, m_depth / 2);
                Gl.glTexCoord2f(1.0f, 1.0f);
                Gl.glVertex3d(m_width / 2, m_height / 2, m_depth / 2);
                Gl.glTexCoord2f(0.0f, 1.0f);
                Gl.glVertex3d(m_width / 4, m_height / 2, m_depth / 2);
                Gl.glTexCoord2f(0.0f, 0.0f);
                Gl.glVertex3d(m_width / 4, -m_height / 2, m_depth / 2);
                // Leva strana
                Gl.glTexCoord2f(0.0f, 0.0f);
                Gl.glVertex3d(-m_width / 2, -m_height / 2, m_depth / 2);
                Gl.glTexCoord2f(1.0f, 0.0f);
                Gl.glVertex3d(-m_width / 4, -m_height / 2, m_depth / 2);
                Gl.glTexCoord2f(1.0f, 1.0f);
                Gl.glVertex3d(-m_width / 4, m_height / 2, m_depth / 2);
                Gl.glTexCoord2f(0.0f, 1.0f);
                Gl.glVertex3d(-m_width / 2, m_height / 2, m_depth / 2);
                // Gornja strana
                Gl.glTexCoord2f(0.0f, 0.0f);
                Gl.glVertex3d(-m_width / 4, m_height / 3, m_depth / 2);
                Gl.glTexCoord2f(1.0f, 0.0f);
                Gl.glVertex3d(m_width / 4, m_height / 3, m_depth / 2);
                Gl.glTexCoord2f(1.0f, 1.0f);
                Gl.glVertex3d(m_width / 4, m_height / 2, m_depth / 2);
                Gl.glTexCoord2f(0.0f, 1.0f);
                Gl.glVertex3d(-m_width / 4, m_height / 2, m_depth / 2);

            // Leva
            Gl.glNormal3f(-1.0f, 0.0f, 0.0f);
            Gl.glTexCoord2f(0.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2, m_depth / 2);
            Gl.glTexCoord2f(0.0f, 1.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, m_depth / 2);
            Gl.glTexCoord2f(1.0f, 1.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, -m_depth / 2);
            Gl.glTexCoord2f(1.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2, -m_depth / 2);

            // Gornja
            Gl.glNormal3f(0.0f, 1.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, m_height / 2, -m_depth / 2);
            Gl.glVertex3d(-m_width / 2, m_height / 2, m_depth / 2);
            Gl.glVertex3d(m_width / 2, m_height / 2, m_depth / 2);
            Gl.glVertex3d(m_width / 2, m_height / 2, -m_depth / 2);

            Gl.glColor3f(0.0f, 0.0f, 0.1f);
            // Donja
            Gl.glNormal3f(0.0f, 1.0f, 0.0f);
            Gl.glVertex3d(-m_width / 2, -m_height / 2 + 0.01f, -m_depth / 2);
            Gl.glVertex3d(m_width / 2, -m_height / 2 + 0.01f, -m_depth / 2);
            Gl.glVertex3d(m_width / 2, -m_height / 2 + 0.01f, m_depth / 2);
            Gl.glVertex3d(-m_width / 2, -m_height / 2 + 0.01f, m_depth / 2);

            Gl.glEnd();
        }

        public void SetSize(double width, double height, double depth)
        {
            m_depth = depth;
            m_height = height;
            m_width = width;
        }

        #endregion Metode
    }
}
