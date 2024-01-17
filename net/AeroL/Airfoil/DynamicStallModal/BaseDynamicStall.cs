

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

using OpenWECD.AeroL;
using OpenWECD.IO.Type;
using static OpenWECD.IO.math.InterpolateHelper;
using System;
namespace OpenWECD.AeroL.Airfoil.DynamicStallModal
{
    public class BaseDynamicStall
    {
        /// <remarks>
        /// 升力系数和阻力系数插值函数
        /// </remarks>
        /// <param name="AOA">攻角</param>
        /// <param name="FoilNo">翼型编号</param>
        /// <param name="Airfoils">一个Airfoils结构体</param>
        /// <returns></returns>
        /// 
        public static (double cl, double cd,double Cm) LiftDragCoeffInterp(double AOA, int FoilNo, Airfoil1 Airfoils)
        {
            double Cl = 0.0;
            double Cd = 0.0;
            double Cm = 0.0;
            if (FoilNo <= Airfoils.Nfoil)
            {
                Cl = Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(1), AOA);
                Cd = Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(2), AOA);
                Cm = Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(3), AOA);
            }
            else
            {
                Console.WriteLine("W A R N I N G!:The foilno: " + FoilNo + " not set!the cl,cd will be set to 0");
            }
            return (Cl, Cd,Cm);
        }


    }
}
