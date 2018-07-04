using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AlgoritmoMochila
{
    class Program
    {   
        //variables del problema de la mochila
        static int[] peso=new int[objetos];
        static int[] ganancia = new int[objetos];
        static int capacidad_mochila=0;
        static int objetos=0;
        static int mochilas=0;

        //Variables del algoritmo genético
        static double pm=0;
        static double pc=0;

        Double[] acumulado = new Double[mochilas];

        public String[] binPadres = new String[mochilas];
        public String[] binHijos = new String[mochilas];

        int[] xPeso = new int[mochilas];
        int[] fxGanancia = new int[mochilas];

        //Resultados
        int[] mejoresPesosGen = new int[generaciones];
        int[] mejoresGananciasGen = new int[generaciones];
        String[] mejoresBin = new String[generaciones];

        static int generaciones = 0;

        static void Main(string[] args)
        {            
            load_data();            

            foreach (int value in peso)
            {
                objetos++;
            }

            Console.Write("Pesos de los objetos: ");

            for (int i = 0; i < objetos; i++)
            {
                Console.Write(peso[i] + ", ");
            }

            Console.Write("\nGanancias de los objetos: ");

            for (int i = 0; i < objetos; i++)
            {
                Console.Write(ganancia[i] + ", ");
            }

            Console.WriteLine("\nCapacidad de la mochila: " + capacidad_mochila);

            Console.Write("Escribe la cantidad de individuos (mochilas) a evaluar: ");
            while (mochilas <= 0 || mochilas % 2 != 0)
            {
                mochilas = Convert.ToInt32(Console.ReadLine());
                if(mochilas <= 0 || mochilas % 2 != 0)
                {
                    Console.WriteLine("El valor debe ser mayor a 0 y par.");
                }
                
            }

            Console.Write("Escribe la probabilidad de cruce: ");
            while (pc < 0.65 || pc > 0.8)
            {
                pc = Convert.ToDouble(Console.ReadLine());
                if (pc < 0.65 || pc > 0.8)
                {
                    Console.WriteLine("El valor debe encontrarse entre 0.65 y 0.8.");
                }
            }            

            Console.Write("Escribe la probabilidad de mutación: ");
            while (pm < 0.001 || pm > 0.01)
            {
                pm = Convert.ToDouble(Console.ReadLine());
                if (pm < 0.001 || pm > 0.01)
                {
                    Console.WriteLine("El valor debe encontrarse entre 0.001 y 0.01.");
                }
            }


            Console.Write("Escribe el número de generaciones a evaluar: ");
            while (generaciones <= 0)
            {
                generaciones = Convert.ToInt32(Console.ReadLine());
            }

            Program obj = new Program();

            obj.generar_individuos(obj);
            obj.calculoMochila(obj);

            for (int i = 0; i < mochilas; i++)
            {
                if (obj.xPeso[i] > capacidad_mochila)
                {
                    String auxBin = obj.binPadres[i];
                    obj.reparacion(auxBin, i, obj);
                }
            }

            for (int auxGen = 0; auxGen < generaciones; auxGen++)
            {
                obj.algoritmo_Genetico(obj);
                obj.calculoMochila(obj);

                for (int i = 0; i < mochilas; i++)
                {
                    if (obj.xPeso[i] > capacidad_mochila)
                    {
                        String auxBin = obj.binPadres[i];
                        obj.reparacion(auxBin, i, obj);
                    }
                }

                int auxGanancia = 0;
                int mejorGan = 0;
                int mejorPeso = 0;
                String mejorBin = "";

                for (int i = 0; i < mochilas; i++)
                {
                    for (int j = mochilas - 1; j >= i; j--)
                    {
                        auxGanancia = obj.fxGanancia[i];
                        if (auxGanancia > mejorGan)
                        {
                            mejorGan = auxGanancia;
                            mejorPeso = obj.xPeso[i];
                            mejorBin = obj.binPadres[i];
                        }
                    }
                }          
        
                obj.mejoresPesosGen[auxGen] = mejorPeso;
                obj.mejoresGananciasGen[auxGen] = mejorGan;
                obj.mejoresBin[auxGen] = mejorBin;
            }

            int auxGananciaGen = 0;
            int mejorGanGeneracion = 0;
            int mejorPesoGen = 0;
            String mejorBinGen = "";

            for (int i = 0; i < generaciones; i++)
            {
                for (int j = generaciones - 1; j >= i; j--)
                {
                    auxGananciaGen = obj.mejoresGananciasGen[i];
                    if (auxGananciaGen > mejorGanGeneracion)
                    {
                        mejorGanGeneracion = auxGananciaGen;
                        mejorPesoGen = obj.mejoresPesosGen[i];
                        mejorBinGen = obj.mejoresBin[i];
                    }
                }
            }

            Console.WriteLine("Carga óptima con un peso de " + mejorPesoGen.ToString() + ", con una ganancia de " + mejorGanGeneracion.ToString()+ ", \nsiendo su cadena binaria " + mejorBinGen);
            Console.ReadKey();            
        }

        public void generar_individuos(Program obj)
        {
            Random r = new Random();
            for(int i=0; i < mochilas; i++)
            {
                String binario = "";
                for (int j = 0; j < objetos; j++)
                {
                    int rand = r.Next(0, 2);
                    binario = binario + Convert.ToString(rand);
                }
                obj.binPadres[i] = binario;
            }            
        }

        public void calculoMochila(Program obj)
        {
            char[] individuo = new char[' '];
            for (int i = 0; i < mochilas; i++)
            {
                obj.xPeso[i] = 0;
                obj.fxGanancia[i] = 0;

                individuo = obj.binPadres[i].ToCharArray();
                for (int j = 0; j < individuo.Length; j++)
                {
                    if (individuo[j] == '1')
                    {
                        obj.xPeso[i] = obj.xPeso[i] + peso[j];
                        obj.fxGanancia[i] = obj.fxGanancia[i] + ganancia[j];
                    }                    
                }
            }
        }

        public void reparacion(String auxBin, int i, Program obj)
        {
            Boolean mochila_llena = true;
            char[] x = auxBin.ToArray();

            Random rand = new Random();

            int auxR;
            int auxPeso = obj.xPeso[i];
            int auxGanancia = obj.fxGanancia[i];

            while(mochila_llena)
            {
                auxR = rand.Next(0, x.Length);

                if (x[auxR] == '1')
                {
                    x[auxR] = '0';
                    auxPeso = auxPeso - peso[auxR];
                    auxGanancia = auxGanancia - ganancia[auxR];
                }

                if (auxPeso <= capacidad_mochila)
                {
                    mochila_llena = false;
                    obj.xPeso[i] = auxPeso;
                    obj.fxGanancia[i] = auxGanancia;
                }
            }

            String xmod = new string(x);
            obj.binPadres[i] = xmod;
        }

        public void algoritmo_Genetico(Program obj)
        {
            double suma = 0.0;
            double sumfnorm = 0.0;

            Double[] fnorm = new Double[mochilas];
            Double[] acumulado = new Double[mochilas];
            String[] newpadres = new String[mochilas];

            for (int i = 0; i < mochilas; i++)
            {
                suma += obj.fxGanancia[i];
            }

            for (int i = 0; i < mochilas; i++)
            {
                fnorm[i] = obj.fxGanancia[i] / suma;
                sumfnorm += fnorm[i];
                acumulado[i] = sumfnorm;
            }

            Random aleatorio = new Random();

            Double[] vector = new Double[mochilas];

            for (int i = 0; i < mochilas; i++)
            {
                vector[i] = aleatorio.NextDouble();
            }

            for (int i = 0; i < mochilas; i++)
            {
                for (int j = 0; j < mochilas; j++)
                {
                    if (acumulado[j] > vector[i])
                    {
                        newpadres[i] = obj.binPadres[j];
                        break;
                    }
                }
            }

            int corte1, corte2;

            Random random = new Random();
            Random cruce = new Random();

            //Cruce
            for (int i = 0; i < mochilas; i = i + 2)
            {
                if (cruce.NextDouble() <= pc)
                {
                    do
                    {
                        corte1 = random.Next(1, objetos);
                        corte2 = random.Next(1, objetos);
                    } while (corte1 == corte2);
                    
                    if (corte1 < corte2)
                    {
                        obj.binHijos[i] = newpadres[i].Substring(0, corte1) + newpadres[i + 1].Substring(corte1, corte2 - corte1) + newpadres[i].Substring(corte2, objetos - corte2);
                        obj.binHijos[i + 1] = newpadres[i + 1].Substring(0, corte1) + newpadres[i].Substring(corte1, corte2 - corte1) + newpadres[i + 1].Substring(corte2, objetos - corte2);
                    }

                    else
                    {
                        obj.binHijos[i] = newpadres[i + 1].Substring(0, corte2) + newpadres[i].Substring(corte2, corte1 - corte2) + newpadres[i + 1].Substring(corte1, objetos - corte1);
                        obj.binHijos[i + 1] = newpadres[i].Substring(0, corte2) + newpadres[i + 1].Substring(corte2, corte1 - corte2) + newpadres[i].Substring(corte1, objetos - corte1);
                    }                    
                }
                else
                {
                    obj.binHijos[i] = newpadres[i];
                    obj.binHijos[i + 1] = newpadres[i + 1];
                }
            }
            //Fin cruce

            //Mutación
            char[] mutacion = new char[' '];
            for (int i = 0; i < mochilas; i++)
            {
                mutacion = obj.binHijos[i].ToCharArray();//Separa los strings en caracteres
                for (int j = 0; j < mutacion.Length; j++)
                {
                    if (random.NextDouble() < pm)
                    {
                        if (mutacion[j] == '1')
                        {
                            mutacion[j] = '0';
                        }
                        if (mutacion[j] == '0')
                        {
                            mutacion[j] = '1';
                        }
                    }
                }
                obj.binHijos[i] = new string(mutacion);                
            }
            //Fin mutación

            //Asigna el valor de los hijos a padres
            for(int i=0; i < mochilas; i++)
            {
                obj.binPadres[i] = obj.binHijos[i];
            }
        }

        public static void load_data()
        {

            string rutaListPesos = "../Datos/weight.txt";
            string rutaListBen = "../Datos/profits.txt";
            string rutaCapacidad = "../Datos/capacity.txt";

            try
            {

                StreamReader sr = new StreamReader(rutaListPesos);
                StreamReader sr_ben = new StreamReader(rutaListBen);
                StreamReader sr_cap = new StreamReader(rutaCapacidad);

                String linea;


                List<int> arrayListPesos = new List<int>();
                List<int> arrayListBeneficio = new List<int>();

                using (StreamReader reader = File.OpenText(rutaListPesos))
                {

                    while ((linea = reader.ReadLine()) != null)
                    {
                        arrayListPesos.Add(Convert.ToInt32(linea));
                    }
                }

                using (StreamReader reader_ben =File.OpenText(rutaListBen))
                {
                    while ((linea = reader_ben.ReadLine()) != null)
                    {
                        arrayListBeneficio.Add(Convert.ToInt32(linea));
                    }
                }

                using (StreamReader reader_cap = File.OpenText(rutaCapacidad))
                {
                    while ((linea = reader_cap.ReadLine()) != null)
                    {
                        capacidad_mochila = Convert.ToInt32(linea);
                    }                    
                }

                peso = arrayListPesos.ToArray();
                ganancia = arrayListBeneficio.ToArray();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
