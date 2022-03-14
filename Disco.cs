using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PlanificacionDisco
{
    public class Disco
    {
        int num_tracks { get; set; }
        public int cabeza { get; set; }
        Random rand = new Random();
        readonly object candado = new object();
        int metodoPlan { get; set; }
        ConcurrentQueue<int> cola_Solicitud = new ConcurrentQueue<int>();
        List<int> cola_Planificacion = new List<int>();
        int [] colaAux;
        

        public Disco(int tracks, int metodo, int cab)
        {
            num_tracks = tracks;
            metodoPlan = metodo;
            cabeza = cab;
        }

        public void RealizarScan()
        {
            List<int> cola = new List<int>();
            int i = 0;

            cola_Planificacion.Add(cabeza);
            cola_Planificacion.Add(num_tracks);
            cola_Planificacion.Add(0);
            cola_Planificacion.Sort();

            int tam = cola_Planificacion.Count();

            for (i = 0; i < tam; i++)
            {
                if (cabeza == cola_Planificacion[i])
                {
                    break;
                }
            }
            int k = i;
            if (k<num_tracks/2)
            {
                for (i = k; i < tam; i++)
                {
                    cola.Add(cola_Planificacion[i]);
                }
                for (i = k-1; i >= 0; i--)
                {
                    cola.Add(cola_Planificacion[i]);
                }
            }
            else
            {
                for (i = k; i >= 0; i--)
                {
                    cola.Add(cola_Planificacion[i]);
                }
                for (i = k + 1; i < tam; i++)
                {
                    cola.Add(cola_Planificacion[i]);
                }
            }
            Console.Write("[ ");
            foreach (var elem in cola_Planificacion)
            {
                Console.Write(elem + " ");
            }
            Console.WriteLine("]");

        }

        public void RealizarSSTF()
        {
            int aux;
            int tam = cola_Planificacion.Count;
            colaAux = new int [tam];
            llenarAux();
            
            for (int i = 0; i < tam; i++)
            {
                for (int j = i+1; j < tam; j++)
                {
                    if (colaAux[i] > colaAux[j])
                    {
                        aux = colaAux[i];
                        colaAux[i] = colaAux[j];
                        colaAux[j] = aux;

                        aux = cola_Planificacion[i];
                        cola_Planificacion[i] = cola_Planificacion[j];
                        cola_Planificacion[j] = aux;
                        //llenarAux();
                    }
                }
            }
            //printAuxYPLan();
        }

        
        public void LlenarColaSoli() 
        {
            while (true)
            {
                int random = rand.Next(1, num_tracks + 1);
                lock (candado)
                {
                    cola_Solicitud.Enqueue(random);
                    printSoli();
                }
                Thread.Sleep(1000);
            }
        }
        public void PlanificarCola()
        {
            switch (metodoPlan)
            {
                case 1:
                    while (true)
                    {
                        lock (candado)
                        {
                            Console.Write("Planificación FIFO: ");
                            cola_Planificacion = cola_Solicitud.ToList();
                            printPlan();
                        }
                        Thread.Sleep(1000);
                    }
                    break;
                case 2:
                    while (true)
                    {
                        lock (candado)
                        {
                            Console.Write("Planificación SSTF: ");
                            cola_Planificacion = cola_Solicitud.ToList();
                            RealizarSSTF();
                            printPlan();
                        }
                        Thread.Sleep(1000);
                    }
                    break;
                case 3:
                    while (true)
                    {
                        lock (candado)
                        {
                            Console.Write("Planificación SCAN: ");
                            cola_Planificacion = cola_Solicitud.ToList();
                            RealizarScan();
                            //printPlan();
                        }
                        Thread.Sleep(1000);
                    }
                    break;
                default:
                    break;
            }
        }
        public void IniciarHilos()
        {
            Thread thread = new Thread(new ThreadStart(LlenarColaSoli));
            thread.Start();

            Thread threadPlan = new Thread(new ThreadStart(PlanificarCola));
            threadPlan.Start();

            thread.Join();
            threadPlan.Join();
        }

        private void printSoli()
        {
            Console.Write("Cola de solicitudes: ");
            Console.Write("[ ");
            foreach (var elem in cola_Solicitud)
            {
                Console.Write(elem + " ");
            }
            Console.WriteLine("]");
        }

        private void printPlan()
        {
            Console.Write("[ ");
            foreach (var elem in cola_Planificacion)
            {
                Console.Write(elem + " ");
            }
            Console.WriteLine("]");
        }

        private void llenarAux()
        {
            int tam = cola_Planificacion.Count;
            for (int i = 0; i < tam; i++)
            {
                var check = Math.Abs(cabeza - cola_Planificacion[i]);
                colaAux[i] = check;
            }
        }
        private void printAuxYPLan()
        {
            int tam = cola_Planificacion.Count;
            Console.Write("Cola Aux: ");
            Console.Write("[ ");
            for (int i = 0; i < tam; i++)
            {

                Console.Write(colaAux[i] + " ");

            }
            Console.WriteLine("]");
            Console.Write("Cola Planificación");
            Console.Write("[ ");
            for (int i = 0; i < tam; i++)
            {
                Console.Write(cola_Planificacion[i] + " ");

            }
            Console.WriteLine("]");
        }
    }
}
