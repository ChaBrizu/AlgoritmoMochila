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
        static int[] peso;
        static int[] ganancia;
        static int capacidad_mochila=0;
        static int objetos=0;
        static int mochilas=0;

        //Variables del algoritmo genético
        static double pm=0;
        static double pc=0;
        
        public String[] binPadres = new String[' '];
        public String[] binHijos = new String[' '];

        int[] xPeso = new int[' '];
        int[] fxGanancia = new int[' '];
        double suma = 0.0;
        double sumfnorm = 0.0;

        double[] mejoresResultadosGen = new double[' '];
        double[] mejoresResultadosGlobal = new double[' '];

        static int generaciones = 0;

        static void Main(string[] args)
        {
            Program obj = new Program();
            load_data();            

            foreach (int value in peso)
            {
                objetos++;
            }

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

            Console.Write("\nPesos de los objetos: ");

            for (int i=0; i<objetos; i++)
            {
                Console.Write(peso[i] +", ");
            }

            Console.Write("\nGanancias de los objetos: ");

            for (int i = 0; i < objetos; i++)
            {
                Console.Write(ganancia[i] + ", ");
            }

            Console.WriteLine("Capacidad de la mochila: " + capacidad_mochila);
            
            obj.generar_individuos();
            obj.calculoMochila();

            for(int auxGen = 0; auxGen < generaciones; auxGen++)
            {
                for(int i = 0; i < mochilas; i++)
                {
                    if (obj.xPeso[i] > capacidad_mochila)
                    {
                        Console.WriteLine("Mochila " + (i + 1) + " sobrepasa el peso máximo");
                        String auxBin = obj.binPadres[i];
                        obj.binPadres[i] = obj.reparacion(auxBin, i);
                    }                    
                }                
            }

            Console.ReadKey();
        }

        public void generar_individuos()
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
                binPadres[i] = binario;
                Console.WriteLine("Individuo " + (i + 1) + ": " + binPadres[i]);
            }            
        }

        public void calculoMochila()
        {
            char[] individuo = new char[' '];
            for (int i = 0; i < mochilas; i++)
            {
                individuo = binPadres[i].ToCharArray();
                for (int j = 0; j < individuo.Length; j++)
                {
                    if (individuo[j] == '1')
                    {
                        xPeso[i] = xPeso[i] + peso[j];
                        //fxGanancia[i] = fxGanancia[i] + ganancia[j];
                    }
                }
                Console.WriteLine("Peso de la mochila " + (i + 1) + ": " + xPeso[i]);
            }
        }

        public String reparacion(String auxBin, int i)
        {
            Console.WriteLine("ENTRA A REPARACIÓN");
            String x = auxBin;
            Boolean mochila_llena = false;

            Random rand = new Random();            

            int auxR;
            int auxPeso = xPeso[i];

            if (auxPeso > capacidad_mochila)
            {
                mochila_llena = true;
                Console.WriteLine("ENTRA A IF EN"+ mochila_llena);
            }

            char[] reparador = x.ToCharArray();

            do
            {
                Console.WriteLine("ENTRA A WHILE EN" + mochila_llena);

                auxR = rand.Next(auxBin.Length);
                if (reparador[auxR] == '1')
                {
                    Console.WriteLine("ENTRA A PRIMER IF DEL WHILE EN" + reparador[auxR]);
                    reparador[auxR] = '0';
                    Console.WriteLine("Y SALE DE PRIMER IF DEL WHILE EN" + reparador[auxR]);
                }
                for (int j = 0; j < reparador.Length; j++)
                {
                    if (reparador[j] == '1')
                    {
                        auxPeso = auxPeso + peso[j];
                    }
                }
                if (auxPeso <= capacidad_mochila)
                {
                    mochila_llena = false;
                    Console.WriteLine("NUEVO individuo " + (i + 1) + ": " + x);
                }
                x = reparador.ToString();
            } while (mochila_llena);
            xPeso[i] = auxPeso;
            return x;
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
