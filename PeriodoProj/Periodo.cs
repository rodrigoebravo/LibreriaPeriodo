using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodoProj
{
    public class Periodo
    {
        public int? Value { get; private set; }
        public bool IsValid { get; private set; }
        public DateTime? Date { get; private set; }
        public int Year { get { if (this.IsValid) return this.Date.Value.Year; return 0; } }
        public int Month { get { if (this.IsValid) return this.Date.Value.Month; return 0; } }
        public static Periodo MinValue { get { return new Periodo(190001); } }
        public static Periodo MaxValue { get { return new Periodo(DateTime.MaxValue); } }
        public static Periodo Current { get { return new Periodo(DateTime.Now); } }

        public Periodo(int periodo) : this(periodo.ToString()) { }
        public Periodo(string periodo)
        {
            if (Validar(periodo.ToString()))
            {
                this.Value = int.Parse(periodo);
                this.Date = new DateTime(int.Parse(periodo.ToString().Substring(0, 4)), int.Parse(periodo.ToString().Substring(4, 2)), 1);
                this.IsValid = true;
                return;
            }
            this.Value = null;
            this.Date = null;
            this.IsValid = false;
        }
        public Periodo(DateTime periodo) : this(TraerPeriodo(periodo))
        {

        }
        private static bool Validar(string periodo)
        {
            if (periodo.ToString().Length != 6)
                return false;

            var valorAnio = int.Parse(periodo.Substring(0, 4));
            var valorMes = int.Parse(periodo.Substring(4, 2));

            if (valorAnio < 1900)
                return false;

            if (valorMes < 1 || valorMes > 12)
                return false;

            return true;
        }
        private static string TraerPeriodo(DateTime periodo)
        {
            if (periodo != null)
                return $"{periodo.Year.ToString().PadLeft(4, '0')}{periodo.Month.ToString().PadLeft(2, '0')}";
            return string.Empty;
        }

        public static explicit operator Periodo(DateTime fecha)
        {
            var valor = ($"{fecha.Year}{fecha.Month}");
            return new Periodo(valor);
        }
        public static Periodo operator +(Periodo periodo, int meses)
        {
            if (!periodo.IsValid)
                throw new Exception("Periodo inválido");

            var anio = periodo.Date.Value.Year;
            var mes = periodo.Date.Value.Month;

            mes += meses;
            while (mes > 12)
            {
                anio++;
                mes -= 12;
            }
            while (mes < 1)
            {
                anio--;
                mes += 12;
            }
            var resAnio = anio.ToString();
            var resMes = mes.ToString().PadLeft(2, '0');
            return new Periodo($"{resAnio}{resMes}");
        }
        public static Periodo operator -(Periodo periodo, int meses)
        {
            if (!periodo.IsValid)
                throw new Exception("Periodo inválido");

            return periodo + (meses * -1);
        }
        public static bool operator ==(Periodo periodoA, Periodo periodoB)
        {
            if (!periodoA.IsValid || !periodoB.IsValid)
                throw new Exception("Alguno de los dos periodos es inválido");
            if (periodoA.Value == periodoB.Value)
                return true;
            return false;
        }
        public static bool operator !=(Periodo periodoA, Periodo periodoB)
        {
            return !(periodoA == periodoB);
        }
        public static bool operator >(Periodo periodoA, Periodo periodoB)
        {
            return periodoA.Date > periodoB.Date;
        }
        public static bool operator <(Periodo periodoA, Periodo periodoB)
        {
            return periodoA.Date < periodoB.Date;
        }
        public static bool operator >=(Periodo periodoA, Periodo periodoB)
        {
            return periodoA.Date >= periodoB.Date;
        }
        public static bool operator <=(Periodo periodoA, Periodo periodoB)
        {
            return periodoA.Date <= periodoB.Date;
        }
        public override string ToString()
        {
            if (this.IsValid)
                return this.Value.ToString();
            return string.Empty;
        }
        public string ToString(string format)
        {
            if (!this.IsValid)
                return string.Empty;
            int cont = 0;

            if (!format.Contains('.') || !format.Contains('-') || !format.Contains('/'))
                return this.Value.ToString();

            for (int i = 0; i < format.Length; i++)
            {

                if (format[i].Equals('#'))
                    cont++;
                else if (format[i].Equals('.') || format[i].Equals('-') || format[i].Equals('/'))
                    break;
            }
            if (cont == 2)
                return $"{this.Value.ToString().Substring(4, 2)}.{this.Value.ToString().Substring(0, 4)}";

            if (cont == 4)
                return $"{this.Value.ToString().Substring(0, 4)}.{this.Value.ToString().Substring(4, 2)}";

            return this.Value.ToString();
        }

    }
}