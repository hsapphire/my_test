

//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2024  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,ZZZ
//
//    This file is part of OpenWECD.AeroL
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

using OpenWECD.IO.Log;
using OpenWECD.IO.Type;
using OpenWECD.AeroL.Airfoil.DynamicStallModal;
using MathNet.Numerics.LinearAlgebra;
using OpenWECD.AeroL.BEM.UnSteaddyBEMT;
using OpenWECD.AeroL.BEM.SteadyBEMT;
using System.Collections.Generic;
using System.Linq;
using System;
using OpenWECD.AeroL;
using OpenWECD.IO.IO;
using System.IO;
using OpenWECD.IO.math;
namespace OpenWECD.AeroL
{
    public static class AeroL_INI
    {
        /// <summary>
        /// 获取当前的AeroL结构体
        /// </summary>
        public static AeroL1 AL_IAeroL1;


        #region 动态失速模型初始化，包含一个接口（外部使用），一个方法（AeroL内部使用）

        /// <summary>
        /// 计算失速模型下的叶片气动系数 (double phi, double theta, double urel, double t, double dt, int FoilNo, double chord, int Numsec, int Numb)
        /// </summary>
        public static Func<double, double, int, (double cl, double cd, double Cm)>? AL_ICalDynamicStall;
        /// <summary>
        /// 这个参数是外部接口，使用动态的失速模型需要在每个时间步进行设置。由于可能纯在多线程，多风场的情况，所以不能设置为static!
        /// </summary>
        public static T_ALDynamicStallPar AL_IDynamicStallPar;
        /// <summary>
        /// AeroL 当中的内部函数，外部不需要调用
        /// </summary>
        /// <param name="AeroL"></param>
        internal static void AL_INIDynamicStall(AeroL1 AeroL)
        {

            if (AeroL.ApOfMb == 0)//外部调用
            {
                switch (AeroL.AFAeroMod)
                {
                    case 1:
                        //LogHelper.WriteLog("Run Steady Modol to Solve！No Dynamic Stall Modol", title: "[success]",color:ConsoleColor.DarkYellow);
                        break;
                    case 2:
                        LogHelper.WriteLog("Run B-L Dynamic Stall Modol to Solve！", title: "[success]");
                        break;
                    case 3:
                        AL_IDynamicStallPar = new T_ALDynamicStallPar();
                        AL_IDynamicStallPar.UseDynamicStallPar = true;//设置为使用动态失速模型。
                        var Oye = new Oye(AeroL, AeroL.Bldnum, AeroL.NumBldNds);
                        AL_ICalDynamicStall = Oye.AL_CalBladeDynamicStall;

                        break;
                    case 4:
                        LogHelper.WriteLog("Run IAG Dynamic Stall Modol to Solve！", title: "[success]");
                        break;
                    case 5:
                        LogHelper.WriteLog("Run GOR Dynamic Stall Modol to Solve！", title: "[success]");
                        break;
                    case 6:
                        LogHelper.WriteLog("Run ATEF Dynamic Stall Modol to Solve！", title: "[success]");
                        break;
                    default:
                        LogHelper.ErrorLog($"AeroL.AFAeroMod={AeroL.AFAeroMod},No such Dynamic Stall Modol");
                        break;
                }
            }
            else
            {
                LogHelper.ErrorLog(" AeroL.ApOfMb != 0,NOT ALLOWED USING INIDynamicStall");
            }
        }

        #endregion 动态失速模型初始化，包含一个接口（外部使用），一个方法（AeroL内部使用）


        #region 非定常叶素动量理论初始化

        /// <remarks>
        /// 输入的 q_GeAz, qd_GeAz,VMB, UMB,  phi，FlexBlSpn， Pitch, chi0供多体动力学计算开启动力学方法
        /// </remarks>
        public static Func<double, double, List<Matrix<double>>, List<Matrix<double>>, Matrix<double>, Matrix<double>, Vector<double>, double, (Matrix<double>, Matrix<double>, Matrix<double>, Matrix<double>)>? AL_ICalBladeDynamicAeroLoad;

        /// <summary>
        /// 供外部多体动力学程序对AeroL当中的结果输出写标题
        /// </summary>
        public static Action<string[]>? AL_IWriteTitle;//        WriteTitle(Dictionary<int, OutFile> directory, params string[] title)

        /// <summary>
        /// 供外部多体动力学程序对AeroL当中的结果输出写单位
        /// </summary>
        public static Action<string[]>? AL_IWriteUnit;

        /// <summary>
        /// 供外部多体动力学程序对AeroL当中的结果输出数字
        /// </summary>
        public static Action<double>? AL_IWriteDouble;

        /// <summary>
        /// 供外部多体动力学程序对AeroL当中的结果输出字符串
        /// </summary>
        public static Action<string>? AL_IWriteString;

        /// <summary>
        /// 供外部多体动力学程序对AeroL当中的结果输出字符串
        /// </summary>
        public static Action<string>? AL_IWriteLineString;

        /// <summary>
        /// 供外部多体动力学程序对AeroL当中的结果输出数字
        /// </summary>
        public static Action<double>? AL_IWriteLineDouble;


        /// <summary>
        /// 
        /// </summary>
        public static Action? AL_IWriteBEMEleValue;

        /// <summary>
        /// 初始化非定常开启动力学，外部调用
        /// </summary>
        /// <param name="aeroL"></param>
        public static void AL_INIUnsteatdyAeroload(AeroL1 aeroL)
        {
            LogHelper.WriteLog("Running AeroL.", show_title: false, color: ConsoleColor.Blue);
            AL_IAeroL1 = aeroL;

            switch (aeroL.WakeMod)
            {
                case 1://使用 BEMT方法
                    AL_INIDynamicStall(aeroL);//初始化动态入流模型
                    var uns = new UnSteadyBEMT(aeroL);
                    AL_ICalBladeDynamicAeroLoad = uns.AL_CalBladeDynamicAeroLoad;
                    AL_IWriteTitle = uns.AL_WriteTitle;
                    AL_IWriteUnit = uns.AL_WriteUnit;
                    AL_IWriteLineDouble = uns.AL_WriteLine;
                    AL_IWriteLineString = uns.AL_WriteLine;
                    AL_IWriteString = uns.AL_Write;
                    AL_IWriteDouble = uns.AL_Write;
                    AL_IWriteBEMEleValue = uns.WriteBEMEleValue;
                    //输出文件标准题头
                    uns.AL_WriteTitle( $"OPenWECD旗下产品 OpenHast By TGTeam ! 当前文件版本为 {uns.ProjectName}", $"如果有相关问题请登录官方网站{uns.url} 寻求帮助");


                    break;
                case 2://使用 Vortex方法
                    LogHelper.WriteLog("Run Vortex Wake Modol to Solve！", title: "[success]");
                    break;
                default:
                    LogHelper.ErrorLog($"AeroL.WakeMod={aeroL.WakeMod},No such AeroLoad Modol");
                    break;
            }

        }

        #endregion 非定常叶素动量理论初始化


        #region 叶素单元初始化
        /// <summary>
        /// 初始化叶片节点单元，该单元包含了叶片的气动力和其他的特性
        /// </summary>
        public static T_ALAeroBladeElement[,]? AL_IBladeAeroElement;

        public static void AL_INIBladeAeroElement(AeroL1 aeroL)
        {
            AL_IBladeAeroElement = AeroL_IO_Subs.ConvertAeroLToBladeElement(aeroL);
        }

        #endregion 叶素单元初始化


        #region Cp计算初始化
        /// <summary>
        /// Cp计算
        /// </summary>
        /// <param name="aeroL"></param>
        public static void AL_INISteadyBEMCp(AeroL1 aeroL)
        {
            AL_IAeroL1 = aeroL;
            LogHelper.WriteLog("Run Blade static Cp Solve！", title: "[success]");
            AL_INIBladeAeroElement(aeroL);
            //SteadyBEM.INISteadyBEM(AeroL);

            var res = new SteadyBEM(aeroL).CalculateCp(aeroL.Minlamda, aeroL.Maxlamda, aeroL.lamdaStep, ref AL_IBladeAeroElement, aeroL.Airfoil, aeroL.HubRad, aeroL.MinPitch, aeroL.MaxPitch, aeroL.PitchStep, aeroL.AirDens, aeroL.Bldnum);                //Console.WriteLine(res);
            OutFile outFile = new OutFile(aeroL.CpResultFilePath);
            string[] rowtitle = MathHelper.Range(aeroL.MinPitch, aeroL.MaxPitch, step: aeroL.PitchStep).Select(x => x.ToString()).ToArray();
            string[] columtitle = MathHelper.Range(aeroL.Minlamda, aeroL.Maxlamda, step: aeroL.lamdaStep).Select(x => x.ToString()).ToArray();
            outFile.WriteLine(Otherhelper.ConvertMatrixTitleToOutfile("Tsr[-]\\Cp[-]\\pitch[rad]", rowtitle, columtitle, res));
        }

        #endregion Cp计算初始化



        #region SteadyCurve 计算初始化

        public static void AL_INISteadyPowerCurve(AeroL1 aeroL)
        {
            AL_IAeroL1 = aeroL;
            LogHelper.WriteLog("Run the power, thrust etc. curves for a variable pitch blade.", title: "[success]");
            AL_INIBladeAeroElement(aeroL);
            T_ALStp_pow stp_Pow = new T_ALStp_pow(aeroL.MinWindSpeed, aeroL.MaxWindSpeed, aeroL.WindSpeedStep, aeroL.orig_pit, aeroL.ωmin, aeroL.opt_KW, aeroL.opt_rpm_rad, aeroL.η, aeroL.pitch_up, aeroL.pitch_down, aeroL.ifpitch, aeroL.Fixed_pitch, aeroL.Fixed_rotationalspeed);
            //SteadyBEM.INISteadyBEM(aeroL);
            var res = new SteadyBEM(aeroL).SteadyPowerCurveFunction(stp_Pow, AL_IBladeAeroElement, aeroL.Airfoil, aeroL.HubRad, aeroL.AirDens, aeroL.Bldnum, true);
            //Console.WriteLine(res);
            OutFile outFile = new OutFile(aeroL.PowerCurveResultFilePath);
            string[] rowtitle = { "风速v[m/s]", "风轮转速[rpm/min]", "叶尖速比λ[-]", "变桨角θ[rad]", "Cp[-]", "Ct[-]", "P_out[kw]" };
            string[] columtitle = MathHelper.Range(aeroL.MinWindSpeed, aeroL.MaxWindSpeed, step: aeroL.WindSpeedStep).Select(x => x.ToString()).ToArray();
            outFile.WriteLine(Otherhelper.ConvertMatrixTitleToOutfile("WindSpeed[m/s]/inf", rowtitle, columtitle, res));

        }

        public static void AL_INIStradyParkedLoads(AeroL1 aeroL)
        {
            AL_IAeroL1 = aeroL;
            LogHelper.WriteLog("Run the StradyParkedLoads etc. for a variable pitch blade.", title: "[success]");
            AL_INIBladeAeroElement(aeroL);

        }
        #endregion SteadyCurve 计算初始化
    }
}
