using ImageDisplayApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageDisplayApp
{
    public partial class Global_Map : Form
    {
        private const string Map1Path = @"\map1.png";
        private const string Map2Path = @"\map2.png";
        private string pathToUse = null;
        private string dir = "";
        public Global_Map(string path = null)
        {
            InitializeComponent();
            pathToUse = path ?? Map1Path;
            dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!dir.EndsWith(@"CharacterLibrary\Files"))
                dir = GetPathToGlobalFiles(dir);
        }

        private void GlobalMap_Load(object sender, EventArgs e)
        {
            SetupMap();
        }

        public void Refresh(string mapPath = null)
        {
            if (mapPath == null)
                pathToUse = pathToUse == Map1Path ? Map2Path : Map1Path;
            else
                pathToUse = mapPath;

            using (var writer = new StreamWriter("asdf.txt"))
            {
                writer.WriteLine(pathToUse);
            }
            Invoke(new Action(() => pictureBox1.Image.Dispose()));
            Invoke(new Action(() => SetupMap()));
            Invoke(new Action(() => base.Refresh()));
        }

        private string GetPathToGlobalFiles(string dllDir)
        {
            var path = Directory.GetParent(dllDir).Parent.Parent.FullName + @"\Files";
            return path;
        }

        private void SetupMap()
        {
            try
            {
                string map = null;
                map = pathToUse.Length < 10 ? dir + pathToUse : pathToUse;
                Image image = Image.FromFile(map);
                pictureBox1.Image = image;
                Height = pictureBox1.Height = image.Height;
                Width = pictureBox1.Width = image.Width;
            }
            catch (Exception exception)
            {
                using (var writer = new StreamWriter("asdf.txt"))
                {
                    writer.WriteLine(exception);
                    writer.WriteLine(dir);
                }
            }
        }
    }
}
