using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResolucionParcial
{
    class Program
    {
        static void Main(string[] args)
        {
            Comun tur1 = new Comun { Dni = 223456, Apellido = "Martinez", Nombres = "Jose", FechaNac = Convert.ToDateTime("11/05/1980"), Nacionalidad = "Argentino" };
            Estudiante est1 = new Estudiante { Dni=36098765,Apellido="Gomez", Nombres="Ariel", FechaNac=Convert.ToDateTime("01/01,2000")};
            Jubilado jub1 = new Jubilado { Dni=9890123,Apellido="Ruiz",Nombres="Angela",FechaNac=Convert.ToDateTime("01/06/1949")};
            Comision comision1 = new Comision(new Paquete { RegionComision = "Cuyo", ImportePaquete = 20000 });
            comision1.AddTurista(tur1);
            comision1.AddTurista(est1);
            comision1.AddTurista(jub1);

            foreach (Turista t in comision1)
            {
                Console.WriteLine(String.Format("{0} - {1} - {2}",t.Apellido,t.Dni,comision1.PaqueteComision.ImportePaquete - (t.ImporteDescuento *  comision1.PaqueteComision.ImportePaquete) ));
            }
            comision1.Sort(SortTipoTurista.OrdenApellido);
            foreach (Turista t in comision1)
            {
                Console.WriteLine(t.ToString());
            }
            comision1.Sort(SortTipoTurista.OrdenTipo);
            foreach (Turista t in comision1)
            {
                Console.WriteLine(t.ToString());
            }
            Console.ReadKey();
        }
    }
    public enum SortTipoTurista { OrdenApellido = 1, OrdenTipo = 2 }
    public delegate int ComparaTurista(Turista a, Turista b);
    public abstract class Turista:IComparable
    {
        private static ComparaTurista compara = Turista.ComparaXApellido;
        protected DateTime _fecha_nac;
        public int Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public string Nacionalidad { get; set; }
        public string Telefono { get; set; }
        public string Region { get; set; }
        public abstract double ImporteDescuento { get; }
        public double ImporteAPagar { get; set; }
        public DateTime FechaNac { get { return _fecha_nac; } set { _fecha_nac = value; } }
        public int Edad { get { return DateTime.Now.Year - this._fecha_nac.Year; } }
        public static int ComparaXApellido(Turista a, Turista b)
        {
            return a.Apellido.CompareTo(b.Apellido);
        }
        public static int ComparaXTipo(Turista a, Turista b)
        {
            return a.GetType().ToString().CompareTo(b.GetType().ToString());
        }
        public int CompareTo(object obj)
        {
            return compara(this,(obj as Turista));
        }
        public static void SetComparacion(SortTipoTurista newCompara)
        {
            if(newCompara == SortTipoTurista.OrdenApellido)
                compara = ComparaXApellido;
            if (newCompara == SortTipoTurista.OrdenTipo)
                compara = ComparaXTipo;
        }
    }
    public class Comun : Turista
    {
        public override double ImporteDescuento
        {
            get { return 0; }
        }
    }

    public class Estudiante : Turista
    {
        public override double ImporteDescuento
        {
            get { return 0.2; }
        }      
    }

    public class Jubilado : Turista
    {
        public override double ImporteDescuento
        {
            get { return 0.3; }
        }       
    }
    public class Paquete
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string RegionComision { get; set; }
        public double ImportePaquete { get; set; }
        List<string> lista_eventos;
        public Paquete()
        {
            lista_eventos = new List<string>();
        }
        public void addEVento(string evento)
        {
            lista_eventos.Add(evento);
        }
    }
    public class Comision: IEnumerable, IEnumerator 
    {
        List<Turista> turistas = null;
        const int Max_Cupo = 30;
        private int _cupo;
        private Paquete _paquete;
        private int indice = -1;
        public Comision(Paquete paq)
        {
            turistas = new List<Turista>();
            iniciarComision(paq);
        }
        public Paquete PaqueteComision{get{return _paquete;}}
        public void Sort(SortTipoTurista tipoOrden)
        {
            Turista.SetComparacion(tipoOrden);
            turistas.Sort();
        }
        public int Cupo
        {
            get { return _cupo; }
        }
        private void iniciarComision(Paquete pq)
        {
            _paquete = pq;
            _cupo = Max_Cupo;
        }
        public bool AddTurista(Turista t)
        {
            if (this._cupo == 0)
                return false;
            else
            {
                turistas.Add(t);
                _cupo--;
            }
            return true;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public object Current
        {
            get { return turistas[indice]; }
        }

        public bool MoveNext()
        {
            if (indice == turistas.Count - 1)
            {
                indice = -1;
                return false;
            }
            indice++;
            return true;
        }

        public void Reset()
        {
           indice=-1;
        }
    }

}
