


//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2024  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,ZZZ
//
//    This file is part of OpenWECD.AeroL.Airfoil.DynamicStallModal
//
// Licensed under the Boost Software License - Version 1.0 - August 17th, 2003
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.openwecd.fun/licenses
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
// SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
// FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
//**********************************************************************************************************************************
using OpenWECD.AeroL.BEM.SteadyBEMT;
using OpenWECD.IO.Log;
using OpenWECD.IO.math;
using OpenWECD.IO.Type;
using OpenWECD.AeroL.Airfoil.DynamicStallModal;
using static OpenWECD.IO.math.InterpolateHelper;
using System;
namespace OpenWECD.AeroL.Airfoil.DynamicStallModal
{
    /// <summary>
    ///  Øye 失速模型
    /// </summary>
    public class Oye : BaseDynamicStall, I_ALDynamicStall
    {

        double[,,]? fs;
        double currentT = 0;
        private Airfoil1 Airfoils;
        private int NumSec;
        private int NumB;
        public Oye(AeroL1 aeroL1, int NumB, int NumSec)
        {
            Airfoils = aeroL1.Airfoil;
            this.NumB = NumB;
            this.NumSec = NumSec;
            AirfoilSolve();
            fs = new double[2, NumSec, NumB];
            LogHelper.WriteLog("Run Oye Dynamic Stall Modol to Solve！", title: "[success]",leval:1);
        }
        public (double Cl, double Cd, double Cm) AL_CalBladeDynamicStall(double phi, double theta, int FoilNo)
        {
            return AL_DynamicStall(phi, theta, FoilNo, ref AeroL_INI.AL_IDynamicStallPar);
        }

        public (double Cl, double Cd, double Cm) AL_DynamicStall(double phi, double theta, int FoilNo,ref  T_ALDynamicStallPar Par)
        {
            double AOA = phi - theta;
            double t = Par.t;
            double dt = Par.dt;
            int Numsec = Par.Numsec;
            int Numb = Par.Numb;
            double urel = Par.Urel;
            double chord = Par.Cord;


            (var Cl, var Cd, var Cm) = LiftDragCoeffInterp(AOA, FoilNo, Airfoils);
            if (t == 0 | Math.Abs(AOA) > 0.872)//+- 50度使用动态失速模型
            {
                return (Cl, Cd, Cm);
            }
            var fs_st = Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(4), AOA);// interp(alpha, obj.airfoil[:, 1, i_z], obj.airfoil[:, 5, i_z])[1]

            //# 插值无粘升力系数
            var Cl_inv = Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(5), AOA);

            //# 完全分离时的升力系数
            var Cl_fs = Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(6), AOA);
            //计算时间系数tau
            double tau = Airfoils[FoilNo].T_f_OYG / 2.0 * chord / urel;
            if (currentT != t)
            {
                fs[1, Numsec, Numb] = fs_st + (fs[0, Numsec, Numb] - fs_st) * Math.Exp(-dt / tau);
                Cl = fs[1, Numsec, Numb] * Cl_inv + (1 - fs[1, Numsec, Numb]) * Cl_fs;
            }
            else
            {
                Cl = fs[1, Numsec, Numb] * Cl_inv + (1 - fs[1, Numsec, Numb]) * Cl_fs;
                fs[0, Numsec, Numb] = fs[1, Numsec, Numb];
            }
            //Console.WriteLine($"{Numsec}=>{Numb}=>{this.NumSec}={this.NumB}");
            if (Numsec == this.NumSec - 2 & Numb == this.NumB - 1)
            {
                currentT = t;
            }

            //fs[t][Numsec, Numb] = fs_st + (fs[dynamicStall.Tspan[dynamicStall.I_t-1]][Numsec, Numb] - fs_st) * Math.Exp(-dt / tau);
            ////# 升力系数顺时值
            //Cl = fs[t][Numsec, Numb] * Cl_inv + (1 - fs[t][Numsec, Numb]) * Cl_fs;
            return (Cl, Cd, Cm);

        }
        public void AirfoilSolve()
        {
            for (int i = 0; i < Airfoils.Nfoil; i++)
            {
                var data = LinearAlgebraHelper.zeros(Airfoils[i].DataSet.RowCount, Airfoils[i].DataSet.ColumnCount + 3);
                data.SetSubMatrix(0, 0, Airfoils[i].DataSet);
                data.SetColumn(4, Airfoils[i].DataSet.Column(1) * 1.1);
                data.SetColumn(5, Airfoils[i].DataSet.Column(1) * 0.8);
                data.SetColumn(6, Airfoils[i].DataSet.Column(1) * 0.5);
                var air = Airfoils[i];
                air.DataSet = data;
                Airfoils[i] = air;
            }
        }


    }
}
