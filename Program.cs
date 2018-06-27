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
        static int[] peso;
        static int[] ganancia;
        static int capacidad_mochila = 0;
        static int objetos=0;
        static int mochilas=0;
        static int generaciones=0;
        static double pm = 0;
        static double pc = 0;

        String[] binPadres = new String[mochilas];
        String[] binHijos = new String[mochilas];
        
        static void Main(string[] args)
        {
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

            Console.WriteLine("Probabilidad de cruce: " + pc);

            Console.Write("Escribe la probabilidad de mutación: ");
            while (pm < 0.001 || pm > 0.01)
            {
                pm = Convert.ToDouble(Console.ReadLine());
                if (pm < 0.001 || pm > 0.01)
                {
                    Console.WriteLine("El valor debe encontrarse entre 0.001 y 0.01.");
                }
            }

            Console.WriteLine("Probabilidad de mutación: " + pm);

            Console.Write("Escribe el número de generaciones a evaluar: ");
            while (generaciones <= 0)
            {
                generaciones = Convert.ToInt32(Console.ReadLine());
            }

            Console.Write("Pesos de los objetos: ");

            for (int i=0; i<objetos; i++)
            {
                Console.Write(peso[i] +", ");
            }

            Console.Write("Ganancias de los objetos: ");

            for (int i = 0; i < objetos; i++)
            {
                Console.Write(ganancia[i] + ", ");
            }

            Console.ReadKey();

        }

        public static void load_data()
        {

            string rutaListPesos = "../Datos/weight.txt";
            string rutaListBen = "../Datos/profits.txt";

            //string rutaListPesos = "C:/Users/vladi/Documents/txt/p07/pesos.txt";
            //string rutaListBen = "C:/Users/vladi/Documents/txt/p07/beneficios.txt";

            // string rutaListPesos = "C:/Users/vladi/Documents/txt/p08/pesos.txt";
            // string rutaListBen = "C:/Users/vladi/Documents/txt/p08/beneficios.txt";

            try
            {

                StreamReader sr = new StreamReader(rutaListPesos);
                StreamReader sr_ben = new StreamReader(rutaListBen);

                String linea;


                List<int> arrayListPesos = new List<int>();
                List<int> arrayListBeneficio = new List<int>();

                using (StreamReader reader = System.IO.File.OpenText(rutaListPesos))
                {

                    while ((linea = reader.ReadLine()) != null)
                    {
                        arrayListPesos.Add(Convert.ToInt32(linea));
                    }
                }

                using (StreamReader reader_ben = System.IO.File.OpenText(rutaListBen))
                {

                    while ((linea = reader_ben.ReadLine()) != null)
                    {
                        arrayListBeneficio.Add(Convert.ToInt32(linea));
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
