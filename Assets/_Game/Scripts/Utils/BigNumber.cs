using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BigNumber {

    public double value;
    public int exp;
    const int MAX_MAGNITUDE = 28; // Max power magnitude diff for operands.
    const int TEN_CUBED = (int)1E3; // Used for normalizing numbers.
    bool negative;
    //int minValueToConvert = 10000;
    // Power of 10 names (only multiple of 3 for Engineering Notation).
    // Using  Conway-Wechsler notation system. http://mrob.com/pub/math/largenum.html#conway-wechsler
    // Note: Any name added to the list will be picked up automatically by the BigNum class.
    List<string> powTenToName = new List<string>(){
        "",
         "a"     , //"Thousand",
         "b"     ,//"Millions",
         "c"     ,//"Billions",
         "d"    ,//"Trillions",
         "e" ,//"Quadrillions",
         "f" ,//"Quintillions",
         "g"  ,//"Sextillions",
         "h"  ,//"Septillions",
         "i"   ,//"Octillions",
         "j"   ,//"Nonillions",
         "k"   ,//"Decellions",
         "l",
         "m",
         "n",
         "o",
         "p",
         "q",
         "r",
         "s",
         "t",
         "u",
         "v",
         "w",
         "x",
         "y",
         "z",
         "aa"
    };

    //***********************************************************************
    // Constructor (Default value provided by long)
    //***********************************************************************

    public BigNumber(long number) {
        if (number < TEN_CUBED) {
            this.exp = 3;
            this.value = (double)number / TEN_CUBED;
        } else {
            this.exp = (int)(Math.Log10(number));
            this.value = number / (Math.Pow(10, this.exp));
        }
    }

    //***********************************************************************
    // Constructor (Default value provided by ulong)
    //***********************************************************************

    public BigNumber(ulong number) {
        if (number < TEN_CUBED) {
            this.exp = 3;
            this.value = (double)number / TEN_CUBED;
        } else {
            this.exp = (int)(Math.Log10(number));
            this.value = number / (Math.Pow(10, this.exp));
        }
    }

    //***********************************************************************
    // Constructor (Default value provided by float)
    //***********************************************************************
    public BigNumber(float number) {
        if (number < TEN_CUBED) {
            this.exp = 3;
            this.value = (double)number / TEN_CUBED;
        } else {
            this.exp = (int)(Math.Log10(number));
            this.value = number / (Math.Pow(10, this.exp));
        }
    }

    //***********************************************************************
    // Constructor (Default value provided by BigNumber)
    //***********************************************************************

    public BigNumber(BigNumber bi) {
        this.value = bi.value;
        this.exp = bi.exp;
    }

    public BigNumber() { }
    public BigNumber(double number) {
        if (number < TEN_CUBED) {
            this.exp = 3;
            this.value = (double)number / TEN_CUBED;
        } else {
            this.exp = (int)(Math.Log10(number));
            this.value = number / (Math.Pow(10, this.exp));
        }
    }
    public static implicit operator BigNumber(long value) {
        return new BigNumber(value);
    }

    public static implicit operator BigNumber(ulong value) {
        return new BigNumber(value);
    }

    public static implicit operator BigNumber(int value) {
        return new BigNumber((long)value);
    }

    public static implicit operator BigNumber(uint value) {
        return new BigNumber((ulong)value);
    }

    public static implicit operator BigNumber(float value) {
        return new BigNumber((float)value);
    }

    public static implicit operator BigNumber(double value) {
        return new BigNumber((double)value);
    }

    public static bool operator ==(BigNumber bi1, BigNumber bi2) {
        return bi1.exp == bi2.exp && bi1.value == bi2.value;
    }
    public static bool operator !=(BigNumber bi1, BigNumber bi2) {
        return !(bi1.Equals(bi2));
    }


    public override bool Equals(object o) {
        BigNumber bi = (BigNumber)o;
        return this == bi;
    }


    public override int GetHashCode() {
        return this.ToString().GetHashCode();
    }

    public BigNumber normalize() {
        if (this.value < 1 && this.exp != 0) {
            // e.g. 0.1E6 is converted to 100E3 ([0.1, 6] = [100, 3])
            this.value *= TEN_CUBED;
            this.exp -= 3;
        } else if (this.value >= TEN_CUBED) {
            // e.g. 10000E3 is converted to 10E6 ([10000, 3] = [10, 6])
            while (this.value >= TEN_CUBED) {
                this.value *= (1 / (double)TEN_CUBED);
                this.exp += 3;
            }
        } else if (this.value <= 0) {
            // Negative flag is set but negative number operations are not supported.
            this.negative = this.value < 0 ? true : false;
            this.exp = 0;
            this.value = 0;
        }
        int e2 = exp % 3;
        if (exp > 3) {
            this.value *= Math.Pow(10, e2);
            exp -= e2;
        }
        return this;
    }
    // Compute the equivalent number at 1.Eexp (note: assumes exp is greater than this.exp).
    public BigNumber align(int exp) {
        int d = exp - this.exp;
        if (d > 0) {
            this.value = ((d <= MAX_MAGNITUDE) ? this.value / Math.Pow(10, d) : 0);
            this.exp = exp;
        }
        return this;
    }
    // Add a number to this number.
    public BigNumber Add(BigNumber bigNum) {
        
        if (bigNum.exp < this.exp) {
            bigNum.align(this.exp);
        } else {
            align(bigNum.exp);
        }
        this.value += bigNum.value;
        return normalize();
    }
    // Subtract a number from this number.
    public BigNumber Substract(BigNumber bigNum) {
        if (bigNum.exp < this.exp) {
            bigNum.align(this.exp);
        } else {
            align(bigNum.exp);
        }
        this.value -= bigNum.value;
        return normalize();
    }
    // Multiply this number by factor.
    public BigNumber Multiply(double factor) {
        // We do not support negative numbers.
        if (factor >= 0) {
            this.value *= factor;
        }
        return normalize();
    }
    public BigNumber Multiply(BigNumber factor) {
        this.value *= factor.value;
        this.exp += factor.exp;
        return normalize();
    }
    // Devide this number by factor.
    public BigNumber Divide(int divisor) {
        if (divisor > 0) {
            this.value /= divisor;
        }
        return normalize();
    }
    public BigNumber Divide(BigNumber amount, bool isReleaseVal = false) {
        int max = Math.Max(exp, amount.exp);
        double v1 = value;
        double v2 = amount.value;
        int m1 = exp;
        int m2 = amount.exp;
        for (int i = 0; i < max - m1; i++) {
            v1 /= 1000;
        }
        for (int i = 0; i < max - m2; i++) {
            v2 /= 1000;
        }
        BigNumber n1 = new BigNumber(v1 / v2);
        BigNumber ret = n1.normalize();
        return ret;
    }
    public bool IsBigger(BigNumber amount) {
        amount.normalize();
        normalize();
        if (exp > amount.exp) return true;
        if (exp == amount.exp && value >= amount.value) return true;
        return false;
    }
    public override string ToString() {
        normalize();
        string format = "";
        if (exp < 3 && value % (int)value == 0)
            format = "0";
        else format = "0.0";
        return this.value.ToString(format).Replace(",", ".") + "" + powTenToName[this.exp / 3];
    }

    public string ToStringDown() {
        normalize();
        string format = "";
        if (exp < 3 && value % (int)value == 0)
            format = "0";
        else format = "0.00";
        double yourDoubleValue = value;
        int decimalPlaces = 2;

        // Calculate the factor to shift the decimal places
        double factor = Math.Pow(10, decimalPlaces);

        // Round down the double value to the specified decimal places
        double roundedDownValue = Math.Floor(yourDoubleValue * factor) / factor;
        return roundedDownValue.ToString(format).Replace(",", ".") + "" + powTenToName[this.exp / 3];
    }
    public string ToString2(string format = "") {
        normalize();
        if (exp <= 5 && value <= 9) return (value * Math.Pow(10, exp)).ToString(format);
        return this.value.ToString(format) + "" + powTenToName[this.exp / 3];
    }
    public string ToString3()
    {
        normalize();
        if (exp <= 5 && value <= 9) return (value * Math.Pow(10, exp)).ToString("0.00",System.Globalization.CultureInfo.InvariantCulture);
        //return this.value.ToString(format) + "" + powTenToName[this.exp / 3];
        if (this.exp == 0)
            return this.value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        return this.value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + "E" + this.exp.ToString();
    }
    public string IntToString(string format = "") {
        normalize();
        if (exp <= 5 && value <= 9) return ((int)(value * Math.Pow(10, exp))).ToString(format);
        return Math.Round(value, 2).ToString(format) + "" + powTenToName[this.exp / 3];
    }

    //***********************************************************************
    // Overloading of inequality operator
    //***********************************************************************

    public static bool operator >(BigNumber bi1, BigNumber bi2) {
        int max = Math.Max(bi1.exp, bi2.exp);

        double v1 = bi1.value;
        double v2 = bi2.value;
        for (int i = 0; i < max - bi1.exp; i++) {
            v1 /= 1000;
        }

        for (int i = 0; i < max - bi2.exp; i++) {
            v2 /= 1000;
        }
        return v1 > v2;
    }

    public static bool operator <(BigNumber bi1, BigNumber bi2) {
        int max = Math.Max(bi1.exp, bi2.exp);

        double v1 = bi1.value;
        double v2 = bi2.value;

        for (int i = 0; i < max - bi1.exp; i++) {
            v1 /= 1000;
        }

        for (int i = 0; i < max - bi2.exp; i++) {
            v2 /= 1000;
        }

        return v1 < v2;
    }


    public static bool operator >=(BigNumber bi1, BigNumber bi2) {
        return (bi1 == bi2 || bi1 > bi2);
    }


    public static bool operator <=(BigNumber bi1, BigNumber bi2) {
        return (bi1 == bi2 || bi1 < bi2);
    }

    //***********************************************************************
    // Overloading of division operator
    //***********************************************************************

    public static BigNumber operator /(BigNumber bi1, BigNumber bi2) {
        BigNumber ret = new BigNumber(bi1);

        return ret.Divide(bi2);
    }
    public static BigNumber operator +(BigNumber bi1, BigNumber bi2) {
        BigNumber ret = new BigNumber(bi1);
        return ret.Add(bi2);
    }
    public static BigNumber operator -(BigNumber bi1, BigNumber bi2) {
        BigNumber ret = new BigNumber(bi1);

        return ret.Substract(bi2);
    }
    public static BigNumber operator *(BigNumber bi1, BigNumber bi2) {
        BigNumber ret = new BigNumber(bi1);

        return ret.Multiply(bi2);
    }

    public double ToDouble()
    {
        return value * Mathf.Pow(10, exp);
    }
}
