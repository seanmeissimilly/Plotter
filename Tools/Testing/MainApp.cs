using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Tokenizing;
using TestingXControls;


namespace Testing
{
    public partial class MainApp : Form
    {
       //static Graficador hola =new Graficador();
       // public static void Creador()
       // { 
       //     Application.Run(hola);
       // }
       // Thread a=new Thread(new ThreadStart(Creador));
       // public MainApp()
       // {
       //     a.Start();
       //     InitializeComponent();
       // }

        private void getTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear text boxes for errors and output
            errorsTextBox.Text = "";
            outputTextBox.Text = "";

            // Create a tokenizer of type MSharpTokenizer
            Tokenizer tokenizer = new Tokenizer();

            // Bind a method to handle errors
            tokenizer.Error += tokenizer_Error;

            // Get a IEnumerable object of tokens
            var tokens = tokenizer.GetTokens(codeTextBox.Text);

            // Set output of each token produced using certain format.
            outputTextBox.Text = string.Join("\n\r", tokens.Select(t => string.Format("{0} {1}", t.Type, t.Text)));
        }

        void tokenizer_Error(string errorMessage, int line, int col)
        {
            // Each error is appened to error textbox.
            errorsTextBox.Text += string.Format("{0} at {1}:{2} \n\r", errorMessage, line, col);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void outputTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
