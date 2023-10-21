using System;
using System.Collections.Generic;
using System.Linq;
using Tokenizing;

namespace Jerarquia
{

    /// <summary>
    /// Clase para evaluar expresiones en notación polaca inversa (RPN)
    /// </summary>
    public static class Parser
    {
        //delegado para invocar a mis funciones.
        public delegate Funcion Factory(params Funcion[] args);

        //Diccionario en el cual voy a tener almacenado el símbolo con la funcion que representa.
        public static Dictionary<string, Factory> library = new Dictionary<string, Factory>();
       

        //Tokenizar con el cual voy a realizar todas operaciones de obtener tokens.
        static Tokenizer reader = new Tokenizer();

        /// <summary>
        /// En el construtor lleno el diccionario.
        /// </summary>
        static Parser()
        {
            FillLibrary();
        }

        /// <summary>
        /// Este método es para introducirle los valores al diccionario .
        /// </summary>
        private static void FillLibrary()
        {
            library.Add("+", Suma.Create);
            library.Add("-", Resta.Create);
            library.Add("*", Multliplicacion.Create);
            library.Add("/", Division.Create);
            library.Add("^", Pow.Create);
            library.Add("%", Resto.Create);
            library.Add("number", Constante.Create);
            library.Add("x", Identidad.Create);
            library.Add("sin", Sin.Create);
            library.Add("cos", Cos.Create);
            library.Add("ln", LogN.Create);
            library.Add("tan", Tan.Create);
            library.Add("cot", Cot.Create);
            library.Add("arcsin", Arcsin.Create);
            library.Add("arcos", Arcos.Create);
            library.Add("arctan", Arctan.Create);
            library.Add("sec", Sec.Create);
            library.Add("csc", Csc.Create);
            library.Add("sign", Sign.Create);
            library.Add("abs", Abs.Create);
        }

        /// <summary>
        ///Método para llevar un array de tokens a notacion posfija o notacion polaca inversa.
        /// <summary>
        public static Queue<Token> ShuntingYard(Token[] tokens)
        {
            var contfuncion = 0;
            var temp = new Token();
            var salida = new Queue<Token>();
            var operadores = new Stack<Token>();

            for (var i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].Type == TokenKind.Number || tokens[i].Type == TokenKind.Identifier && tokens[i].Text == "x")
                {
                    salida.Enqueue(tokens[i]); continue;
                }
                if (tokens[i].Type == TokenKind.Identifier)
                {
                    if (i < tokens.Length - 1 && tokens[i + 1].Text != "(") throw new Exception("Esta mal la entrada.");
                    operadores.Push(tokens[i]);
                    contfuncion++;
                    temp = tokens[i];
                }
                if (temp.Type == TokenKind.Identifier && tokens[i].Text == ")")
                {
                    while (operadores.Count > 0 && operadores.Peek().Text != "(") salida.Enqueue(operadores.Pop());

                    contfuncion--;

                    if (operadores.Count > 0)
                    {
                        if (contfuncion == 0) temp = operadores.Pop();

                        else operadores.Pop();

                        salida.Enqueue(operadores.Pop());
                    }
                }
                
                if (tokens[i].Type == TokenKind.Symbol)
                {
                    while (operadores.Count > 0 && operadores.Peek().Type == TokenKind.Symbol && tokens[i].Text != "(" && tokens[i].Text != ")" && GetPrecedence(tokens[i]) <= GetPrecedence(operadores.Peek()))
                        
                        salida.Enqueue(operadores.Pop());

                    if (tokens[i].Text != "(" && tokens[i].Text != ")") operadores.Push(tokens[i]);
                }

                if (tokens[i].Text == "(") operadores.Push(tokens[i]);

                if (tokens[i].Text == ")")
                {
                    while (operadores.Count != 0 && operadores.Peek().Text != "(") salida.Enqueue(operadores.Pop());

                    if (operadores.Count > 0 && operadores.Peek().Text == "(") operadores.Pop();
                }
            }
            while (operadores.Count > 0) salida.Enqueue(operadores.Pop());

            return salida;
        }

        /// <summary>
        ///Método para saber la precedencia de los operadores.
        /// <summary>
        private static int GetPrecedence(Token token)
        {
            switch (token.Text)
            {
                case "+": return 1;
                case "-": return 1;
                case "%": return 1;
                case "*": return 2;
                case "/": return 2;
                case "^": return 3;
                case "`": return 4;
            }
            return 0;
        }

        /// <summary>
        ///Este método es para arreglar la entrada con los signos "-" .
        /// <summary>
        private static string InputFixing(string instruction)
        {
            var fix = "";

            if (instruction[0] == '-') fix += "0-";
            for (var i = 0; i < instruction.Length; i++)
            {
                if (i == 0 && instruction[i] == '-') continue;
                if (instruction[i] == '-')
                {
                    var current = reader.GetTokens(instruction[i - 1].ToString()).GetEnumerator();
                    current.MoveNext();
                    var aux = current.Current;

                    if (aux.Text == "(" || aux.Type == TokenKind.Unknown) fix += "0-";
                    
                    else fix += "+(0-1)*";
                    
                    continue;
                }

                fix += instruction[i];
            }
            return fix;
        }

        /// <summary>
        ///Este método es para saber si los paréntesis están balanceados.
        /// <summary>
        private static bool IsBalanced(List<Token> token)
        {
            var parenthesis = new Stack<string>();

            for (var i = 0; i < token.Count(); i++)
            {
                switch (token[i].Text)
                {
                    case "(":
                        parenthesis.Push(token[i].Text);//Si me encuentro un parentesis abierto lo introduzco en la pila.
                        break;
                    case ")":
                        if (parenthesis.Count > 0) parenthesis.Pop();

                        else return false;
                        break;
                }

            }
            //Si la pila esta vacia la instruccion esta balanceada.
            return parenthesis.Count == 0;
        }

        /// <summary>
        ///Este método es para evaluar instrucciones y devolver una funcion.
        /// <summary>
        public static Funcion Evaluar(string code)
        {
            code = InputFixing(code);
            var tokens = reader.GetTokens(code).ToArray();

            var result = new Stack<Funcion>();
            Funcion right, left;

            //Reviso si los paréntesis están balanceados.
            if (!IsBalanced(tokens.ToList())) throw new InvalidOperationException("Los parentesis no están balanceados.");

            //Llevo la instrución a notación posfija.
            var exit = ShuntingYard(tokens.ToArray());

            if (exit.Count > 0)
            {
                foreach (var token in exit)
                {
                    if (token.Type == TokenKind.Number || token.Type == TokenKind.Identifier && token.Text == "x")
                    {
                        if (token.Type == TokenKind.Number) result.Push(new Constante(double.Parse(token.Text)));

                        if (token.Text == "x") result.Push(new Identidad());
                    }

                    else if (result.Count < 1) throw new InvalidOperationException("La entrada es incorrecta");
                    
                    else switch (token.Type)
                        {
                            case TokenKind.Symbol:

                                if (token.Text == "`")
                                {
                                    right = result.Pop();
                                    result.Push(right.Derivate());
                                    continue;
                                }
                                right = result.Pop();
                                left = result.Pop();
                                result.Push(library[token.Text].Invoke(left, right));

                                break;
                            case TokenKind.Identifier:
                                right = result.Pop();
                                result.Push(library[token.Text].Invoke(right));
                                break;
                        }
                }
            }
            if (result.Count > 1) throw new Exception();

            return result.Pop();
        }
    }
}
