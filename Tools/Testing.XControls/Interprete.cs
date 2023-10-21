using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Jerarquia;
using Tokenizing;

namespace TestingXControls
{
    /// <summary>
    /// Esta clase es para interpretar todas las palabras del lenguaje M#
    /// </summary>
    static class Interprete
    {
        //Diccionario en el cual voy a tener almacenado todas las variables que el usuario ha declarado.
        private static Dictionary<string, double> variables;
        
        //Diccionario en el cual voy a tener almacenado todas las funciones que el usario ha declarado.
        private static Dictionary<string, Funcion> funciones;

        /* Diccionario en el cual voy a tener almacenado todas las funciones que el usario ha declarado 
        * con su representación en tokens.*/
        private static Dictionary<string, List<Token>> funcionestoken;

        //Tokenizar con el cual voy a realizar todas operaciones de obtener tokens.
        private static Tokenizer reader = new Tokenizer();

        //Lista donde voy a tener almacenada la instrucción que escribió el usuario.
        private static List<Token> instrution;

        //Lista donde voy a tener almacenada las funciones que el usuario desea graficar.
        internal static List<Funcion> graficar;

        //Lista donde voy a tener almacenada las valores que el usuario desea imprimir.
        internal static List<string> imprimir;

        //Lista donde voy a tener almacenada las valores que el usuario desea leer.
        private static List<Token> read;

        //Delegados para invocar a los operadores booleanos.
        private delegate IOperadoresLogicos OperBuilder(params bool[] args);

        private delegate IBooleanExpressions BooleanBuilder(params Funcion[] args);

        //Diccionario en el cual voy a tener almacenado todas los operadores lógicos.
        private static Dictionary<string, OperBuilder> logoperator;

        //Diccionario en el cual voy a tener almacenado todas los operadores booleanos.
        private static Dictionary<string, BooleanBuilder> logicaldic;

        //variable auxiliar.
        private static int index;

        //Diccionario en el cual voy a tener almacenado todas las funciones por partes.
        private static Dictionary<string, Tuple<List<Funcion>, List<List<Token>>>> fxpartes;

        //Lista donde voy a tener almacenada las funciones por partes que el usuario desea graficar.
        internal static List<Tuple<List<Funcion>, List<List<Token>>>> graficarxpartes;

        /// <summary>
        /// Constructor de la clase Intérprete.
        /// </summary>
        static Interprete()
        {
           //inicializo todas las variables.
           Inicializar();
        }

        /// <summary>
        /// Este método es para llenar el dicionario de expresiones de comparadores.
        /// </summary>
        internal static void FillDictionary()
        {
            logicaldic.Add("<", MenorQue.Create);
            logicaldic.Add(">", MayorQue.Create);
            logicaldic.Add("!=", Diferente.Create);
            logicaldic.Add("==", Igual.Create);
            logicaldic.Add(">=", MayorIgual.Create);
            logicaldic.Add("<=", MenorIgual.Create);
            logoperator.Add("&&",And.Create);
            logoperator.Add("||", Or.Create);
        }

        /// <summary>
        /// Este método inicializa todas las variables.
        /// </summary>
        private static void Inicializar()
        {
            graficarxpartes=new List<Tuple<List<Funcion>, List<List<Token>>>>();
            fxpartes=new Dictionary<string, Tuple<List<Funcion>, List<List<Token>>>>();
            logicaldic = new Dictionary<string, BooleanBuilder>();
            logoperator = new Dictionary<string, OperBuilder>();
            read = new List<Token>();
            imprimir = new List<string>();
            funciones = new Dictionary<string, Funcion>();
            graficar = new List<Funcion>();
            instrution = new List<Token>();
            variables = new Dictionary<string, double>();
            funcionestoken = new Dictionary<string, List<Token>>();
        }

        /// <summary>
        /// Guarda el código en un módulo.
        /// </summary>
        public static void SaveModule(string name_of_module, bool overwrite,string text)
        {
            if (File.Exists(name_of_module+".mat") && !overwrite) throw new InvalidOperationException("El módulo ya existe. Desea sobrescribirlo.");
            IFormatter formatter = new BinaryFormatter();
            var fs = new FileStream(name_of_module + ".mat", FileMode.Create);
            formatter.Serialize(fs,text);
            fs.Close();
        }

        /// <summary>
        /// Carga los módulos incluidos dentro del nuevo código.
        /// </summary>
        private static string LoadModule(string name_of_module)
        {
            var formatter = new BinaryFormatter();
            var fs = new FileStream(name_of_module + ".mat", FileMode.Open);
            if (fs == null)  throw new Exception("El módulo esta vacio."); 
            var code_loaded = (string)formatter.Deserialize(fs);
            fs.Close();
            return code_loaded;
        }

        /// <summary>
        /// Reinicio los valores de las variables y mando el texto que el usario escribió al método Compile.
        /// </summary>
        public static void Organizes(string code)
        {
            Inicializar();
            Compile(code);
        }

        /// <summary>
        /// Realiza el análisis del código y ejecuta todos los parses correspondientes.
        /// </summary>
        private static void Compile(string code)
        {
            Inicializar();
            var aux = 0;
            code = code + '\n';
            var tokens = reader.GetTokens(code).ToList();
            instrution.Clear();

            for (var i = 0; i < tokens.Count; i++)

                if (tokens[i].Type == TokenKind.Keyword)
                {
                    for (var j = i + 1; j < tokens.Count && tokens[j].Type != TokenKind.Separator ; j++)
                    {
                        instrution.Add(tokens[j]);
                        aux = j;
                    }
                    Classifies(tokens[i],instrution);
                    instrution.Clear();
                    i = aux;
                }
        }

        /// <summary>
        /// Este método clasifica la instrucción según la palabra clave.
        /// </summary>
        private static void Classifies(Token keyword, List<Token> expresion)
        {
            switch (keyword.Text)
            {
                case "let":
                {
                    var name = expresion[0].Text;
                    variables.Add(name,ParseaExpresion(GiveIntrution(expresion)));
                    break;
                }

                case "set":
                {
                    var name = expresion[0].Text;
                    if (!variables.ContainsKey(name)) throw new Exception("La variable no está declarada.");
                    variables[name] = ParseaExpresion(GiveIntrution(expresion));
                    break;
                }

                case "graph":
                {
                    if (fxpartes.ContainsKey(expresion[0].Text)) graficarxpartes.Add(fxpartes[expresion[0].Text]);
                    else graficar.Add(Parser.Evaluar(Desencriptar(expresion)));
                    break;
                }
                  
                case "def":
                {
                    var name = expresion[0].Text;

                    if (HayConditionals(GiveIntrution(expresion))) fxpartes.Add(name, ParsearCondicion(GiveIntrution(expresion)));
                    else funciones.Add(name, ParsearFuncion(GiveIntrution(expresion), name));
                    break;
                }
                    
                case "include":
                {
                    var name_of_module= expresion.TakeWhile(t => t.Type != TokenKind.Separator).Aggregate("", (current, t) => current + t.Text);
                    Compile(LoadModule(name_of_module));
                    break;
                }
                case "print":
                {
                    var name = expresion[0].Text;
                    Imprimir(name, expresion);
                    break;
                }
                case "read":
                {
                    var name = expresion[0].Text;
                    if (!variables.ContainsKey(name)) throw new Exception("La variable no está declarada.");
                    var leer = new Read();
                    leer.ShowDialog();
                    variables[name] = ParseaExpresion(read);
                    break;
                }
            }
        }

        /// <summary>
        /// Este método va recibir el texto que tengo que evaluar en la expresión read y mandar a parsearlo.
        /// </summary>
        internal static void Recibido(string code)
        {
            read.Clear();
            code = ArreglarEntrada(code);
            var temp = reader.GetTokens(code);
            read = temp.ToList();
        }

        /// <summary>
        /// Este método evalua las condicionales.
        /// </summary>
        private static Tuple<List<Funcion>,List<List<Token>>> ParsearCondicion(List<Token> tokens)
        {
           var entrega = new List<Funcion>();
           var condicion = new List<List<Token>>();
           var codigo = new List<Token>();
           return ParsearCondicion(tokens, entrega, condicion, codigo);
        }

        private static Tuple<List<Funcion>,List<List<Token>>> ParsearCondicion(List<Token> tokens,List<Funcion> funcion,List<List<Token>> condicion,List<Token> codigo  )
        {
            var haymas = false;
            index = 0;

            //copio la función que encabeza la instrucción, la evaluo y la guardo.
            foreach (var t in tokens.TakeWhile(t => t.Text != "if" && t.Text != "else"))
            {
                codigo.Add(t); index++;
            }
            funcion.Add(Parser.Evaluar(Desencriptar(codigo)));

            for (var t = index; t < tokens.Count; t++)
            {
                switch (tokens[t].Text)
                {
                    case "if":
                        //obtengo la condición.
                        codigo.Clear();
                        for (var j = t + 1; j < tokens.Count && tokens[j].Text != "else"; j++) codigo.Add(tokens[j]);

                        //copio la concición a mi lista de condiciones.
                        var aux = codigo.ToList();
                        condicion.Add(aux);
                        break;

                    case "else":
                        codigo.Clear();

                        for (var j = t + 1; j < tokens.Count; j++)
                            if (tokens[j].Text == "if")
                            {
                                haymas = true;
                                break;
                            }

                        if (haymas)
                        {
                            var temp = new List<Token>();
                            for (var j = t + 1; j < tokens.Count; j++) temp.Add(tokens[j]);
                            return ParsearCondicion(temp, funcion, condicion, codigo);
                        }
                        for (var j = t + 1; j < tokens.Count; j++) codigo.Add(tokens[j]);
                        funcion.Add(Parser.Evaluar(Desencriptar(codigo)));
                        return new Tuple<List<Funcion>, List<List<Token>>>(funcion,condicion);
                }
            }
           return null;
        }

        /// <summary>
        /// Este método evalua los comparadores.
        /// </summary>
        internal static bool EvaluarComparador(List<Token> codigo, double x)
        {
            var comparer = "";
            var left = new List<Token>();
            var right = new List<Token>();

            if (codigo.Any(t => t.Text == "|" || t.Text == "&")) return EvaluaCompadorBooleano(codigo, x);

            for (var i = 0; i < codigo.Count; i++)

                if (codigo[i].Type == TokenKind.Comparetor)
                {
                    comparer += codigo[i].Text;

                    if (i + 1 < codigo.Count && codigo[i+1].Type == TokenKind.Comparetor) comparer += codigo[i+1].Text;
                
                        for (var j = i - 1; j > -1; j--) left.Add(codigo[j]);

                        for (var j = i + 1; j < codigo.Count; j++)
                        {
                            if (codigo[j].Type == TokenKind.Comparetor) continue; right.Add(codigo[j]);
                        }
                        break; 
                }
            
            if (comparer == "") throw new Exception("No existe comparador.");
            
            return logicaldic[comparer].Invoke(Parser.Evaluar(Desencriptar(left)), Parser.Evaluar(Desencriptar(right))).Evaluate(x);
        }

        /// <summary>
        /// Este método evalua los comparadores booleanos.
        /// </summary>
        private static bool EvaluaCompadorBooleano(List<Token> codigo, double x)
        {
            var comparer = "";
            var left = new List<Token>();
            var right = new List<Token>();

            for (var i = 0; i < codigo.Count; i++)
                if (codigo[i].Text == "|" || codigo[i].Text == "&")
                {
                    comparer += codigo[i].Text;
                    if (i + 1 < codigo.Count && codigo[i + 1].Text == "|" || codigo[i].Text == "&")
                        comparer += codigo[i + 1].Text;

                    for (var j = i - 1; j > -1; j--) left.Add(codigo[j]);

                    for (var j = i + 1; j < codigo.Count; j++)
                    {
                        if (codigo[j].Text == "|" || codigo[i].Text == "&") continue;
                        right.Add(codigo[j]);
                    }
                    break; 
                }
                var izquierda = EvaluarComparador(left, x);
                var derecha = EvaluarComparador(right, x);
           
            return logoperator[comparer].Invoke(izquierda, derecha).Evaluate(izquierda, derecha);
        }

        /// <summary>
        /// Este método ejecuta las expresiones "print".
        /// </summary>
        private static void Imprimir(string name, List<Token> instrucciones)
        {
            //Si es una palabra.
            if (instrucciones[0].Type == TokenKind.Identifier && !variables.ContainsKey(instrucciones[0].Text) && !funciones.ContainsKey(instrucciones[0].Text))
            {
                var temp = instrucciones.Aggregate("", (current, t) => current + t.Text + " ");
                imprimir.Add(temp);
            }
            else if (variables.ContainsKey(name)) imprimir.Add(variables[name].ToString());

           //Si lo que mando a imprimir es una función copio lo que está en el medio de los paréntesis y lo mando a evaluar.
            else if (funciones.ContainsKey(name))
            {
                //Lista temporal, solo para copiar y luego mandar a evaluar.
                var temp = new List<Token>();

                for (var i = 1; i < instrucciones.Count; i++)
                    if (instrucciones[i].Text == "(")
                    {
                        for (var j = i + 1; j < instrucciones.Count && instrucciones[j].Text != ")"; j++)
                            temp.Add(instrucciones[j]);
                    }
                imprimir.Add(ParseaExpresion(temp).ToString());
            }
        }

        /// <summary>
        /// Este método me proporciona la instrucción sin el nombre de la variable (Después del símbolo "=").
        /// </summary>
        private static List<Token> GiveIntrution(List<Token> expresion)
        {
            var codigo = new List<Token>();

            for (var i = 0; i < expresion.Count; i++)
                if (expresion[i].Text=="=")
                {
                    for (var j = i + 1; j < expresion.Count; j++)
                        codigo.Add(expresion[j]);
                    break;
                }

            return codigo;
        }

        /// <summary>
        /// Este método parsea la expresión después de la variable.
        /// </summary>
        private static double ParseaExpresion(IEnumerable<Token> orden)
        {
            var temp = "";

           foreach (var t in orden)
           {
               if (t.Type != TokenKind.Separator)
               {
                   if (variables.ContainsKey(t.Text))
                   {
                       temp += variables[t.Text].ToString(); continue;
                   }
               }
               temp += t.Text;
           }

            temp = ArreglarEntrada(temp);
            return EvaluateTokens(Parser.ShuntingYard(reader.GetTokens(temp).ToArray()));
        }

        /// <summary>
        ///Este método es para arreglar la entrada con los signos "-" .
        /// <summary>
        private static string ArreglarEntrada(string code)
        {
            var fix = "";
            if (code[0] == '-') fix += "0-";
            for (var i = 0; i < code.Length; i++)
            {
                if (i == 0 && code[i] == '-') continue;
                if (code[i] == '-')
                {
                    if (code[i - 1] == '(') fix += "0-";
                    
                    else fix += "+(0-1)*";
                    
                    continue;
                }

                fix += code[i];
            }
            return fix;
        }

        /// <summary>
        /// Este método evalua la expresión.
        /// </summary>
        private static double EvaluateTokens(Queue<Token> tokens)
        {
            var result = new Stack<double>();
            double right, left;

            if (tokens.Count > 0)
            {
                foreach (var token in tokens)
                {
                    if (token.Type == TokenKind.Number)
                    {                        
                            result.Push(double.Parse(token.Text)); continue;
                    }
                    if (token.Type == TokenKind.Identifier && variables.ContainsKey(token.Text))
                    {
                        result.Push(variables[token.Text]); continue;
                    }

                    if (result.Count < 1) throw new InvalidOperationException("La entrada es incorrecta");

                    switch (token.Type)
                    {
                        case TokenKind.Symbol:

                            if (token.Text == "`")
                            {
                                right = result.Pop();
                                result.Push(0);
                                continue;
                            }
                            right = result.Pop();
                            left = result.Pop();
                            result.Push(Parser.library[token.Text].Invoke(new Constante(left), new Constante(right)).Evaluar(1));
                            break;

                        case TokenKind.Identifier:
                            right = result.Pop();
                            if (Parser.library.ContainsKey(token.Text))
                            {
                                result.Push(Parser.library[token.Text].Invoke().Evaluar(right));
                                continue;
                            }
                            if (funciones.ContainsKey(token.Text))
                            {
                                result.Push(funciones[token.Text].Evaluar(right)); continue;
                            }
                            result.Push(Parser.library[token.Text].Invoke(new Identidad()).Evaluar(right));
                            break;
                    }
                }
            }
            if (result.Count > 1) throw new Exception();

            return result.Pop();
        }

        /// <summary>
        /// Este método parsea la expresion despues de la variable.
        /// </summary>
        private static Funcion ParsearFuncion(IEnumerable<Token> tokens,string name)
        {
            var temp = Desencriptar(tokens.ToList());
            funcionestoken.Add(name,reader.GetTokens(temp).ToList());
            return Parser.Evaluar(temp);
        }

        /// <summary>
        /// Este método devuelve si existen condicionales en la instrucción.
        /// </summary>
        private static bool HayConditionals(IEnumerable<Token> tokens)
        {
            return tokens.Any(token => token.Text == "if");
        }

        /// <summary>
        /// Este método desencripta el array de token para poderlo evaluar.
        /// </summary>
        private static string Desencriptar(List<Token> lineaArreglada)
        {
                index = 0;
                var aparsear = "";

                if (!HayDeclarada(lineaArreglada)) return lineaArreglada.Aggregate(aparsear, (current, t) => current + t.Text);
                
            for (var i = 0; i < lineaArreglada.Count; i++)
                {
                    //si es un identificador y es distinto de x y no está en mi diccionario.
                    if (lineaArreglada[i].Type == TokenKind.Identifier && lineaArreglada[i].Text != "x" && !Parser.library.ContainsKey(lineaArreglada[i].Text))
                    {
                        //si el identificador es una variable ya declarado
                        if (variables.ContainsKey(lineaArreglada[i].Text))
                        {
                            aparsear += variables[lineaArreglada[i].Text].ToString();
                            continue;
                        }
                        //si el identificador es una función ya declarada
                        if (funciones.ContainsKey(lineaArreglada[i].Text))
                        {
                            var temp = Sustituir(i + 2, lineaArreglada);
                            for (var j = 0; j < funcionestoken[lineaArreglada[i].Text].Count; j++)
                            {
                                if (funcionestoken[lineaArreglada[i].Text][j].Text == "x") aparsear += temp;

                                else aparsear += funcionestoken[lineaArreglada[i].Text][j].Text;
                            }
                            i = index - 1;
                            continue;
                        }
                    }
                    aparsear += lineaArreglada[i].Text;
                }

                var x = reader.GetTokens(aparsear).ToList();
                lineaArreglada = x;
               return Desencriptar(x);
        }

        /// <summary>
        /// Este método es auxiliar para sustituir lo de adentro de los paréntesis.
        /// </summary>
        private static string Sustituir(int i, List<Token> lineaArreglada)
        {
            var count = 0;
            var temp = "";
            for (var j = i; j < lineaArreglada.Count; j++)
            {
                if (lineaArreglada[j].Text == ")" && count == 0)
                {
                    index = j + 1;
                    return temp;
                }
                switch (lineaArreglada[j].Text)
                {
                    case "(":
                        count++;
                        temp += lineaArreglada[j].Text;
                        continue;
                    case ")":
                        count--;
                        temp += lineaArreglada[j].Text;
                        continue;
                }
                temp += lineaArreglada[j].Text;

            }
            return temp;
        }

        /// <summary>
        /// Este método es para saber si tengo alguna funcion declarada anteriormente.
        /// </summary>
        private static bool HayDeclarada(List<Token> tokens)
        {
            return tokens.Any(t => funcionestoken.ContainsKey(t.Text) || variables.ContainsKey(t.Text));
        }
    }
}
