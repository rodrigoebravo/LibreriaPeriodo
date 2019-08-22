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

        /// <summary>
        /// Valida el periodo del parametro
        /// </summary>
        /// <returns></returns>
        private static bool Validar(string valorPeriodo)
        {
            if (valorPeriodo.ToString().Length != 6)
                return false;

            var valorAnio = int.Parse(valorPeriodo.Substring(0, 4));
            var valorMes = int.Parse(valorPeriodo.Substring(4, 2));

            if (valorAnio < 1900)
                return false;

            if (valorMes < 1 || valorMes > 12)
                return false;

            return true;
        }
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
            var resMes = mes < 10 ? mes.ToString().PadLeft(2, '0') : mes.ToString();
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
        public void sarasa()
        {
            Console.WriteLine("aaaaaaaaaaaaaaaa");
        }
        public override string ToString()
        {
            if (this.IsValid)
                return this.Value.ToString();
            return string.Empty;
        }
    }
}