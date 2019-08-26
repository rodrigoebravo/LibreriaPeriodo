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
        public static Periodo MaxValue { get { return new Periodo(299912); } }
        public static Periodo Current { get { return new Periodo(DateTime.Now); } }
        private static Formato Format { get; set; }
        private enum Formato { yyyyMM, MMyyyy }
        public Periodo(int periodo) : this(periodo.ToString()) { }
        public Periodo(string periodo)
        {
            if (periodo.ToString().Length != 6)
                periodo = periodo.PadLeft(6, '0');
            if (Validar(periodo.ToString()))
            {
                int year;
                int month;
                if (Format == Formato.yyyyMM)
                {
                    year = int.Parse(periodo.ToString().Substring(0, 4));
                    month = int.Parse(periodo.ToString().Substring(4, 2));
                    this.Value = int.Parse($"{year}{month}");
                    this.Date = new DateTime(year, month, 1);
                }
                else
                {
                    year = int.Parse(periodo.ToString().Substring(2, 4));
                    month = int.Parse(periodo.ToString().Substring(0, 2));

                }
                this.Value = int.Parse($"{year}{month.ToString().PadLeft(2, '0')}");
                this.Date = new DateTime(year, month, 1);
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
        public Periodo(int year, int month) : this($"{year}{month.ToString().PadLeft(2, '0')}")
        {

        }
        private static bool Validar(string periodo)
        {
            for (int i = 1900; i < 3001; i++)
            {
                for (int j = 1; j < 13; j++)
                {
                    var peryyyyMM = $"{i}{j.ToString().PadLeft(2, '0')}";
                    var perMMyyyy = $"{j.ToString().PadLeft(2, '0')}{i}";
                    if (peryyyyMM == periodo)
                    {
                        Format = Formato.yyyyMM;
                        return true;
                    }
                    if (perMMyyyy == periodo)
                    {
                        Format = Formato.MMyyyy;
                        return true;
                    }
                }
            }
            return false;
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

        public static int operator -(Periodo periodoA, Periodo periodoB)
        {
            var anioA = periodoA.Year;
            var mesA = periodoA.Month;
            var anioB = periodoB.Year;
            var mesB = periodoB.Month;

            var resultAnio = anioA - anioB;
            var resultMes = mesA - mesB;

            resultMes += resultAnio * 12;

            return resultMes;
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
        public static bool operator ==(Periodo periodoA, DateTime periodoB)
        {
            var perAux = new Periodo(periodoB);
            if (!periodoA.IsValid || !perAux.IsValid)
                throw new Exception("Alguno de los dos periodos es inválido");
            if (periodoA.Value == perAux.Value)
                return true;
            return false;
        }
        public static bool operator !=(Periodo periodoA, DateTime periodoB)
        {
            return !(periodoA == periodoB);
        }
        public static bool operator ==(Periodo periodoA, int periodoB)
        {
            var perAux = new Periodo(periodoB);
            if (!periodoA.IsValid || !perAux.IsValid)
                throw new Exception("Alguno de los dos periodos es inválido");
            if (periodoA.Value == perAux.Value)
                return true;
            return false;
        }
        public static bool operator !=(Periodo periodoA, int periodoB)
        {
            return !(periodoA == periodoB);
        }
        public static bool operator ==(Periodo periodoA, string periodoB)
        {
            var perAux = new Periodo(periodoB);
            if (!periodoA.IsValid || !perAux.IsValid)
                throw new Exception("Alguno de los dos periodos es inválido");
            if (periodoA.Value == perAux.Value)
                return true;
            return false;
        }
        public static bool operator !=(Periodo periodoA, string periodoB)
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

            if (format.Equals("MMyyyy"))
                return $"{this.Month.ToString().PadLeft(2, '0')}{this.Year}";

            if (format.Equals("yyyyMM"))
                return $"{this.Year}{this.Month.ToString().PadLeft(2, '0')}";

            if (!format.Contains('.') && !format.Contains('-') && !format.Contains('/') && !format.Contains(' '))
                return this.Value.ToString();

            char simbolo = char.MinValue;
            for (int i = 0; i < format.Length; i++)
            {

                if (format[i].Equals('#'))
                {
                    cont++;
                }
                else if (format[i].Equals('.') || format[i].Equals('-') || format[i].Equals('/') || format[i].Equals(' '))
                {
                    simbolo = format[i];
                    break;
                }
            }

            if (cont == 2)
                return $"{this.Month.ToString().PadLeft(2, '0')}{simbolo}{this.Year}";

            if (cont == 4)
                return $"{this.Year}{simbolo}{this.Month.ToString().PadLeft(2, '0')}";

            return this.Value.ToString();
        }
    }
}