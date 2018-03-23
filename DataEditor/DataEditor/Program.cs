using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.Skins.Info;
using DevExpress.Internal;
using DevExpress.UserSkins;

namespace DataEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            SkinBlobXmlCreator skinCreator = new SkinBlobXmlCreator(
                "HybridApp", // skin name 
                "DataEditor.SkinData.", // path to the .blob file 
                typeof(Program).Assembly, // replace Form1 with your form class name 
                null);
            SkinManager.Default.RegisterSkin(skinCreator);
           DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("HybridApp");
            Application.Run(new MainForm ());
        }
        }
      
}
