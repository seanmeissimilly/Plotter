using System;
using System.Windows.Forms;

namespace TestingXControls
{
    /// <summary>
    /// Form para ejecutar la instrucción read.
    /// </summary>
    public partial class Read : Form
    {
        string valor;
        public Read()
        {
            InitializeComponent();
        }

        //Botón OK.
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                valor = "";
                if (textBox1.Text == "") throw new Exception("Has not written the value of the variable.");

                valor += textBox1.Text;

                //aquí le paso el texto a mi clase Intérprete para poder evaluarlo.
                Interprete.Recibido(valor);
                Close();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }
        
    }
}
