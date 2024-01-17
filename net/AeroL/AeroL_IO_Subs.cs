

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

using MathNet.Numerics.LinearAlgebra;
using OpenWECD.IO.IO;
using OpenWECD.IO.Log;
using OpenWECD.IO.Type;
using OpenWECD.IO.Interface1;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using MathNet.Numerics.Distributions;
namespace OpenWECD.AeroL
{
    /// <remarks>
    /// 处理AeroL输入文件的类型 读取NRELOffshrBsline5MW_Onshore_AeroDyn15文件
    /// </remarks>
    public class AeroL_IO_Subs : InterPutMainFile
    {
        #region 读取区域

        /// <remarks>
        /// 读取多个翼型文件并转换为Airfoil1结构体
        /// </remarks>
        /// <param name="filepath">翼型文件的路径数组，厚度从高到底eg"G:\2023\OpenWECD\5MW_Baseline\AeroData\Airfoils/Cylinder2.dat"
        /// "G:\2023\OpenWECD\5MW_Baseline\AeroData\Airfoils/DU40_A17.dat"
        /// "G:\2023\OpenWECD\5MW_Baseline\AeroData\Airfoils/DU35_A17.dat"
        /// "G:\2023\OpenWECD\5MW_Baseline\AeroData\Airfoils/DU30_A17.dat"
        /// "G:\2023\OpenWECD\5MW_Baseline\AeroData\Airfoils/DU25_A17.dat"
        /// "G:\2023\OpenWECD\5MW_Baseline\AeroData\Airfoils/DU21_A17.dat"
        /// "G:\2023\OpenWECD\5MW_Baseline\AeroData\Airfoils/NACA64_A17.dat":</param>
        /// <param name="angleFoil"></param>
        /// <returns></returns>
        private static Airfoil1 ReadAeroLAirfoil(string[] filepath, AeroL1 aeroL1, string angleFoil = "rad")
        {
            Airfoil1 res = new Airfoil1();
            double fac = angleFoil == "deg" ? 1 : Math.PI / 180;

            // 1. Read the number of airfoils
            res.Nfoil = filepath.Length;

            // 2. Read and store airfoil file names
            res.StringFoil = filepath;
            // 3. Add data to the given files
            if (File.ReadAllLines(filepath[0])[0].Trim().Split()[0].Trim() == "!")
            {
                for (int i = 0; i < res.Nfoil; i++)
                {
                    airfoil__temp airfoil__Temp = new airfoil__temp();
                    string[] fileLines = File.ReadAllLines(res.StringFoil[i]).Where(item => item != "").ToArray();
                    int fd(string temp) => Otherhelper.GetMatchingLineIndexes(fileLines, temp)[0];
                    airfoil__Temp.InterpOrd = Otherhelper.ParseLine<int>(fileLines, fd(" InterpOrd"), 1);
                    airfoil__Temp.NumAlf = Otherhelper.ParseLine<int>(fileLines, fd(" NumAlf"));
                    airfoil__Temp.DataSet = Otherhelper.ParseLine<Matrix<double>>(fileLines, fd(" NumAlf") + 3, num: airfoil__Temp.NumAlf);
                    airfoil__Temp.DataSet.SetColumn(0, airfoil__Temp.DataSet.Column(0) * fac);
                    if (aeroL1.AFAeroMod != 1)
                    {
                        try//配合V1.0.2增加的动态失速模型数据
                        {
                            int fd1(string temp) => Otherhelper.GetMatchingLineIndexes(fileLines, temp, false)[0];
                            switch (aeroL1.AFAeroMod)
                            {
                                case 2://BL
                                    break;
                                case 3:// OYG
                                    airfoil__Temp.T_f_OYG = Otherhelper.ParseLine<double>(fileLines, fd1(" T_f_OYG "));
                                    break;
                                case 4://IAG
                                    airfoil__Temp.A1 = Otherhelper.ParseLine<double>(fileLines, fd1(" A1 "));
                                    airfoil__Temp.A2 = Otherhelper.ParseLine<double>(fileLines, fd1(" A2 "));
                                    airfoil__Temp.b1 = Otherhelper.ParseLine<double>(fileLines, fd1(" b1 "));
                                    airfoil__Temp.b2 = Otherhelper.ParseLine<double>(fileLines, fd1(" b2 "));
                                    airfoil__Temp.ka = Otherhelper.ParseLine<double>(fileLines, fd1(" ka "));
                                    airfoil__Temp.T_p = Otherhelper.ParseLine<double>(fileLines, fd1(" T_p "));
                                    airfoil__Temp.T_f = Otherhelper.ParseLine<double>(fileLines, fd1(" T_f "));
                                    airfoil__Temp.T_V = Otherhelper.ParseLine<double>(fileLines, fd1(" T_V "));
                                    airfoil__Temp.T_VL = Otherhelper.ParseLine<double>(fileLines, fd1(" T_VL "));
                                    airfoil__Temp.K_v = Otherhelper.ParseLine<double>(fileLines, fd1(" K_v "));
                                    airfoil__Temp.K_Cf = Otherhelper.ParseLine<double>(fileLines, fd1(" K_Cf "));
                                    airfoil__Temp.T_Um = Otherhelper.ParseLine<double>(fileLines, fd1(" T_Um "));
                                    airfoil__Temp.T_Dm = Otherhelper.ParseLine<double>(fileLines, fd1(" T_Dm "));
                                    airfoil__Temp.M_IAG = Otherhelper.ParseLine<double>(fileLines, fd1(" M_IAG "));
                                    break;
                                case 5://GOR
                                    airfoil__Temp.A_M = Otherhelper.ParseLine<double>(fileLines, fd1(" A_M "));
                                    break;
                                case 6:// ATEF modal
                                    airfoil__Temp.Tf_ATEF = Otherhelper.ParseLine<double>(fileLines, fd1(" Tf_ATEF "));
                                    airfoil__Temp.Tp_ATEF = Otherhelper.ParseLine<double>(fileLines, fd1(" Tp_ATEF "));
                                    break;

                            }

                        }
                        catch
                        {
                            LogHelper.WarnLog($"翼型文件版本低于V1.0.2，aeroL1.AFAeroMod={aeroL1.AFAeroMod}计算动态失速，需更新文件");
                        }
                    }
                    res.list.Add(airfoil__Temp);
                }
            }
            else
            {
                for (int i = 0; i < res.Nfoil; i++)
                {
                    airfoil__temp airfoil__Temp = new airfoil__temp();
                    string[] fileLines = File.ReadAllLines(res.StringFoil[i]).Where(item => item != "").ToArray();
                    airfoil__Temp.DataSet = Otherhelper.ParseLine<Matrix<double>>(fileLines, 14, num: fileLines.Length - 14);//-1表示这行下面没有别的，都是矩阵，适用于这个类
                    airfoil__Temp.DataSet.SetColumn(0, airfoil__Temp.DataSet.Column(0) * fac);
                    res.list.Add(airfoil__Temp);
                }
                return res;
            }

            return res;
        }


        private static FreeWake ReadFreeWakeFoil(string pathfreewake)
        {
            return new FreeWake();
        }
        /// <remarks>
        /// 读取叶片和塔架的几何文件并创建Geometry1结构体
        /// </remarks>
        /// <param name="pathblade">叶片文件的路径 NRELOffshrBsline5MW_AeroDyn_blade.dat</param>
        /// <param name="angleFoil">默认为rad </param>
        /// <returns></returns>
        private static Geometry1 ReadAeroDynBladeGeometry(string pathblade = "", string angleFoil = "rad")
        {
            //LogHelper.WriteLog($"Reading GEOMETRY INPUT FILE....\nPATH: {path}");
            CheckError.Filexists(pathblade);
            Geometry1 res = new Geometry1();
            angleFoil = angleFoil.ToLower();
            double fac = 0;
            if (angleFoil == "deg")
            {
                fac = 1;
            }
            else if (angleFoil == "rad")
            {
                fac = Math.PI / 180.0;
            }
            else
            {
                LogHelper.ErrorLog("给定的angleFoil参数不正确，只可以指定\"rad\"或者\"deg\"");
            }

            // Read Blade geometry
            string[] infoLines = File.ReadAllLines(pathblade);
            int NumBlNds = Otherhelper.ParseLine<int>(infoLines, Otherhelper.GetMatchingLineIndexes(infoLines, "NumBlNds")[0]);
            //读取Blade几何
            res.Blade = Otherhelper.ParseLine<Matrix<double>>(infoLines, Otherhelper.GetMatchingLineIndexes(infoLines, "NumBlNds")[0] + 3, num: NumBlNds);
            res.Blade.SetColumn(4, res.Blade.Column(4).Multiply(fac));
            readgeometry1bld(ref res);
            return res;
        }

        /// <remarks>
        /// 这个函数的作用是对结构体其他变量进行赋值。
        /// </remarks>
        /// <param name="geo"></param>
        private static void readgeometry1bld(ref Geometry1 geo)
        {
            geo.NumBladeSection = geo.Blade.RowCount;
            geo.BlSpn = geo.Blade.Column(0);
            geo.BlCrvAC = geo.Blade.Column(1);
            geo.BlSwpAC = geo.Blade.Column(2);
            geo.BlCrvAng = geo.Blade.Column(3);
            geo.BlTwist = geo.Blade.Column(4);
            geo.BlChord = geo.Blade.Column(5);
            geo.BlAFID = ConvertMathNetDoubleVectorToIntArray(geo.Blade.Column(6) - 1);
            geo.PitchAxis = geo.Blade.Column(7);
        }




        private static void readgeometry1twr(ref Geometry1 geo)
        {
            geo.NumTowerSection = geo.Tower.RowCount;
            geo.TowerH = geo.Tower.Column(0);
            geo.TowerD = geo.Tower.Column(1);
            geo.TowerCd = geo.Tower.Column(2);
        }


        /// <remarks>
        /// 将一个double向量转换为一个int向量
        /// </remarks>
        /// <param name="doubleVector"></param>
        /// <returns></returns>
        private static int[] ConvertMathNetDoubleVectorToIntArray(Vector<double> doubleVector)
        {
            int[] intArray = doubleVector.Select(x => (int)x).ToArray();

            return intArray;
        }





        #region 读取AeroL 主文件
        /// <remarks>
        /// 将AeroL主文件读取为AeroL1 结构体
        /// </remarks>
        /// <param name="path"></param>
        /// <param name="AeroL_ApOfMb">0表示外部调用，1表示计算Cp,2表示计算功率曲线</param>
        /// <returns></returns>
        public static AeroL1 AeroL_AeroLFile(string path, int AeroL_ApOfMb = -1)
        {
            
            CheckError.Filexists(path);
            AeroL1 AeroL = new AeroL1();
            AeroL.AeroLfilepath = path;
            CheckError.Filexists(path);
            AeroL.AerolData = File.ReadAllLines(path);

            int fd(string temp, bool error = true, bool show = true) => Otherhelper.GetMatchingLineIndexes(AeroL.AerolData, temp, error, show)[0];
            //# 读取计算选项
            AeroL.ApOfMb = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" ApOfMb"), 0);

            //# ======  叶片信息   ===================================== [used only when ApOfMb=1 and 2]
            AeroL.HubRad = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" HubRad"));// - 最小的叶尖速比
            AeroL.Bldnum = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" Bldnum"));// - 最小的叶尖速比
            AeroL.TowerBsHt = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" TowerBsHt"), moren: 0.0);
            // 
            if (AeroL_ApOfMb != -1)
            {
                AeroL.ApOfMb = AeroL_ApOfMb;
            }
            //# General Options
            if (AeroL.ApOfMb == 0)//外部调用
            {
                //# General Options 
                AeroL.DTAero = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" DTAero"), 0.0125);
                AeroL.WakeMod = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" WakeMod"), 1);
                AeroL.AFAeroMod = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" AFAeroMod"), 1);
                AeroL.TwrPotent = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" TwrPotent"), 0);
                AeroL.TwrShadow = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" TwrShadow"), false);
                AeroL.TwrAero = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" TwrAero"), false);

                //# Environmental Conditions
                AeroL.AirDens = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" AirDens"));
                AeroL.KinVisc = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" KinVisc"));
                AeroL.SpdSound = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" SpdSound"));

                //# 叶片翼型信息
                AeroL.NumAFfiles = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" NumAFfiles"));
                List<string> list = new List<string>();
                for (int i = 0; i < AeroL.NumAFfiles; i++)
                {
                    string Airfoilpath = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(" AFNames") + i);
                    CheckError.Filexists(path, ref Airfoilpath, false, ".dat");
                    list.Add(Airfoilpath);
                }
                AeroL.airfoilpath = list.ToArray();
                AeroL.Airfoil = ReadAeroLAirfoil(AeroL.airfoilpath, AeroL);

                //# 叶片的气动几何
                AeroL.ADBlFile_1 = new string[AeroL.Bldnum];//初始化文件路径
                AeroL.Geometry = new Geometry1[AeroL.Bldnum];//初始化几何数组
                AeroL.PitchAxis = new Vector<double>[AeroL.Bldnum];
                AeroL.AeroCentJ1 = new Vector<double>[AeroL.Bldnum];
                AeroL.AeroCentJ2 = new Vector<double>[AeroL.Bldnum];
                for (int i = 0; i < AeroL.Bldnum; i++)
                {
                    //确认路径是否存在并成功读取
                    int icp = i + 1;
                    string filename = " ADBlFile(" + icp + ") ";
                    AeroL.ADBlFile_1[i] = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(filename));
                    CheckError.Filexists(path, ref AeroL.ADBlFile_1[i], false, ".dat");
                    //读取几何
                    AeroL.Geometry[i] = ReadAeroDynBladeGeometry(AeroL.ADBlFile_1[i]);
                    AeroL.NumBldNds = AeroL.Geometry[i].NumBladeSection;
                    //设置变桨轴线
                    AeroL.PitchAxis[i] = AeroL.Geometry[i].PitchAxis;
                    //设置气动中心
                    var TempDist = (0.25 - AeroL.PitchAxis[i]).PointwiseMultiply(AeroL.Geometry[i].BlChord);
                    var TempDistJ1 = TempDist.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist));
                    var TempDistJ2 = TempDist.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist));


                    AeroL.AeroCentJ1[i] = TempDistJ1.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist)) - TempDistJ2.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist));
                    AeroL.AeroCentJ2[i] = TempDistJ1.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist)) + TempDistJ2.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist));


                }

                //# 塔架气动几何
                AeroL.NumTwrNds = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" NumTwrNds"));
                for (int i = 0; i < AeroL.Bldnum; i++)
                {
                    Geometry1 geometry = new Geometry1();
                    geometry.Blade = AeroL.Geometry[i].Blade;
                    readgeometry1bld(ref geometry);
                    geometry.Tower = Otherhelper.ParseLine<Matrix<double>>(AeroL.AerolData, fd(" NumTwrNds") + 3, num: AeroL.NumTwrNds);
                    readgeometry1twr(ref geometry);
                    AeroL.Geometry[i] = geometry;
                }

                if (AeroL.WakeMod == 1)//使用 BEMT方法
                {
                    //# Blade-Element/Momentum Theory Options
                    AeroL.SkewMod = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" SkewMod"));
                    AeroL.SkewModFactor = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" SkewModFactor"), 15.0 / 3.02 * Math.PI);
                    AeroL.TipLoss = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" TipLoss"), true);
                    AeroL.HubLoss = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" HubLoss"), true);
                    AeroL.BemtError = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" BemtError"), 1E-15);
                    AeroL.MaxIter = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" MaxIter"), 200);
                }
                if (AeroL.AFAeroMod == 2)
                {
                    AeroL.UAMod = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" UAMod"));
                    AeroL.FLookup = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" FLookup"), true);
                }
                if (AeroL.WakeMod == 2)//使用 自由涡尾迹 方法
                {
                    AeroL.OLAFInputFileName = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(" OLAFInputFileName"));
                    CheckError.Filexists(path, ref AeroL.OLAFInputFileName, false, ".dat");
                    AeroL.FreeWake = ReadFreeWakeFoil(AeroL.OLAFInputFileName);
                }
            }
            if (AeroL.ApOfMb == 1)//计算叶片Cp
            {
                //# Environmental Conditions
                AeroL.AirDens = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" AirDens"));
                AeroL.KinVisc = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" KinVisc"));
                AeroL.SpdSound = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" SpdSound"));

                //# Blade-Element/Momentum Theory Options
                AeroL.SkewMod = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" SkewMod"));
                AeroL.SkewModFactor = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" SkewModFactor"), 15.0 / 32.0 * Math.PI);
                AeroL.TipLoss = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" TipLoss"), true);
                AeroL.HubLoss = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" HubLoss"), true);
                AeroL.BemtError = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" BemtError"), 1E-15);
                AeroL.MaxIter = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" MaxIter"), 300);

                //# 叶片翼型信息
                AeroL.NumAFfiles = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" NumAFfiles"));
                List<string> list = new List<string>();
                for (int i = 0; i < AeroL.NumAFfiles; i++)
                {
                    string Airfoilpath = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(" AFNames") + i);
                    CheckError.Filexists(path, ref Airfoilpath, false, ".dat");
                    list.Add(Airfoilpath);
                }
                AeroL.airfoilpath = list.ToArray();
                AeroL.Airfoil = ReadAeroLAirfoil(AeroL.airfoilpath, AeroL);

                //# 叶片的气动几何
                AeroL.ADBlFile_1 = new string[AeroL.Bldnum];//初始化文件路径
                AeroL.Geometry = new Geometry1[AeroL.Bldnum];//初始化几何数组
                AeroL.PitchAxis = new Vector<double>[AeroL.Bldnum];
                AeroL.AeroCentJ1 = new Vector<double>[AeroL.Bldnum];
                AeroL.AeroCentJ2 = new Vector<double>[AeroL.Bldnum];
                for (int i = 0; i < AeroL.Bldnum; i++)
                {
                    //确认路径是否存在并成功读取
                    int icp = i + 1;
                    string filename = " ADBlFile(" + icp + ") ";
                    AeroL.ADBlFile_1[i] = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(filename));
                    CheckError.Filexists(path, ref AeroL.ADBlFile_1[i], false, ".dat");
                    //读取几何
                    AeroL.Geometry[i] = ReadAeroDynBladeGeometry(AeroL.ADBlFile_1[i]);
                    AeroL.NumBldNds = AeroL.Geometry[i].NumBladeSection;
                    //设置变桨轴线
                    AeroL.PitchAxis[i] = AeroL.Geometry[i].PitchAxis;
                    //设置气动中心
                    var TempDist = (0.25 - AeroL.PitchAxis[i]).PointwiseMultiply(AeroL.Geometry[i].BlChord);
                    var TempDistJ1 = TempDist.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist));
                    var TempDistJ2 = TempDist.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist));


                    AeroL.AeroCentJ1[i] = TempDistJ1.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist)) - TempDistJ2.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist));
                    AeroL.AeroCentJ2[i] = TempDistJ1.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist)) + TempDistJ2.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist));


                }


                //# 其他参数
                AeroL.ADBlFile_1 = new string[AeroL.Bldnum];//初始化文件路径
                AeroL.Geometry = new Geometry1[AeroL.Bldnum];//初始化几何数组
                AeroL.PitchAxis = new Vector<double>[AeroL.Bldnum];
                AeroL.AeroCentJ1 = new Vector<double>[AeroL.Bldnum];
                AeroL.AeroCentJ2 = new Vector<double>[AeroL.Bldnum];
                for (int i = 0; i < AeroL.Bldnum; i++)
                {
                    //确认路径是否存在并成功读取
                    int icp = i + 1;
                    string filename = " ADBlFile(" + icp + ") ";
                    AeroL.ADBlFile_1[i] = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(filename));
                    CheckError.Filexists(path, ref AeroL.ADBlFile_1[i], false, ".dat");
                    //读取几何
                    AeroL.Geometry[i] = ReadAeroDynBladeGeometry(AeroL.ADBlFile_1[i]);
                    AeroL.NumBldNds = AeroL.Geometry[i].NumBladeSection;
                    //设置变桨轴线
                    AeroL.PitchAxis[i] = AeroL.Geometry[i].PitchAxis;
                    //设置气动中心
                    var TempDist = (0.25 - AeroL.PitchAxis[i]).PointwiseMultiply(AeroL.Geometry[i].BlChord);
                    var TempDistJ1 = TempDist.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist));
                    var TempDistJ2 = TempDist.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist));


                    AeroL.AeroCentJ1[i] = TempDistJ1.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist)) - TempDistJ2.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist));
                    AeroL.AeroCentJ2[i] = TempDistJ1.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist)) + TempDistJ2.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist));


                }
                //# 计算Cp曲线  ===================================== [used only when ApOfMb=1]
                AeroL.Minlamda = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" Minlamda"));// - 最小的叶尖速比
                AeroL.Maxlamda = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" Maxlamda"));// - 最大的叶尖速比
                AeroL.lamdaStep = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" lamdaStep"));// - 叶尖速比的间隔
                AeroL.MinPitch = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" MinPitch"));// - 最小的叶尖速比
                AeroL.MaxPitch = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" MaxPitch"));// - 最大的叶尖速比
                AeroL.PitchStep = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" PitchStep"));// - 叶尖速比的间隔
                AeroL.CpResultFilePath = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(" CpResultFilePath"));// - 计算Cp曲线的结果文件
                CheckError.Filexists(path, ref AeroL.CpResultFilePath, true, ".out", true);
            }
            if (AeroL.ApOfMb == 2)//计算叶片的功率，推力等变桨曲线表
            {
                //# Environmental Conditions
                AeroL.AirDens = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" AirDens"));
                AeroL.KinVisc = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" KinVisc"));
                AeroL.SpdSound = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" SpdSound"));

                //# Blade-Element/Momentum Theory Options
                AeroL.SkewMod = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" SkewMod"));
                AeroL.SkewModFactor = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" SkewModFactor"), 15.0 / 3.02 * Math.PI);
                AeroL.TipLoss = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" TipLoss"), true);
                AeroL.HubLoss = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" HubLoss"), true);
                AeroL.BemtError = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" BemtError"), 1E-15);
                AeroL.MaxIter = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" MaxIter"), 200);

                //# 叶片翼型信息
                AeroL.NumAFfiles = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" NumAFfiles"));
                List<string> list = new List<string>();
                for (int i = 0; i < AeroL.NumAFfiles; i++)
                {
                    string Airfoilpath = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(" AFNames") + i);
                    CheckError.Filexists(path, ref Airfoilpath, false, ".dat");
                    list.Add(Airfoilpath);
                }
                AeroL.airfoilpath = list.ToArray();
                AeroL.Airfoil = ReadAeroLAirfoil(AeroL.airfoilpath, AeroL);

                //# 叶片的气动几何
                AeroL.ADBlFile_1 = new string[AeroL.Bldnum];//初始化文件路径
                AeroL.Geometry = new Geometry1[AeroL.Bldnum];//初始化几何数组
                AeroL.PitchAxis = new Vector<double>[AeroL.Bldnum];
                AeroL.AeroCentJ1 = new Vector<double>[AeroL.Bldnum];
                AeroL.AeroCentJ2 = new Vector<double>[AeroL.Bldnum];
                for (int i = 0; i < AeroL.Bldnum; i++)
                {
                    //确认路径是否存在并成功读取
                    int icp = i + 1;
                    string filename = " ADBlFile(" + icp + ") ";
                    AeroL.ADBlFile_1[i] = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(filename));
                    CheckError.Filexists(path, ref AeroL.ADBlFile_1[i], false, ".dat");
                    //读取几何
                    AeroL.Geometry[i] = ReadAeroDynBladeGeometry(AeroL.ADBlFile_1[i]);
                    AeroL.NumBldNds = AeroL.Geometry[i].NumBladeSection;
                    //设置变桨轴线
                    AeroL.PitchAxis[i] = AeroL.Geometry[i].PitchAxis;
                    //设置气动中心
                    var TempDist = (0.25 - AeroL.PitchAxis[i]).PointwiseMultiply(AeroL.Geometry[i].BlChord);
                    var TempDistJ1 = TempDist.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist));
                    var TempDistJ2 = TempDist.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist));


                    AeroL.AeroCentJ1[i] = TempDistJ1.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist)) - TempDistJ2.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist));
                    AeroL.AeroCentJ2[i] = TempDistJ1.PointwiseMultiply(Vector<double>.Sin(AeroL.Geometry[i].BlTwist)) + TempDistJ2.PointwiseMultiply(Vector<double>.Cos(AeroL.Geometry[i].BlTwist));


                }

                //# 计算功率曲线  ===================================== [used only when ApOfMb=2]
                AeroL.MinWindSpeed = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" MinWindSpeed "));// - 最小风速
                AeroL.MaxWindSpeed = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" MaxWindSpeed "));// - 最大风速
                AeroL.WindSpeedStep = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" WindSpeedStep "));// - 风速间隔
                AeroL.orig_pit = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" orig_pit "));//- 初始桨距角[rad]
                AeroL.ωmin = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" ωmin "));//- 切入转速[rpm / min]
                AeroL.opt_KW = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" opt_KW "));// - 额定功率[kw] </ param >
                AeroL.opt_rpm_rad = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" opt_rpm_rad "));//- 额定转速[rpm / min]
                AeroL.η = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" η "));//- 发电机效率 %
                AeroL.pitch_up = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" pitch_up "));//- 最大桨距角[rad]
                AeroL.pitch_down = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" pitch_down "));// - 最小桨距角[rad]
                AeroL.ifpitch = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" ifpitch "));//                -是否变桨
                if (!AeroL.ifpitch)
                {
                    AeroL.Fixed_pitch = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" Fixed_pitch "));//- 固定桨距角[rad][used only when ifpitch = false]
                    AeroL.Fixed_rotationalspeed = Otherhelper.ParseLine<double>(AeroL.AerolData, fd(" Fixed_rotationalspeed "));//- 固定转速[rpm / min][used only when ifpitch = false]

                }
                AeroL.PowerCurveResultFilePath = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(" PowerCurveResultFilePath "));// - 计算Cp曲线的结果文件 - 计算功率曲线的结果文件目录
                CheckError.Filexists(path, ref AeroL.PowerCurveResultFilePath, true, ".out", true);

            }
            //# 输出文件的读取和设置
            AeroL.SumPrint = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" SumPrint "), true);
            if (AeroL.SumPrint)
            {
                AeroL.AfSpanput = Otherhelper.ParseLine<bool>(AeroL.AerolData, fd(" AfSpanput "));
                AeroL.SumPath = Otherhelper.ParseLine<string>(AeroL.AerolData, fd(" SumPath "));
                AeroL.BldOutSig = Otherhelper.ParseLine<int[]>(AeroL.AerolData, fd(" BldOutSig "), fg1: ',', moren: new int[] { 0 });
                string temp = AeroL.SumPath + "1.out";
                CheckError.Filexists(path, ref temp, true, ".out", true);
                AeroL.SumPath = temp.Replace("1.out", " ").Trim();
                //# 叶片输出节点
                AeroL.NBlOuts = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" NBlOuts "));
                if (AeroL.NBlOuts != 0)
                {
                    AeroL.BlOutNd = Otherhelper.ParseLine<int[]>(AeroL.AerolData, fd(" BlOutNd "), fg1: ',');
                    if (AeroL.NBlOuts > AeroL.BlOutNd.Length)
                    {
                        LogHelper.ErrorLog($"AeroL.BlOutNd>AeroL.BlOutNd.Length");
                    }
                    AeroL.BlOutNd = AeroL.BlOutNd[0..AeroL.NBlOuts];
                }
                //# 塔架输出节点
                AeroL.NTwOuts = Otherhelper.ParseLine<int>(AeroL.AerolData, fd(" NTwOuts "));
                if (AeroL.NTwOuts != 0)
                {
                    AeroL.TwOutNd = Otherhelper.ParseLine<int[]>(AeroL.AerolData, fd(" TwOutNd "), fg1: ',');
                    if (AeroL.NTwOuts > AeroL.TwOutNd.Length)
                    {
                        LogHelper.ErrorLog($"AeroL.NTwOuts > AeroL.TwOutNd.Length");
                    }
                    AeroL.TwOutNd = AeroL.TwOutNd[0..AeroL.NTwOuts];
                }
                AeroL.Outputs_OutList = Otherhelper.ReadOutputWord(AeroL.AerolData, fd(" OutList ") + 1, true);
            }
            return AeroL;
        }

        /// <summary>
        /// 将AeroL1 文件当中的几何数据读取为一个叶片单元结构体的数组，每一列对应一个叶片，每一行代表一个相同的叶片截面位置
        /// </summary>
        /// <param name="AeroL"></param>
        /// <returns></returns>
        public static T_ALAeroBladeElement[,] ConvertAeroLToBladeElement(AeroL1 AeroL)
        {
            var res = new T_ALAeroBladeElement[AeroL.NumBldNds, AeroL.Bldnum];
            for (int i = 0; i < AeroL.Bldnum; i++)
            {
                for (int j = 0; j < AeroL.NumBldNds; j++)
                {
                    //res[j, i] = new T_ALAeroBladeElement();
                    res[j, i].SRHub = AeroL.HubRad;
                    res[j, i].SBlspan = AeroL.Geometry[i].BlSpn[j] + AeroL.HubRad;
                    res[j, i].SChord = AeroL.Geometry[i].BlChord[j];
                    res[j, i].SFoilNum = AeroL.Geometry[i].BlAFID[j];
                    res[j, i].STwist = AeroL.Geometry[i].BlTwist[j];
                    res[j, i].SSolid = (AeroL.Bldnum / 2.0 / Math.PI) * res[j, i].SChord;
                    res[j, i].SAeroCentJ1 = AeroL.AeroCentJ1[i][j];
                    res[j, i].SAeroCentJ2 = AeroL.AeroCentJ2[i][j];
                    res[j, i].SPitchAxis = AeroL.PitchAxis[i][j];
                    res[j, i].STipRad = AeroL.Geometry[i].BlSpn.Max() + AeroL.HubRad;
                }
            }
            return res;
        }


        #endregion 读取AeroL 主文件

        #endregion 读取区域


        #region 写文件区域
        public void putfile(string Dir)
        {
            string[] AeroLmainfile5MW = new string[]
            {
              $"  ------- OpenWECD.AeroL  主文件 v{Otherhelper.GetCurrentVersion()}.* This software design and writen By 赵子祯 ------------------------------------------------",
$"NREL 5.0 MW offshore baseline aerodynamic input properties.",
"======  计算选项   ===================================================================================",
"            0       ApOfMb           - 计算选项{0=外部调用将读取塔架叶片等模型参数，1=计算叶片Cp,2=计算叶片的功率，推力等变桨曲线表}",
"======  General Options  ==========================================================================",
"\"default\"     DTAero             - Time interval for aerodynamic calculations { or \"default\"}(s)",
"         1    WakeMod - Type of wake/ induction model(switch) { 0 = none, 1 = BEMT, 2 = FreeWake}",
"         1   AFAeroMod - Type of blade airfoil aerodynamics model(switch) { 1 = steady model, 2 = Beddoes - Leishman unsteady model}(没有完成)",
"         1   TwrPotent - Type tower influence on wind based on potential flow around the tower(switch) { 0 = none, 1 = baseline potential flow, 2 = potential flow with Bak correction}",
"          False TwrShadow          -Calculate tower influence on wind based on downstream tower shadow ? (flag)",
"            False TwrAero -Calculate tower aerodynamic loads ? (flag)",
"            ====== Environmental Conditions ===================================================================",
 "                 1.225   AirDens - Air density(kg / m ^ 3)",
 " 1.464E-05   KinVisc - Kinematic air viscosity(m^2 / s)",
 "       335   SpdSound - Speed of sound(m/ s)",
"====== Blade - Element / Momentum Theory Options  ====================================================== [used only when WakeMod = 1]",
"2                      SkewMod - Type of skewed-wake correction model(switch) { 1 = uncoupled, 2 = Pitt / Peters, 3 = coupled}[unused when WakeMod= 1 or 2]",
"default                SkewModFactor - Constant used in Pitt / Peters skewed wake model { or \"default\" is 15 / 32 * pi} (-)[used only when SkewMod = 2; unused when WakeMod = 0 or 3]",
"True TipLoss            -Use the Prandtl tip - loss model ? (flag)[used only when WakeMod = 1]",
"True          HubLoss - Use the Prandtl hub - loss model ? (flag)[used only when WakeMod = 1]",
"1E-15         BemtError - 叶素动量理论的迭代误差要求[used only when WakeMod = 1]",
"        300   MaxIter - Maximum number of iteration steps(-) [used only when WakeMod = 1]",
"====== Beddoes - Leishman Unsteady Airfoil Aerodynamics Options  ===================================== [used only when AFAeroMod = 2] ",
"          3   UAMod - Unsteady Aero Model Switch(switch) { 1 = Baseline model(Original), 2 = Gonzalez s variant(changes in Cn, Cc, Cm), 3 = Minemma / Pierce variant(changes in Cc and Cm)}[used only when AFAeroMod= 2] ",
"True FLookup            -Flag to indicate whether a lookup for f' will be calculated (TRUE) or whether best-fit exponential equations will be used (FALSE); if FALSE S1-S4 must be provided in airfoil input files (flag) [used only when AFAeroMod=2] ",
"====== OLAF-- cOnvecting LAgrangian Filaments(Free Vortex Wake) Theory Options ================== [used only when WakeMod = 2] ",
"\"../IEA-15-240-RWT-OLAF/IEA-15-240-RWT_OLAF.dat\"  OLAFInputFileName - Input file for OLAF[used only when WakeMod = 2] ",
"                ====== Airfoil Information ========================================================================= ",
"          8   NumAFfiles - Number of airfoil files used(-) ",
"\"./Airfoils/Cylinder1.dat\"    AFNames - Airfoil file names(NumAFfiles lines)(quoted strings)#填写完整的路径,既绝对路径 ",
"\"./Airfoils/Cylinder2.dat\"",
"\"./Airfoils/DU40_A17.dat\"",
"\"./Airfoils/DU35_A17.dat\"",
"\"./Airfoils/DU30_A17.dat\"",
"\"./Airfoils/DU25_A17.dat\"",
"\"./Airfoils/DU21_A17.dat\"",
"\"./Airfoils/NACA64_A17.dat\"",
"====== Rotor / Blade Properties =====================================================================",
"True          UseBlCm - Include aerodynamic pitching moment in calculations ? (flag)",
"\"./demo/NRELOffshrBsline5MW_AeroL_blade.dat\"    ADBlFile(1) - Name of file containing distributed aerodynamic properties for Blade #1 (-)",
"\"./demo/NRELOffshrBsline5MW_AeroL_blade.dat\"    ADBlFile(2) - Name of file containing distributed aerodynamic properties for Blade #2 (-) [unused if NumBl < 2]",
"\"./demo/NRELOffshrBsline5MW_AeroL_blade.dat\"    ADBlFile(3) - Name of file containing distributed aerodynamic properties for Blade #3 (-) [unused if NumBl < 3]",
"====== Tower Influence and Aerodynamics ============================================================= [used only when TwrPotent != 0, TwrShadow = True, or TwrAero = True] ",
"     10   TowerBsHt - 塔基开始的基础高度在水面上的位置（陆上风力机设置为0，海上按照ElastoDyn.TowerBsHt的高度进行设置）",
"         11   NumTwrNds - Number of tower nodes used in the analysis(-)[used only when TwrPotent /= 0, TwrShadow = True, or TwrAero = True]",
"TwrElev        TwrDiam        TwrCd ",
"   (m)          (m)            (-) ",
"0.000000E+00    6.000000E+00    1.000000E+00 ",
"8.760000E+00    6.000000E+00    1.000000E+00 ",
"1.752000E+01    6.000000E+00    1.000000E+00 ",
"2.628000E+01    6.000000E+00    1.000000E+00 ",
"3.504000E+01    6.000000E+00    1.000000E+00 ",
"4.380000E+01    6.000000E+00    1.000000E+00 ",
"5.256000E+01    6.000000E+00    1.000000E+00 ",
"6.132000E+01    6.000000E+00    1.000000E+00 ",
"7.008000E+01    6.000000E+00    1.000000E+00 ",
"7.884000E+01    6.000000E+00    1.000000E+00 ",
"8.760000E+01    6.000000E+00    1.000000E+00 ",
"====== 风力机叶片信息 ===================================== [used only when ApOfMb = 0，1 and 2] ",
"1.5           HubRad - 轮毂半径  ",
"3             Bldnum - 叶片数量  ",
"====== 计算Cp曲线 ===================================== [used only when ApOfMb = 1] ",
"2             Minlamda - 最小的叶尖速比 ",
"20            Maxlamda - 最大的叶尖速比",
"0.5           lamdaStep - 叶尖速比的间隔",
"- 0.1          MinPitch - 最小的变桨角[rad]",
"1.57          MaxPitch - 最大的变桨角[rad]",
"0.01          PitchStep - 变桨角的间隔[rad]",
"\"./Result/test_AeroL_cp.out\"                     CpResultFilePath - 计算Cp曲线的结果文件 ",
"                            ====== 计算功率曲线 ===================================== [used only when ApOfMb = 2] ",
"2             MinWindSpeed - 最小风速[m / s]",
"25            MaxWindSpeed - 最大风速[m / s]",
"0.01          WindSpeedStep - 风速间隔[m / s]",
"0.0           orig_pit - 初始桨距角[rad]",
"3             ωmin - 切入转速[rpm / min]",
"5000          opt_KW - 额定功率[kw] </ param >",
"13            opt_rpm_rad - 额定转速[rpm / min]",
"100           η - 发电机效率 %",
"1.57          pitch_up - 最大桨距角[rad]",
"0.0           pitch_down - 最小桨距角[rad]",
"True          ifpitch - 是否变桨",
"0.0           Fixed_pitch - 固定桨距角[rad][used only when ifpitch = false]",
"0.0           Fixed_rotationalspeed - 固定转速[rpm / min][used only when ifpitch = false]",
"\"./Result/test_AeroL_PowerCurve.out\"                     PowerCurveResultFilePath - 计算功率曲线的结果文件目录 ",
"====== Outputs ====================================================================================",
"True          SumPrint - Generate a summary file listing input options and interpolated properties to \" < rootname>.AD.sum\" ? (flag)",
                "         0   NBlOuts - Number of blade node outputs[0 - 9](-) ",
                "         1, 9, 19    BlOutNd - Blade nodes whose values will be output(-)",
                "          0   NTwOuts - Number of tower node outputs[0 - 9](-)",
                "         1, 2, 6    TwOutNd - Tower nodes whose values will be output(-)",
                "                   OutList - The next line(s) contains a list of output parameters.See OutListParameters.xlsx for a listing of available output channels, (-)",
                "END of input file(the word \"END\" must appear in the first 3 columns of this last OutList line)",
                "-------------------------------------------------------------------------------------- -"
            };

            string[] AerofoilDu40demoV1_0_1 = new string[]
            {
            $"! ------------ AirfoilInfo v{Otherhelper.GetCurrentVersion()} Input File ---------------------------------- ",
"! DU40 airfoil with an aspect ratio of 17.  Original -180 to 180deg Cl, Cd, and Cm versus AOA data taken from Appendix A of DOWEC document 10046_009.pdf (numerical values obtained from Koert Lindenburg of ECN).",
"! Cl and Cd values corrected for rotational stall delay and Cd values corrected using the Viterna method for 0 to 90deg AOA by Jason Jonkman using AirfoilPrep_v2p0.xls.",
"!note that this file uses Marshall Buhl's new input file processing; start all comment lines with ! ",
"!------------------------------------------------------------------------------ ",
"\"DEFAULT\"     InterpOrd! Interpolation order to use for quasi - steady table lookup { 1 = linear; 3 = cubic spline; \"default\"} \\",
            "            [default = 1] ",
"          1   NonDimArea! The non-dimensional area of the airfoil(area/ chord ^ 2) (set to 1.0 if unsure or unneeded)",
"@\"DU40_A17_coords.txt\"    NumCoords! The number of coordinates in the airfoil shape file.  Set to zero if coordinates not included.",
"          1   NumTabs! Number of airfoil tables in this file.",
"!------------------------------------------------------------------------------",
"! data for table 1",
"!------------------------------------------------------------------------------ ",
"       0.75   Re! Reynolds number in millions ",
"          0   UserProp! User property(control) setting \\",
"         True          InclUAdata! Is unsteady aerodynamics data included in this table ? If TRUE, then include 30 UA coefficients below this line",
"            !........................................-3.2   alpha0! 0 - lift angle of attack, depends on airfoil.",
"          9   alpha1! Angle of attack at f = 0.7, (approximately the stall angle) for AOA > alpha0. (deg)",
"         - 9   alpha2! Angle of attack at f = 0.7, (approximately the stall angle) for AOA < alpha0. (deg)",
"          1   eta_e! Recovery factor in the range[0.85 - 0.95] used only for UAMOD = 1, it is set to 1 in the code when flookup = True. (-)",
"        7.4888   C_nalpha! Slope of the 2D normal force coefficient curve. (1 / rad)",
"          3   T_f0! Initial value of the time constant associated with Df in the expression of Df and f''. [default = 3]",
"          6   T_V0! Initial value of the time constant associated with the vortex lift decay process; it is used in the expression of Cvn.It depends on Re, M, and airfoil class. [default = 6]",
"        1.7   T_p               ! Boundary-layer,leading edge pressure gradient time constant in the expression of Dp.It should be tuned based on airfoil experimental data. [default = 1.7]",
"         11   T_VL              ! Initial value of the time constant associated with the vortex advection process; it represents the non-dimensional time in semi-chords, needed for a vortex to travel from LE to trailing edge(TE); it is used in the expression of Cvn.It depends on Re, M (weakly), and airfoil. [valid range = 6 - 13, default = 11]",
"       0.14   b1                ! Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.14]",
"       0.53   b2                ! Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.53]",
"          5   b5                ! Constant in the expression of K'''_q,Cm_q^nc, and k_m,q.  [from  experimental results, defaults to 5]",
"        0.3   A1                ! Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.3]",
"        0.7   A2                ! Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]",
"          1   A5                ! Constant in the expression of K'''_q,Cm_q^nc, and k_m,q. [from experimental results, defaults to 1]",
"          0   S1                ! Constant in the f curve best-fit for alpha0<=AOA<=alpha1; by definition it depends on the airfoil. [ignored if UAMod<>1]",
"          0   S2                ! Constant in the f curve best-fit for         AOA> alpha1; by definition it depends on the airfoil. [ignored if UAMod<>1]",
"          0   S3                ! Constant in the f curve best-fit for alpha2<=AOA<alpha0; by definition it depends on the airfoil. [ignored if UAMod<>1]",
"          0   S4                ! Constant in the f curve best-fit for         AOA<alpha2; by definition it depends on the airfoil. [ignored if UAMod<>1]",
"      1.3519   Cn1               ! Critical value of C0n at leading edge separation.It should be extracted from airfoil data at a given Mach and Reynolds number.It can be calculated from the static value of Cn at either the break in the pitching moment or the loss of chord force at the onset of stall.It is close to the condition of maximum lift of the airfoil at low Mach numbers.",
"     - 0.3226   Cn2               ! As Cn1 for negative AOAs.",
"       0.19   St_sh             ! Strouhal's shedding frequency constant.  [default = 0.19]",
"       0.03   Cd0               ! 2D drag coefficient value at 0-lift.",
"      - 0.05   Cm0               ! 2D pitching moment coefficient about 1/4-chord location, at 0-lift, positive if nose up. [If the aerodynamics coefficients table does not include a column for Cm, this needs to be set to 0.0]",
"          0   k0                ! Constant in the \\hat(x) _cp curve best-fit; = (\\hat(x)_AC-0.25).  [ignored if UAMod<>1]",
"          0   k1                ! Constant in the \\hat(x)_cp curve best-fit.  [ignored if UAMod<>1]",
"          0   k2                ! Constant in the \\hat(x)_cp curve best-fit.  [ignored if UAMod<>1]",
"          0   k3                ! Constant in the \\hat(x)_cp curve best-fit.  [ignored if UAMod<>1]",
"          0   k1_hat            ! Constant in the expression of Cc due to leading edge vortex effects.  [ignored if UAMod<>1]",
"        0.2   x_cp_bar          ! Constant in the expression of \\hat(x) _cp^v. [ignored if UAMod<>1, default = 0.2]",
"      \"DEFAULT\"     UACutout          ! Angle of attack above which unsteady aerodynamics are disabled (deg). [Specifying the string \"Default\" sets UACutout to 45 degrees]",
"       \"DEFAULT\"     filtCutOff        ! Cut-off frequency(-3 dB corner frequency) for low-pass filtering the AoA input to UA, as well as the 1st and 2nd derivatives(Hz) [default = 20]",
"     !........................................",
"     ! Table of aerodynamics coefficients",
"        12   NumAlf            ! Number of data lines in the following table",
"     !    Alpha Cl      Cd Cm",
"     !    (deg) (-)     (-)       (-)",
"  - 180.00    0.000   0.0602   0.0000",
"  - 175.00    0.218   0.0699   0.0934",
"  - 170.00    0.397   0.1107   0.1697",
"  - 160.00    0.642   0.3045   0.2813",
"  - 155.00    0.715   0.4179   0.3208",
"  - 150.00    0.757   0.5355   0.3516",
"   150.00   -0.782   0.5532  -0.3863",
"   155.00   -0.739   0.4318  -0.3521",
"   160.00   -0.664   0.3147  -0.3085",
"   170.00   -0.410   0.1144  -0.1858",
"   175.00   -0.226   0.0702  -0.1022",
"   180.00    0.000   0.0602   0.0000",
            };

            string[] AerofoilDu35demoV1_0_2 = new string[]
            {
"            AeroL.Airfoil V1.0.2 Powered by 赵子祯 ",
"这个文件是适配OpenWECD 1.0.2及其之上的版本，支持OYE,IAG，GOR,ATEF 以及BL模型，模型的开启与否在AeroL 主文件当中 ",
"===========  OYG modal  ============ ",
"8      T_f_OYG  - Time constant",
"===========  IAG modal  ============",
"0.3    A1   - Constant in the expression of phi_alpha^c and phi_q^c.  This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.3] ",
"0.7    A2   - Constant in the expression of phi_alpha^c and phi_q^c.  This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]",
"0.7    b1   - Constant in the expression of phi_alpha^c and phi_q^c.  This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]",
"0.53   b2   - Constant in the expression of phi_alpha^c and phi_q^c.  This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.53]",
"0.75   ka    -",
"1.7    T_p  - Boundary-layer,leading edge pressure gradient time constant in the expression of Dp. It should be tuned based on airfoil experimental data. [default = 1.7]",
"3      T_f  - Initial value of the time constant associated with Df in the expression of Df and f''. [default = 3]",
"6      T_V  - Initial value of the time constant associated with the vortex lift decay process; it is used in the expression of Cvn. It depends on Re,M, and airfoil class. [default = 6]",
"11     T_VL - Initial value of the time constant associated with the vortex advection process; it represents the non-dimensional time in semi-chords, needed for a vortex to travel from LE to trailing edge (TE); it is used in the expression of Cvn. It depends on Re, M (weakly), and airfoil. [valid range = 6 - 13, default = 6]",
"0.2    K_v",
"0.1    K_Cf",
"1.5    T_Um",
"1.5    T_Dm",
"0      M_IAG",
"===========  GOR modal  ============",
"6      A_M",
"===========  BL modal  =============",
"===========  ATEF modal  ===========",
"3      Tf_ATEF",
"1.7    Tp_ATEF",
"===========  Airfoil data  ===========",
" 1    InterpOrd  - Interpolation order to use for quasi-steady table lookup {1=linear; 2=cubic spline; \"default\"} [default=1]",
"9   NumAlf     - Number of data lines in the following table",
"4     Numcolumn     - Number of data Column",
"      Alpha      Cl      Cd        Cm",
"     (deg)      (-)     (-)       (-)",
"   -180.00    0.000   0.0407   0.0000",
"   -175.00    0.223   0.0507   0.0937",
"   -170.00    0.405   0.1055   0.1702",
"   -160.00    0.658   0.2982   0.2819",
"   -155.00    0.733   0.4121   0.3213",
"    160.00   -0.677   0.3066  -0.3054",
"    170.00   -0.417   0.1085  -0.1842",
"    175.00   -0.229   0.0510  -0.1013",
"    180.00    0.000   0.0407   0.0000"





            };
            string[] BladeGeoShape = new string[]
            {
            $"! ------------ Blade Geometry v{Otherhelper.GetCurrentVersion()} Input File ---------------------------------- ",
"            NREL 5.0 MW offshore baseline aerodynamic blade input properties; note that we need to add the aerodynamic center to this file",
"====== Blade Properties =================================================================",
"         19   NumBlNds - Number of blade nodes used in the analysis(-)",
"  BlSpn      BlCrvAC      BlSwpAC   BlCrvAng    BlTwist     BlChord  BlAFID   Pitch",
"   (m)        (m)           (m)       (deg)       (deg)       (m)     (-)      (-)",
"1.000E-05   0.000E+00   0.000E+00   0.000E+00   1.331E+01   3.542E+00  1   2.500E-01",
"1.367E+00 - 8.153E-04 - 3.447E-03  0.000E+00   1.331E+01   3.542E+00   1   2.508E-01",
"4.100E+00 - 2.484E-02 - 1.050E-01  0.000E+00   1.331E+01   3.854E+00   1   2.782E-01",
"6.833E+00 - 5.947E-02 - 2.514E-01  0.000E+00   1.331E+01   4.167E+00   2   3.117E-01",
"1.025E+01 - 1.091E-01 - 4.612E-01  0.000E+00   1.331E+01   4.557E+00   3   3.536E-01",
"1.435E+01 - 1.157E-01 - 5.699E-01  0.000E+00   1.148E+01   4.652E+00   4   3.750E-01",
"1.845E+01 - 9.832E-02 - 5.485E-01  0.000E+00   1.016E+01   4.458E+00   4   3.750E-01",
"2.255E+01 - 8.319E-02 - 5.246E-01  0.000E+00   9.011E+00   4.249E+00   5   3.750E-01",
"2.665E+01 - 6.793E-02 - 4.962E-01  0.000E+00   7.795E+00   4.007E+00   6   3.750E-01",
"3.075E+01 - 5.339E-02 - 4.654E-01  0.000E+00   6.544E+00   3.748E+00   6   3.750E-01",
"3.485E+01 - 4.090E-02 - 4.358E-01  0.000E+00   5.361E+00   3.502E+00   7   3.750E-01",
"3.895E+01 - 2.972E-02 - 4.059E-01  0.000E+00   4.188E+00   3.256E+00   7   3.750E-01",
"4.305E+01 - 2.051E-02 - 3.757E-01  0.000E+00   3.125E+00   3.010E+00   8   3.750E-01",
"4.715E+01 - 1.398E-02 - 3.452E-01  0.000E+00   2.319E+00   2.764E+00   8   3.750E-01",
"5.125E+01 - 8.382E-03 - 3.146E-01  0.000E+00   1.526E+00   2.518E+00   8   3.750E-01",
"5.467E+01 - 4.355E-03 - 2.891E-01  0.000E+00   8.630E-01   2.313E+00   8   3.750E-01",
"5.740E+01 - 1.684E-03 - 2.607E-01  0.000E+00   3.700E-01   2.086E+00   8   3.750E-01",
"6.013E+01 - 3.282E-04 - 1.774E-01  0.000E+00   1.060E-01   1.419E+00   8   3.750E-01",
"6.150E+01 - 3.282E-04 - 1.774E-01  0.000E+00   1.060E-01   1.419E+00   8   3.750E-01"
            };
        }

        #endregion 写文件区域


        #region 输出的变量定义
        public static Dictionary<string, string> AL_OutPar = new()
        {

            { "AxInd","[-]" },
            { "TnInd","[-]" },
            { "Alpha","[deg]" },
            { "Theta","[deg]" },
            { "Phi","[deg]" },
            { "Cl","[-]" },
            { "Cd","[-]" },
            { "Cm","[-]" },
            { "Cx","[-]" },
            { "Cy","[-]" },
            { "Vx","[m/s]" },
            { "Vy","[m/s]" },
            { "Fx","[N]" },
            { "Fy","[N]" },
            { "Vindx","[m/s]" },
            { "Vindy","[m/s]" },
            { "Mz","[Nm]" },
            { "Mx","[Nm]" },
            { "Thrust","[N]" },
            { "Torque","[Nm]" }

        };

        #endregion 输出的变量定义


        #region IO 输出流
        public class AL_PutBemEle : MutiPutData, I_IO_INIPutBemEle, I_ALPUTini
        {
            private AeroL1 AeroL;
            public Dictionary<int, Dictionary<int, OutFile>> directory = new Dictionary<int, Dictionary<int, OutFile>>();
            public void AL_PutINI(AeroL1 aeroL1)
            {

                put = aeroL1.SumPrint;
                if (aeroL1.SumPrint)
                {
                    BLDNUM = aeroL1.BldOutSig.Length;
                    SECNUM = aeroL1.BlOutNd.Length;
                    BLDNode = aeroL1.BldOutSig;
                    SECNode = aeroL1.BlOutNd;
                    AeroL = aeroL1;
                    CheckRes();//检查输入文件的设置是否有错误
                }
            }
            /// <summary>
            /// 生成文件流，调用之前请先调用AL_INIPutEle(AeroL1 aeroL1)
            /// </summary>
            public void PutInI(string title = "")
            {
                if (AeroL.SumPrint)
                {
                    if (AeroL.AfSpanput)//默认节点输出
                    {
                        for (int i = 0; i < AeroL.BldOutSig.Length; i++)
                        {
                            Dictionary<int, OutFile> blade = new Dictionary<int, OutFile>();
                            for (int j = 0; j < AeroL.BlOutNd.Length; j++)
                            {

                                if (title == "")
                                {
                                    string path = $"./AeroL_Bld_{AeroL.BldOutSig[i]}_Node_" + AeroL.BlOutNd[j] + ".AL.out";
                                    blade.Add(AeroL.BlOutNd[j], new OutFile(Path.Combine(AeroL.SumPath, path)));

                                }
                                else
                                {
                                    string path = $"./AeroL_{title}_Bld{AeroL.BldOutSig[i]}_Node_" + AeroL.BlOutNd[j] + ".AL.out";
                                    blade.Add(AeroL.BlOutNd[j], new OutFile(Path.Combine(AeroL.SumPath, path)));

                                }
                            }
                            directory.Add(AeroL.BldOutSig[i], blade);
                        }

                    }
                    else//变量输出
                    {
                        LogHelper.ErrorLog("Error HS0000001 ! notSupport");
                        //for (int i = 0; i < AeroL.Outputs_OutList.Length; i++)
                        //{
                        //    string path = "./AeroL_Blade_Par_" + AeroL.Outputs_OutList[i] + ".AL.out";
                        //    directory.Add(AeroL.Outputs_OutList[i], new OutFile(Path.Combine(AeroL.SumPath, path)));
                        //}
                        //OutFile.WriteLineALL($"OPenWECD旗下产品 OpenHast By TGTeam ! 当前文件版本为 {ProjectVision}");
                        //OutFile.WriteLineALL($"如果有相关问题请登录官方网站{url} 寻求帮助");
                        //if (AeroL.ApOfMb == 0)//Cp计算
                        //{

                        //}
                    }
                }
            }
            public double GetRes(string par, int NumSec, int BldBum, int i_t = -1, double t = -1, double pars = -1)
            {
                switch (par)
                {
                    case "AxInd":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].Da;
                    case "TnInd":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].Daa;
                    case "Alpha":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DAOA * RadTodeg;
                    case "Theta":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DTheta * RadTodeg;
                    case "Phi":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].Dphi * RadTodeg;
                    case "Cl":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DCl;
                    case "Cd":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DCd;
                    case "Cm":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DCm;
                    case "Cx":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DCx;
                    case "Cy":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DCy;
                    case "Vx":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DVx;
                    case "Vy":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DVy;
                    case "Fx":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DPx;
                    case "Fy":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DPy;
                    case "Vindx":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].Vindx;
                    case "Vindy":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].Vindy;
                    case "Mz":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DMz;
                    case "Mx":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].DMx;
                    case "Thrust":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].Thrust;
                    case "Torque":
                        return AeroL_INI.AL_IBladeAeroElement[NumSec, BldBum].Torque;


                    default:
                        LogHelper.ErrorLog($"{par} not Support!,Please Visit OutputParList or {url} ");
                        return double.NaN;
                }
            }
            public void CheckRes()
            {
                for (int i = 0; i < AeroL.Outputs_OutList.Length; i++)
                {
                    if (!AeroL_IO_Subs.AL_OutPar.ContainsKey(AeroL.Outputs_OutList[i]))
                    {
                        LogHelper.ErrorLog($"{AeroL.Outputs_OutList[i]} not Support!,Please Visit OutputParList or {url} ");
                    }
                }
            }
            public string GetResUnit(string par)
            {
                return AeroL_IO_Subs.AL_OutPar[par];
            }

            public void WriteBEMEleValue()
            {
                if (put)
                {
                    for (int i = 0; i < AeroL.BldOutSig.Length; i++)
                    {
                        for (int j = 0; j < AeroL.BlOutNd.Length; j++)
                        {
                            for (int k = 0; k < AeroL.Outputs_OutList.Length; k++)
                            {
                                // directory[i][AeroL.BlOutNd[j]].Write(GetRes(AeroL.Outputs_OutList[k], j, i));//0代表0号叶片
                                directory[BLDNode[i]][SECNode[j]].Write(GetRes(AeroL.Outputs_OutList[k], SECNode[j], BLDNode[i]));//0代表0号叶片
                            }
                        }

                    }
                }


            }

        }

        #endregion IO 输出流


    }

}

