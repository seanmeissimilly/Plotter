using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Jerarquia;
using Tokenizing;
using XControls;

namespace TestingXControls
{
    public partial class Graficador : Form
    {
        ///Constructor al cual le paso las funciones que voy a graficar.
        public Graficador(IEnumerable<Funcion> funciones, IEnumerable<Tuple<List<Funcion>, List<List<Token>>>>  xpartes)
        {
            InitializeComponent();
            
              //Creo un objeto Random y un Array de Color para pintar las funciones de diferentes colores.
                var decidecolor = new Random();
                Color[] colores = { Color.Black, Color.Yellow, Color.Red, Color.Magenta, Color.Lime, Color.DarkBlue, Color.White };

                // Funciones normales.
                // Realizo un ciclo para pintar cada función.
                foreach (var funcion in funciones)
                {
                    functionsViewer1.Functions.Add(new FunctionInfo()
                    {
                        Name = "f",
                        Function = x => (float)(funcion.Evaluar(x)),
                        Color = colores[decidecolor.Next(0, 7)]
                    });
                }

            //Lleno mi diccionario de comparadores.
            if (xpartes.ToList().Count>0) Interprete.FillDictionary();
            
                // Funciones por partes.
                // Realizo un ciclo para pintar cada función.
            foreach (var t in xpartes)
            {
                var indice=0;
                functionsViewer1.Functions.Add(new FunctionInfo()
                {
                    Name = "f",
                    Function = x => (float)(Evaluar(x, t, indice)),
                    Color = colores[decidecolor.Next(0, 7)]
                });
            }
        }

        private double Evaluar(double x, Tuple<List<Funcion>, List<List<Token>>> t, int indice)
        {
            if (indice == t.Item2.Count) return t.Item1[indice].Evaluar(x);
            for (var i = indice; i < t.Item2.Count; i++)
             if (Interprete.EvaluarComparador(t.Item2[i], x)) 
                 return t.Item1[i].Evaluar(x);
            return Evaluar(x, t, indice += 1);
        }
    }
}
