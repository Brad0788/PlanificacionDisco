using System;

namespace PlanificacionDisco
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Simulación de Disco:");
            Console.Write("Ingresa el número de tracks: ");
            int num_tracks = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ingresa la posición inicial de la cabeza: ");
            int cabeza = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Algoritmos disponibles:");
            Console.WriteLine("1. Planificación FIFO\n2. Planificación SSTF\n3. Planificación SCAN");
            Console.WriteLine("Que algoritmo de planficación desea simular?");
            int plan = Convert.ToInt32(Console.ReadLine());
            Disco disco = new Disco(num_tracks, plan, cabeza);
            disco.IniciarHilos();

            Console.WriteLine("Termina");
        }
    }

}