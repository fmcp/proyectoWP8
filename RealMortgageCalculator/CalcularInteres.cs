﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RealMortgageCalculator {

    class CalcularInteres {
        
        //Capital de la hipoteca: 
	    private double capital;

        //Interés anual aplicado a la hipoteca
        private  double interesAnual;

        //Número de mensualidades en las que se pagará el préstamo
		private int periodos;

        private Boolean calcInteres, calcMatriz;

        //Resultados del cálculo de la hipoteca
        private double cuotaMensual;
        private double cuotaMensualCV;
        private double interesMes;
        private double interesMesCV;
		private double[,] matriz, matrizCV;

        /**
         * Constructor de la clase. Crea un
         * @param capital Capital de la hipoteca
         * @param interes Interés aplicado al préstamo
         * @param periodos Periodos en los que se dividirá el pago
         **/
		CalcularInteres(double capital, double interes, int periodos) {
			this.capital = capital;
			this.interesAnual = interes;
			this.periodos = periodos;
            this.calcInteres = false;
            this.calcMatriz = false;
		}

        /**
         * Calcula los intereses y la cuota mensual tanto por el sistema tradicional como por el correcto
         **/
		public void calcular() {

            //Cálculo de los intereses mensuales:
            interesMes = calcularIntMes(interesAnual);
            interesMesCV = calcularIntMesCuentaVieja(interesAnual);

            //Cálculo de las cuotas mensuales
			cuotaMensual = calcularCuotaMensual(capital, interesMes, periodos);
			cuotaMensualCV = calcularCuotaMensual(capital, interesMesCV, periodos);
            
            calcInteres = true;
		}

        /**
         * Calcula las matrices de amortización tanto por el sistema tradicional como por el correcto
         **/
		public void calcularMatriz()
		{
			matriz = new double[3, periodos];
			matrizCV = new double[3, periodos];

            //Si no han sido calculados los intereses,  se calculan
            if (!this.calcInteres)
                this.calcular();

			double deuda = capital, interes = 0, amortizacion = 0;

            //Cálculo de la tabla de amortización real
			int i = 0;
			for (i = 0; i < periodos; i++)
			{
				interes = deuda * interesMes;
				amortizacion = cuotaMensual - interes;
				deuda = deuda - amortizacion;
				matriz[0, i] = deuda;
				matriz[1, i] = amortizacion;
				matriz[2, i] = interes;
			}


            //Cálculo de la tabla de amortización aproximada
			deuda = capital; interes = 0; amortizacion = 0;
			for (i = 0; i < periodos; i++)
			{
				interes = deuda * interesMesCV;
				amortizacion = cuotaMensualCV - interes;
				deuda = deuda - amortizacion;
				matrizCV[0, i] = deuda;
				matrizCV[1, i] = amortizacion;
				matrizCV[2, i] = interes;
			}


            calcMatriz = true;
		}

        /**
         * Calcula tanto los intereses como las matrices de amortización
         **/
		public void run() {
			this.calcular();
			this.calcularMatriz();
		}

        /**
         * Calcula el interés mensual aplicable de forma real
         * @param interes Interés anual
         * @return Interés mensual
         **/
        static double calcularIntMes(double interes) {
			return Math.Pow((1.0 + interes), (1.0 / 12.0)) - 1.0;
		}

        /**
         * Calcula el interés mensual aproximado
         * @param interes Interés anual
         * @return Interés mensual
         **/
		static double calcularIntMesCuentaVieja(double interes)	{
			return interes / 12.0;
		}

        /**
         *  Calcula la cuota mensual de un préstamo
         *  @param cantidad Cuantía del préstamo
         *  @param interes Interés mensual aplicable
         *  @param periodos Número de períodos en los que se dividirá el pago (mensualidades)
         **/
		static double calcularCuotaMensual(double cantidad, double interes, int periodos) 
        {
			return cantidad * (interes / (1 - (1 / (Math.Pow((1 + interes), (periodos))))));
		}

        /**
         *  Devuelve el interés mensual de la hipoteca
         *  @return Interés mensual
         **/
        public double getInteresMes()
        {
            if (!this.calcInteres)
                this.calcular();
            return this.interesMes;
        }

        /**
         * @return Interés mensual por el método de la cuenta la vieja
         **/
        public double getInteresMesCV()
        {
            if (!this.calcInteres)
                this.calcular();
            return this.interesMesCV;
        }

        /**
         * @return Cuota mensual que será necesario pagar cada mes
         **/
        public double getCuotaMensual()
        {
            if (!this.calcInteres)
                this.calcular();
            return this.cuotaMensual;
        }

        /**
         * @return Cuota mensual que será necesario pagar cada mes por el método bancario
         **/
        public double getCuotaMensualCV()
        {
            if (!this.calcInteres)
                this.calcular();
            return this.cuotaMensualCV;
        }

        /**
         * @return Matriz de amortización correcta
         **/
        public double[,] getMatriz()
        {
            if (!this.calcMatriz)
                this.calcularMatriz();
            return this.matriz;
        }

        /**
         * @return Matriz de amortización por el método bancario
         **/
        public double[,] getMatrizCV()
        {
            if (!this.calcMatriz)
                this.calcularMatriz();
            return this.matrizCV;
        }
    }
}