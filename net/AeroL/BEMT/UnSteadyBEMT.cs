

//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2024  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,ZZZ
//
//    This file is part of OpenWECD.AeroL.BEMT
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

using MathNet.Numerics.LinearAlgebra;
using OpenWECD.IO.Log;
using OpenWECD.IO.Type;
using static OpenWECD.IO.math.InterpolateHelper;
using static OpenWECD.IO.math.RootsHelper;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
namespace OpenWECD.AeroL.BEM.UnSteaddyBEMT
{

    /// <remarks>
    /// 这个类型当中封装了非常多的方法
    /// </remarks>
    public class UnSteadyBEMT : AeroL_IO_Subs.AL_PutBemEle, I_ALCalDynamicStall, I_ALCalDynamicAeroLoad, I_ALUnsteadyPutRes
    {
        #region 字段
        private static AeroL1 AeroL;
        private static Airfoil1 Airfoils;
        private static int nb_Load;
        private static int BldNum;
        private static Matrix<double>? px;// = Matrix<double>.Build.Dense(nb_Load, 3);
        private static Matrix<double>? py;//= Matrix<double>.Build.Dense(nb_Load, 3);
        private static Matrix<double>? Mz;//= Matrix<double>.Build.Dense(nb_Load, 3);

        #endregion 字段
        /// <summary>
        /// 运行之前，请先调用AeroL_INI.INIDynamicStall(AeroL)来初始化动态失速模型,当前模块下已经默认初始化了AL_INIBladeAeroElement
        /// </summary>
        /// <param name="aeroL1"></param>
        public UnSteadyBEMT(AeroL1 aeroL1)
        {
            AeroL_INI.AL_INIBladeAeroElement(aeroL1);
            AeroL = aeroL1;
            Airfoils = aeroL1.Airfoil;
            nb_Load = aeroL1.NumBldNds;
            BldNum = aeroL1.Bldnum;
            px = Matrix<double>.Build.Dense(nb_Load, BldNum);
            py = Matrix<double>.Build.Dense(nb_Load, BldNum);
            Mz = Matrix<double>.Build.Dense(nb_Load, BldNum);
            LogHelper.WriteLog("Run BEMT Modol to Solve！", title: "[success]", leval: 1);
            //初始化初始化流
            AL_PutINI(aeroL1);
            PutInI(aeroL1.AerolData[1]);
        }
      
        public (Matrix<double> px, Matrix<double> py, Matrix<double> Mz, Matrix<double> phi) AL_CalBladeDynamicAeroLoad(double q_GeAz, double qd_GeAz, List<Matrix<double>> VMB, List<Matrix<double>> UMB, Matrix<double> phi, Matrix<double> FlexBlSpn, Vector<double> Pitch, double chi0)
        {
            BEMTMex(ref AeroL_INI.AL_IBladeAeroElement, q_GeAz, qd_GeAz, VMB, UMB, ref phi, FlexBlSpn, Pitch, chi0, nb_Load, ref AeroL_INI.AL_IDynamicStallPar, ref px, ref py, ref Mz);
            return (px, py, Mz, phi);
        }

        public void AL_WriteTitle(params string[] title)
        {
            WriteTitle(directory, title);
        }
        public void AL_WriteUnit(params string[] title)
        {
            WriteUnit(directory, GetResUnit, AeroL.Outputs_OutList, title);
        }
        public void AL_Write(double value)
        {
            Write(directory, value);
        }
      
        public void AL_Write(string value)
        {
            Write(directory, value);
        }
      
        public void AL_WriteLine(double value)
        {
            WriteLine(directory, value);
        }
      
        public void AL_WriteLine(string value)
        {
            WriteLine(directory, value);
        }

      
        public (double Cl, double Cd, double Cm) AL_DynamicStall(double phi, double theta, int FoilNo)
        {
            return AeroL_INI.AL_ICalDynamicStall(phi, theta, FoilNo);
        }
      
        private void BEMTMex(ref T_ALAeroBladeElement[,] BlDEle, double q_GeAz, double qd_GeAz, List<Matrix<double>> VMB, List<Matrix<double>> UMB, ref Matrix<double> phi, Matrix<double> FlexBlSpn, Vector<double> Pitch, double chi0, int nb_Load, ref T_ALDynamicStallPar DynamicStallPar, ref Matrix<double> px, ref Matrix<double> py, ref Matrix<double> Mz)
        {

            double TipRad = BlDEle[0, 0].STipRad;
            double HubRad = AeroL.HubRad;
            for (int B = 0; B < AeroL.Bldnum; B++)
            {
                double Azimuth = q_GeAz + B * 2.0 * Math.PI / 3.0;
                for (int iNdP = 1; iNdP < nb_Load - 1; iNdP++)
                {
                    //int iNdP = iNd - 1;
                    double Solid = BlDEle[iNdP, B].SSolid / FlexBlSpn[iNdP, B];// (3.0 / 2.0 / Math.PI) * Cord[iNdP ] / FlexBlSpn[iNdP , B];
                    double Vx = VMB[B][iNdP, 0] - UMB[0][iNdP, B];
                    double Vy = VMB[B][iNdP, 1] + UMB[1][iNdP, B];

                    DynamicStallPar.Numsec = iNdP;
                    DynamicStallPar.Numb = B;
                    DynamicStallPar.Cord = BlDEle[iNdP, B].SChord;
                    DynamicStallPar.Urel = Math.Sqrt(Vx * Vx + Vy * Vy);

                    double theta = Pitch[B] + BlDEle[iNdP, B].STwist;



                    double Residue = CallStateResidual(phi[iNdP, B], theta, Vx, Vy, BlDEle[iNdP, B].SFoilNum, Airfoils, TipRad, HubRad, BlDEle[iNdP, B].SBlspan, Solid, Azimuth, chi0);
                    double a, aa, Cx, Cy, Cl, Cd, Cm;
                    if (Vx == 0 || Vy == 0)
                    {
                        phi[iNdP, B] = Math.Atan2(Vx, Vy);
                        (a, aa, Cx, Cy, Cl, Cd, Cm) = CalcOutput(phi[iNdP, B], theta, BlDEle[iNdP, B].SFoilNum, ref Airfoils, TipRad, HubRad, BlDEle[iNdP, B].SBlspan, Solid, Azimuth, chi0);
                    }
                    else if (Math.Abs(Residue) < 1e-3)
                    {
                        (a, aa, Cx, Cy, Cl, Cd, Cm) = CalcOutput(phi[iNdP, B], theta, BlDEle[iNdP, B].SFoilNum, ref Airfoils, TipRad, HubRad, BlDEle[iNdP, B].SBlspan, Solid, Azimuth, chi0);
                    }
                    else
                    {
                        phi[iNdP, B] = UpdateInflowAngle(ref AeroL, theta, Vx, Vy, BlDEle[iNdP, B].SFoilNum, Airfoils, TipRad, HubRad, BlDEle[iNdP, B].SBlspan, Solid, Azimuth, chi0);
                        (a, aa, Cx, Cy, Cl, Cd, Cm) = CalcOutput(phi[iNdP, B], theta, BlDEle[iNdP, B].SFoilNum, ref Airfoils, TipRad, HubRad, BlDEle[iNdP, B].SBlspan, Solid, Azimuth, chi0);
                    }

                    // Final Velocities and forces //
                    double W = Math.Pow(Vx * (1 - a), 2) + Math.Pow(Vy * (1 + aa), 2);

                    double Const = 0.5 * AeroL.AirDens * BlDEle[iNdP, B].SChord * W;

                    //赋值
                    BlDEle[iNdP, B].DPx = px[iNdP, B] = Cx * Const;
                    BlDEle[iNdP, B].DPy = py[iNdP, B] = -Cy * Const;
                    BlDEle[iNdP, B].DMz = Mz[iNdP, B] = Cl * Const * BlDEle[iNdP, B].SAeroCentJ2 - Cd * Const * BlDEle[iNdP, B].SAeroCentJ1;// + Cm * Const * BlDEle[iNdP, B].SChord;
                    BlDEle[iNdP, B].Vindx = Vx;
                    BlDEle[iNdP, B].Vindy = Vy;
                    BlDEle[iNdP, B].DPitch = Pitch[B];
                    BlDEle[iNdP, B].DFlexSpan = FlexBlSpn[iNdP, B];
                    BlDEle[iNdP, B].DAzimuth = Azimuth;
                    BlDEle[iNdP, B].DChi0 = chi0;
                    BlDEle[iNdP, B].DU = W;
                    BlDEle[iNdP, B].DVx = Vx * (1 - a);
                    BlDEle[iNdP, B].DVy = Vy * (1 + aa);
                    BlDEle[iNdP, B].Da = a;
                    BlDEle[iNdP, B].Daa = aa;
                    BlDEle[iNdP, B].DCx = Cx;
                    BlDEle[iNdP, B].DCy = Cy;
                    BlDEle[iNdP, B].DCl = Cl;
                    BlDEle[iNdP, B].DCd = Cd;
                    BlDEle[iNdP, B].DCm = Cm;
                    BlDEle[iNdP, B].Dphi = phi[iNdP, B];
                    BlDEle[iNdP, B].DVrel = DynamicStallPar.Urel;
                    BlDEle[iNdP, B].DTheta = theta;
                    BlDEle[iNdP, B].DAOA = phi[iNdP, B] - theta;
                    BlDEle[iNdP, B].lmda = qd_GeAz * FlexBlSpn[iNdP, B] / Vx;
                    BlDEle[iNdP, B].DMx = Cy * Const *BlDEle[iNdP, B].SBlspan;
                }
            }
            //Console.WriteLine();
        }
        /// <remarks>
        /// 升力系数和阻力系数插值函数
        /// </remarks>
        /// <param name="AOA">攻角</param>
        /// <param name="FoilNo">翼型编号</param>
        /// <param name="Airfoils">一个Airfoils结构体</param>
        /// <returns></returns>
        /// 
      
        private static (double cl, double cd, double cm) LiftDragCoeffInterp(double AOA, int FoilNo, ref Airfoil1 Airfoils)
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
            return (Cl, Cd, Cm);
        }

        // Hub/Tip Losses
        /*
         * 该函数用来计算损失修正系数F
         * 输入:
         * - phi: 入流角
         * - TipRad: 叶片长度,是包含轮毂半径的
         * - HubRad: 轮毂半径
         * - BlSpn: 叶片的展向位置
         * - Bldnum: 叶片数，默认为3
         * 输出:
         * - F: 叶尖和轮毂损失F=Fhub*Ftip
         */
        /// <remarks>
        /// 该函数用来计算损失修正系数F
        /// </remarks>
        /// <param name="phi">入流角</param>
        /// <param name="TipRad">叶片长度,是包含轮毂半径的</param>
        /// <param name="HubRad">轮毂半径</param>
        /// <param name="BlSpn">叶片的展向位置</param>
        /// <param name="Bldnum">叶片数，默认为3</param>
        /// <returns>F: 叶尖和轮毂损失F=Fhub*Ftip</returns>
      
        private double Hub_tip_loss(double phi, double TipRad, double HubRad, double BlSpn, int Bldnum)
        {
            double abssinphi = Math.Abs(Math.Sin(phi));
            double ftip = Bldnum / 2.0 * (TipRad - BlSpn) / (BlSpn * abssinphi);
            double Ftip = 2.0 / Math.PI * Math.Acos(Math.Min(1, Math.Exp(-ftip)));

            double fhub = Bldnum / 2.0 * (BlSpn - HubRad) / (HubRad * abssinphi);
            double Fhub = 2.0 / Math.PI * Math.Acos(Math.Min(1, Math.Exp(-fhub)));
            double F = Ftip * Fhub;
            return F;
        }

        /// <remarks>
        /// 存储入流角，亟已经计算过的入流角，以加快计算速度
        /// </remarks>
        private static double phiStore = -100;

        /// <remarks>
        /// 更新入流角，如果上面的CallResidual函数表明，当前计算没有收敛这更新入流角来迭代求解，
        /// </remarks>
        /// <param name="aeroL"></param>
        /// <param name="theta"></param>
        /// <param name="Vx"></param>
        /// <param name="Vy"></param>
        /// <param name="FoilNo"></param>
        /// <param name="Airfoils"></param>
        /// <param name="TipRad"></param>
        /// <param name="HubRad"></param>
        /// <param name="BlSpn"></param>
        /// <param name="Solid"></param>
        /// <param name="Azimuth"></param>
        /// <param name="chi0"></param>
        /// <returns></returns>
      
        private double UpdateInflowAngle(ref AeroL1 aeroL, double theta, double Vx, double Vy, int FoilNo, Airfoil1 Airfoils, double TipRad, double HubRad, double BlSpn, double Solid, double Azimuth, double chi0)
        {
            double ep = 1e-6;
            double Res(double x) => CallStateResidual(x, theta, Vx, Vy, FoilNo, Airfoils, TipRad, HubRad, BlSpn, Solid, Azimuth, chi0);

            double x0, x1, phi;
            if (Res(Math.PI / 2.0) * Res(ep) < 0)
            {
                x0 = ep;
                x1 = Math.PI / 2.0;
                phi = fzero(Res, x0, x1, aeroL.BemtError, aeroL.MaxIter, ref phiStore);
                //phi = fzero(Res, x0, x1, 1E-3);
            }
            else if (Res(-Math.PI / 4.0) * Res(-ep) < 0)
            {
                x0 = -Math.PI / 4.0;
                x1 = -ep;
                phi = fzero(Res, x0, x1, aeroL.BemtError, aeroL.MaxIter, ref phiStore);
            }
            else
            {
                x0 = Math.PI / 2.0;
                x1 = Math.PI - ep;
                phi = fzero(Res, x0, x1, aeroL.BemtError, aeroL.MaxIter, ref phiStore);
            }
            phiStore = phi;
            return phi;
        }


        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="phi"></param>
        /// <param name="theta"></param>
        /// <param name="FoilNo"></param>
        /// <param name="Airfoils"></param>
        /// <param name="TipRad"></param>
        /// <param name="HubRad"></param>
        /// <param name="BlSpn"></param>
        /// <param name="Solid"></param>
        /// <param name="Azimuth"></param>
        /// <param name="chi0"></param>
        /// <returns>a, aa, Cx, Cy, Cl, Cd</returns>
      
        private (double a, double aa, double Cx, double Cy, double Cl, double Cd, double Cm) CalcOutput(double phi, double theta, int FoilNo, ref Airfoil1 Airfoils, double TipRad, double HubRad, double BlSpn, double Solid, double Azimuth, double chi0)
        {
            double AOA = phi - theta;
            double Cl = 0;
            double Cd = 0;
            double Cm = 0;
            if (!AeroL_INI.AL_IDynamicStallPar.UseDynamicStallPar)
            {
                (Cl, Cd, Cm) = LiftDragCoeffInterp(AOA, FoilNo, ref Airfoils);
            }
            else
            {
                (Cl, Cd, Cm) = AL_DynamicStall(phi, theta, FoilNo);
            }

            double Cx = Cl * Math.Cos(phi) + Cd * Math.Sin(phi);
            double Cy = Cl * Math.Sin(phi) - Cd * Math.Cos(phi);
            //(double Cx, double Cy) = ForceCoefficents(AOA, phi, FoilNo, Airfoils);
            double F = Hub_tip_loss(phi, TipRad, HubRad, BlSpn, BldNum);

            double cosphi = Math.Cos(phi);
            double sinphi = Math.Sin(phi);

            // Non dimensional parameters
            double k;
            if (sinphi == 0.0)
            {
                k = double.MaxValue;
            }
            else
            {
                k = Solid * Cx / (4.0 * F * sinphi * sinphi);
            }

            // Different equation depending on solution region
            double a;
            if (phi > 0.0)
            {
                if (k <= 2.0 / 3.0) // momentum region
                {
                    if (k == -1.0)
                    {
                        k = k - 0.1;
                    }
                    a = k / (1.0 + k);
                    a = Math.Max(a, -10.0); // Patch
                }
                else // emperical region
                {
                    double g1 = 2.0 * F * k - (10.0 / 9.0 - F);
                    double g2 = 2.0 * F * k - F * (4.0 / 3.0 - F);
                    double g3 = 2.0 * F * k - (25.0 / 9.0 - 2.0 * F);
                    if (Math.Abs(g3) < 1e-6)
                    {
                        a = 1.0 - 1.0 / (2.0 * Math.Sqrt(g2));
                    }
                    else
                    {
                        a = (g1 - Math.Sqrt(g2)) / g3;
                    }
                }
            }
            else if (phi < 0.0)
            {
                if (k > 1.0) // propeller brake region
                {
                    a = k / (k - 1.0);
                    a = Math.Min(a, 10); // Patch
                }
                else
                {
                    a = 0.0;
                }
            }
            else
            {
                return (0.0, 0.0, Cx, Cy, Cl, Cd, Cm);
            }

            // Pitt and Peters yaw correction model
            double x = (0.6 * a + 1) * chi0;
            a = a * (1.0 + 15 * Math.PI / 64 * Math.Tan(x / 2.0) * BlSpn / TipRad * Math.Sin(Azimuth));

            // Tangential induction factor
            double kk;
            if (sinphi * cosphi == 0)
            {
                kk = double.MaxValue;
            }
            else
            {
                kk = Solid * Cy / (4 * F * sinphi * cosphi);
            }

            double aa;
            if (kk == 1.0)
            {
                a = 0.0;
                aa = 0.0;
            }
            else
            {
                aa = kk / (1 - kk);
                if (Math.Abs(aa) > 10.0)
                {
                    aa = 10 * Math.Sign(aa); // Patch
                }
            }

            return (a, aa, Cx, Cy, Cl, Cd, Cm);
        }

        /// <remarks>
        /// 计算当前迭代的解和残差
        /// </remarks>
        /// <param name="phi">入流角</param>
        /// <param name="theta">扭角</param>
        /// <param name="Vx">x方向上的入流风速度</param>
        /// <param name="Vy">y方向上的入流风速度</param>
        /// <param name="FoilNo">当前截面的易信编号</param>
        /// <param name="Airfoils">Airfoil1结构体，摘IO.Type当中定义</param>
        /// <param name="TipRad">叶片长度,是包含轮毂半径的[m]</param>
        /// <param name="HubRad">轮毂半径</param>
        /// <param name="BlSpn">展向位置</param>
        /// <param name="Solid">当前截面的实度</param>
        /// <param name="Azimuth">当前的偏航角</param>
        /// <param name="chi0">默认为0，偏航修正参数</param>
        /// <returns>一个残差，用来判断当前是否收敛</returns>
      
        private double CallStateResidual(double phi, double theta, double Vx, double Vy, int FoilNo,
            Airfoil1 Airfoils, double TipRad, double HubRad, double BlSpn, double Solid, double Azimuth,
            double chi0)
        {
            double AOA = phi - theta;


            double Cl = 0;
            double Cd = 0;
            double Cm = 0;
            if (!AeroL_INI.AL_IDynamicStallPar.UseDynamicStallPar)
            {
                (Cl, Cd, Cm) = LiftDragCoeffInterp(AOA, FoilNo, ref Airfoils);
            }
            else
            {
                (Cl, Cd, Cm) = AL_DynamicStall(phi, theta, FoilNo);
            }
            double Cx = Cl * Math.Cos(phi) + Cd * Math.Sin(phi);
            double Cy = Cl * Math.Sin(phi) - Cd * Math.Cos(phi);
            //(double Cx, double Cy) = ForceCoefficents(AOA, phi, FoilNo, Airfoils);
            double F = Hub_tip_loss(phi, TipRad, HubRad, BlSpn, BldNum);
            double cosphi = Math.Cos(phi);
            double sinphi = Math.Sin(phi);

            // Non-dimensional parameters
            double k;
            if (sinphi == 0)
                k = double.MaxValue;
            else
                k = Solid * Cx / (4.0 * F * Math.Pow(sinphi, 2));

            double a = 0.0;

            // Different equation depending on solution region
            if (phi > 0)
            {
                if (k <= 2.0 / 3.0) // momentum region
                {
                    if (k == -1.0)
                        k = k - 0.1;

                    a = k / (1.0 + k);
                    a = Math.Max(a, -10.0); // Patch
                }
                else // empirical region
                {
                    double g1 = 2.0 * F * k - (10.0 / 9.0 - F);
                    double g2 = 2.0 * F * k - F * (4.0 / 3.0 - F);
                    double g3 = 2.0 * F * k - (25.0 / 9.0 - 2.0 * F);

                    if (Math.Abs(g3) < 1e-6)
                        a = 1 - 1 / (2 * Math.Sqrt(g2));
                    else
                        a = (g1 - Math.Sqrt(g2)) / g3;
                }
            }
            else if (phi < 0)
            {
                if (k > 1) // propeller brake region
                {
                    a = k / (k - 1.0);
                    a = Math.Min(a, 10.0); // Patch
                }
                else
                {
                    a = 0.0;
                }
            }
            else if (phi == 0)
            {
                return sinphi / (1.0 - 0.0) - Vx / Vy * cosphi / (1.0 + 0.0);
            }

            // Pitt and Peters yaw correction model
            double x = (0.6 * a + 1.0) * chi0;
            a = a * (1.0 + 15.0 * Math.PI / 64.0 * Math.Tan(x / 2.0) * BlSpn / TipRad * Math.Sin(Azimuth));

            // Tangential induction factor
            double kk;

            if (sinphi * cosphi == 0)
                kk = double.MaxValue;
            else
                kk = Solid * Cy / (4.0 * F * sinphi * cosphi);

            double aa;

            if (kk == 1.0)
            {
                a = 0.0;
                aa = 0.0;
            }
            else
            {
                aa = kk / (1.0 - kk);

                if (Math.Abs(aa) > 10.0)
                    aa = 10.0 * Math.Sign(aa); // Patch
            }

            // Residue
            double Residue;

            if (a == 1.0)
                Residue = -Vx / Vy * cosphi / (1.0 + aa);
            else
                Residue = sinphi / (1.0 - a) - Vx / Vy * cosphi / (1.0 + aa);

            return Residue;
        }

    }
}
