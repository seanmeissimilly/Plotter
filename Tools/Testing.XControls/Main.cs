using System;
using System.Windows.Forms;

namespace TestingXControls
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        //Botón help.
        private void aboToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Plotter v1.0" + " David Sean Meissimilly Frometa C-121", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Botón salvar.
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (module_name.Text == "")
            {
                MessageBox.Show("Debe asignarle un nombre al módulo", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Interprete.SaveModule(module_name.Text, false, inputboxtext.Text);
            }
            catch (Exception a)
            {
                var d = MessageBox.Show(a.Message, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (d == DialogResult.OK)
                {
                    Interprete.SaveModule(module_name.Text, true, inputboxtext.Text);
                }
            }
        }

        //Botón compilar.
        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //borro el textbox de salida por si tenía algo impreso
                exitboxtext.Clear();
                Interprete.Organizes(inputboxtext.Text);
                
                //Aquí es donde ejecuto la instrucción "print".
                var print = Interprete.imprimir;
                foreach (var t in print) exitboxtext.Text += t + "\r\n";
            }

            catch (Exception a)
            {
                exitboxtext.Text = a.Message;
            }
        }

        //Botón graficar.
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form graficador = new Graficador(Interprete.graficar, Interprete.graficarxpartes);
                graficador.ShowDialog();
            }
            catch (Exception)
            {
                exitboxtext.Text = "No existen funciones para graficar.";
            }
            
        }

        //Botón salir.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
