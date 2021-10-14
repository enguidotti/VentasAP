using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasAP
{
    public static class Colores
    {
        public static Color PrimaryColor { get; set; }
        public static Color SecondaryColor { get; set; }
        public static List<string> ColorList = new List<string>() { "#3F51B5", //azul
                                                                    "#009688", //verdeinicio
                                                                    "#FF5722", //naranjo
                                                                    "#607D8B", //gris azul
                                                                    "#FF9800", //naranjo más claro
                                                                    "#9C27B0", //purple
                                                                    "#2196F3", //celeste
                                                                    "#EA676C", //rojo claro
                                                                    "#E41A4A", //rojo
                                                                    "#5978BB", //otro celeste
                                                                    "#018790", //turquesa
                                                                    "#0E3441", //azul oscuro
                                                                    "#00B0AD", //verde más claro
                                                                    "#721D47", //beterraga
                                                                    "#EA4833", //otro naranjo
                                                                    "#EF937E", //damasco
                                                                    "#F37521", //naranjo
                                                                    "#A12059", 
                                                                    "#126881",
                                                                    "#8BC240",
                                                                    "#364D5B",
                                                                    "#C7DC5B",
                                                                    "#0094BC",
                                                                    "#E4126B",
                                                                    "#43B76E",
                                                                    "#7BCFE9",
                                                                    "#B71C46"};
        public static Color ChangeColorBrightness(Color color, double correctionFactor)
        {
            double red = color.R;
            double green = color.G;
            double blue = color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }

            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }
            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }
    }
}
