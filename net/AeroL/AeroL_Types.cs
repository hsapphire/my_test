

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
using OpenWECD.IO.Type;
using System;
using System.Collections;
using System.Collections.Generic;
namespace OpenWECD.AeroL
{

    /// <summary>
    /// 静态功率曲线结构体
    /// </summary>
    public struct T_ALStp_pow
    {
        /// <summary>
        /// 最小风速[m/s]
        /// </summary>
        public double min_windv; // =3.0

        /// <summary>
        /// 最大风速[m/s]
        /// </summary>
        public double max_windv; // =25.0

        /// <summary>
        /// 风速间隔 [m/s]
        /// </summary>
        public double wind_step; // =0.1

        /// <summary>
        /// 额度桨距角[rad]
        /// </summary>
        public double orig_pit; // =0.0

        /// <summary>
        /// 切入转速[rpm/min]
        /// </summary>
        public double ωmin; // =1.0

        /// <summary>
        /// 额定功率[kw]
        /// </summary>
        public double opt_KW; // =0.0

        /// <summary>
        /// 额定转速[rpm/min]
        /// </summary>
        public double opt_rpm_rad; // =0.0

        /// <summary>
        /// 发电机效率%
        /// </summary>
        public double η; // =1.0

        /// <summary>
        /// 最大桨距角[rad]
        /// </summary>
        public double pitch_up; // =pi/2

        /// <summary>
        /// 最小桨距角[rad]
        /// </summary>
        public double pitch_down; // =0.0

        /// <summary>
        /// 是否变桨
        /// </summary>
        public bool ifpitch; // =true

        /// <summary>
        /// 
        /// </summary>
        public double Fixed_pitch; // =0.0

        /// <summary>
        /// 
        /// </summary>
        public double Fixed_rotationalspeed; // =0.0

        /// <remarks>
        /// 计算叶片静态功率曲线的
        /// </remarks>
        /// <param name="min_windv">最小风速[m/s]</param>
        /// <param name="max_windv">最大风速[m/s]</param>
        /// <param name="wind_step">间隔 -</param>
        /// <param name="orig_pit">额度桨距角[rad]【</param>
        /// <param name="ωmin">切入转速[rpm/min]</param>
        /// <param name="opt_KW">额定功率[kw]</param>
        /// <param name="opt_rpm_rad">>额定转速[rpm/min]</param>
        /// <param name="η">发电机效率%</param>
        /// <param name="pitch_up">最大桨距角[rad]</param>
        /// <param name="pitch_down">最小桨距角[rad]</param>
        /// <param name="ifpitch">是否变桨</param>
        /// <param name="Fixed_pitch"></param>
        /// <param name="Fixed_rotationalspeed"></param>
        public T_ALStp_pow(double min_windv = 3.0, double max_windv = 25.0, double wind_step = 0.1, double orig_pit = 0.0, double ωmin = 1.0, double opt_KW = 0.0, double opt_rpm_rad = 0.0, double η = 1.0, double pitch_up = Math.PI / 2, double pitch_down = 0.0, bool ifpitch = true, double Fixed_pitch = 0.0, double Fixed_rotationalspeed = 0.0)
        {
            this.min_windv = min_windv;
            this.max_windv = max_windv;
            this.wind_step = wind_step;
            this.orig_pit = orig_pit;
            this.ωmin = ωmin;
            this.opt_KW = opt_KW;
            this.opt_rpm_rad = opt_rpm_rad;
            this.η = η;
            this.pitch_up = pitch_up;
            this.pitch_down = pitch_down;
            this.ifpitch = ifpitch;
            this.Fixed_pitch = Fixed_pitch;
            this.Fixed_rotationalspeed = Fixed_rotationalspeed;
        }
    }

    /// <summary>
    /// 动态失速模型结构体
    /// </summary>
    public struct T_ALDynamicStallPar
    {
        /// <summary>
        /// 当前仿真时间,Elastic 设置
        /// </summary>
        public double t;
        /// <summary>
        /// 仿真时间间隔,Elastic 设置
        /// </summary>
        public double dt;

        public int I_t;

        public double[] Tspan;
        /// <summary>
        /// 当前的风力机是否使用动态失速模型；AeroL_Pro(string path)当中设置
        /// </summary>
        public bool UseDynamicStallPar;

        /// <summary>
        /// 当前要计算动态失速的界面位置编号，UnSteadyBEMT.BEMTMex实时更新
        /// </summary>
        public int Numsec;

        /// <summary>
        /// 当前要计算动态失速的叶片编号，UnSteadyBEMT.BEMTMex实时更新
        /// </summary>
        public int Numb;

        /// <summary>
        /// 当前截面的风速,UnSteadyBEMT.BEMTMex实时更新
        /// </summary>
        public double Urel;

        /// <summary>
        /// 当前截面的弦长,UnSteadyBEMT.BEMTMex实时更新
        /// </summary>
        public double Cord;
    }


    /// <summary>
    /// 动态失速模型接口
    /// </summary>
    public interface I_ALDynamicStall
    {
        public (double Cl, double Cd, double Cm) AL_DynamicStall(double phi, double theta, int FoilNo, ref T_ALDynamicStallPar par);

        public (double Cl, double Cd, double Cm) AL_CalBladeDynamicStall(double phi, double theta, int FoilNo);
        public void AirfoilSolve();
    }
    /// <summary>
    /// 要使用动态失速模型，必须实现这个接口
    /// </summary>
    public interface I_ALCalDynamicStall
    {
        public (double Cl, double Cd, double Cm) AL_DynamicStall(double phi, double theta, int FoilNo);
    }
    /// <summary>
    /// 要计算非定常空气动力学力要继承这个接口。
    /// </summary>
    public interface I_ALCalDynamicAeroLoad
    {
        /// <remarks>
        /// 输入的 q_GeAz, VMB, UMB,  phi，FlexBlSpn， Pitch, chi0
        /// </remarks>
        public (Matrix<double> px, Matrix<double> py, Matrix<double> Mz, Matrix<double> phi) AL_CalBladeDynamicAeroLoad(double q_GeAz, double qd_GeAz, List<Matrix<double>> VMB, List<Matrix<double>> UMB, Matrix<double> phi, Matrix<double> FlexBlSpn, Vector<double> Pitch, double chi0);
    }

    public interface I_ALUnsteadyPutRes
    {
        public void AL_WriteTitle(string[] title);

        public void AL_WriteUnit(string[] title);

        public void AL_Write(double value);

        public void AL_Write(string value);

        public void AL_WriteLine(double value);

        public void AL_WriteLine(string value);



    }




    public interface I_ALPUTini
    {
        public void AL_PutINI(AeroL1 aeroL1);

        public void WriteBEMEleValue();
    }
    /// <summary>
    /// 叶片节点单元气动结构体。
    /// </summary>
    public struct T_ALAeroBladeElement
    {
        // BEMT理论下定义的叶片节点单元
        /// <summary>
        /// 叶片单元节点的弦长[m]
        /// </summary>
        public double SChord;

        /// <summary>
        /// 叶片单元的气动扭角[rad]
        /// </summary>
        public double STwist;

        /// <summary>
        /// 当前节点的翼型编号[-]
        /// </summary>
        public int SFoilNum;

        /// <summary>
        /// 叶片当前节点的厚度[%],当前的模型版本不需要
        /// </summary>
        public double Sthickness;

        /// <summary>
        /// 当前叶片节点的展向位置(包括了轮毂半径)[m]
        /// </summary>
        public double SBlspan;

        /// <summary>
        /// 当前叶片节点的轮毂半径[m]
        /// </summary>
        public double SRHub;

        /// <summary>
        /// 叶片的叶片顶端到轴中心的距离[m]
        /// </summary>
        public double STipRad;

        /// <summary>
        /// 当前叶片节点的实度。是一个定值。[-]没有除以展向位置
        /// </summary>
        public double SSolid;

        /// <summary>
        /// 当前的变桨轴向的位置[m]
        /// </summary>
        public double SPitchAxis;

        /// <summary>
        /// 当前的气动力矩中心J1
        /// </summary>
        public double SAeroCentJ1;

        /// <summary>
        /// 当前的气动力矩中心J2
        /// </summary>
        public double SAeroCentJ2;
        /// <summary>
        /// 叶片当前当前节点的攻角[rad]
        /// </summary>
        public double DAOA;

        /// <summary>
        /// 当前叶片节点的入流角[rad]
        /// </summary>
        public double Dphi;

        /// <summary>
        /// 当前叶片节点由于结构变形而产生的扭转角。[rad]
        /// </summary>
        public double DStrTwist;


        /// <summary>
        /// 当前叶片节点由于变形而产生的展向位移的改变[m]
        /// </summary>
        public double DFlexSpan;

        /// <summary>
        /// 当前叶片节点的方位角，相同叶片上的节点方位角相同。[rad]
        /// </summary>
        public double DAzimuth;

        /// <summary>
        /// 当前节点的x向当地水平风速[m/s]
        /// </summary>
        public double DVx;

        /// <summary>
        /// 当前节点的y向切向风速[m/s]
        /// </summary>
        public double DVy;

        /// <summary>
        /// Axial induced wind velocity at each node,轴向输入风速
        /// </summary>
        public double Vindx;

        /// <summary>
        /// Tangential induced wind velocity at each node，切向输入风速
        /// </summary>
        public double Vindy;


        /// <summary>
        ///  当前节点的入流风速[m/s]
        /// </summary>
        public double DVrel;

        /// <summary>
        /// 当前节点的实际风速[m/s]
        /// </summary>
        public double DU;

        /// <summary>
        /// 轴向诱导因子[-]
        /// </summary>
        public double Da;

        /// <summary>
        /// 切向诱导因子[-]
        /// </summary>
        public double Daa;

        /// <summary>
        /// 当前节点的插值升力系数[-]
        /// </summary>
        public double DCl;

        /// <summary>
        /// 当前节点的插值阻力系数[-]
        /// </summary>
        public double DCd;

        /// <summary>
        /// 当前节点的插值倾覆力矩系数[-]
        /// </summary>
        public double DCm;
        /// <summary>
        /// 轴向力系数Cx[-]
        /// </summary>
        public double DCx;

        /// <summary>
        /// 切向力系数Cy[-]
        /// </summary>
        public double DCy;

        /// <summary>
        /// 当前节点的变桨角[rad]
        /// </summary>
        public double DPitch;

        /// <summary>
        /// 轴向气动力[N]
        /// </summary>
        public double DPx;

        /// <summary>
        /// 切向气动力[N]
        /// </summary>
        public double DPy;

        /// <summary>
        /// 变桨气动力矩[Nm]
        /// </summary>
        public double DMz;

        /// <summary>
        /// 气动扭矩[Nm]
        /// </summary>
        public double DMx;
        /// <summary>
        /// 叶片的尖速比。
        /// </summary>
        public double lmda;

        /// <summary>
        /// Pitch+Twist angle at each node
        /// </summary>
        public double DTheta;
        /// <summary>
        /// 单个叶片气动推力[N]
        /// </summary>
        public double Thrust;
        /// <summary>
        /// 单个叶片的气动扭矩[Nm]
        /// </summary>
         public double Torque;

        public double Dpostion_x;

        public double Dpostion_y;

        public double DChi0;

    }

    /// <summary>
    /// 塔架节点单元气动结构体。
    /// </summary>
    public struct T_ALAeroTowerElement
    {
        /// <summary>
        /// 塔架单元节点的直径[m]
        /// </summary>
        public double SChord;

        /// <summary>
        /// 塔架单元的高度[m]，没有叠加基础高度
        /// </summary>
        public double SHeight;

        /// <summary>
        /// 塔架节点的气动力系数[-]
        /// </summary>
        public double SCd;

    }
    /// <remarks>
    /// 所有的AeroL当中的参数。读取AeroDyn15文件
    /// </remarks>
    public struct AeroL1
    {
        #region 文件生成结构体
        //# 保存AeroDyn文件的路径eg:NRELOffshrBsline5MW_Onshore_AeroDyn15

        /// <remarks>
        /// 保存AeroDyn文件的路径eg:NRELOffshrBsline5MW_Onshore_AeroDyn15
        /// </remarks>
        public string AeroLfilepath;

        /// <remarks>
        /// AeroLfilepath;//>NRELOffshrBsline5MW_Onshore_AeroDyn15文件当中的信息
        /// </summary;
        public string[] AerolData;

        #endregion 文件生成结构体

        #region 计算选项
        /// <remarks>
        /// 计算选项{0=外部调用将读取塔架叶片等模型参数，1=计算叶片Cp,2=计算叶片的功率，推力等变桨曲线表}
        /// </remarks>
        public int ApOfMb;
        #endregion 计算选项


        #region 常规选项
        /// <remarks>
        /// 空气动力学计算的时间间隔｛或“默认”｝（s）
        /// </remarks>
        public double DTAero;

        /// <remarks>
        /// 尾流/感应模型（开关）的类型｛0=无，1=BEMT，2=FreeWake｝
        /// </remarks>
        public int WakeMod; //       

        /// <remarks>
        /// 叶片动态失速模型类型（开关）｛1=steady model, 2=BL ，3=Øye ，4=IAG，5=GOR，6=ATEF｝
        /// </remarks>
        public int AFAeroMod;

        /// <remarks>
        /// 基于塔周围潜在流量的类型塔对风的影响（开关）｛0=无，1=基线潜在流量，2=带Bak校正的潜在流量｝
        /// </remarks>
        public int TwrPotent;

        /// <remarks>
        /// 是否计算塔影效应
        /// </remarks>
        public bool TwrShadow;

        /// <remarks>
        ///  计算塔架开启动力学
        /// </remarks>
        public bool TwrAero;
        #endregion 常规选项


        #region 环境物理参数
        /// <remarks>
        /// 空气密度kg/m^3
        /// </remarks>
        public double AirDens;

        /// <remarks>
        /// 运动空气粘度（m^2/s）
        /// </remarks>
        public double KinVisc;

        /// <remarks>
        /// 声速（m/s）
        /// </remarks>
        public double SpdSound;
        #endregion 环境物理参数


        #region 叶素动量理论选项

        /// <remarks>
        /// 偏斜尾流修正模型类型（开关） { 1 = none, 2 = Pitt / Peters}[used only when WakeMod=1]
        /// </remarks>
        public int SkewMod;

        /// <remarks>
        /// Pitt/Peters斜尾流模型中使用的参数 {or "default" is 15/32*pi} (-) [used only when SkewMod=2; unused when WakeMod=0 or 3]
        /// </remarks>
        public double SkewModFactor;

        /// <remarks>
        /// 叶尖损失修正
        /// </remarks>
        public bool TipLoss;

        /// <remarks>
        /// 叶根损失修正
        /// </remarks>
        public bool HubLoss;

        /// <remarks>
        /// 叶素动量理论的迭代误差要求 [used only when WakeMod=1]
        /// </remarks>
        public double BemtError;


        /// <remarks>
        /// 叶素动量理论的最大迭代次数
        /// </remarks>
        public int MaxIter;

        #endregion 叶素动量理论选项


        #region Beddoes Leishman不稳定翼型空气动力学选项
        /// <remarks>
        /// 非定常气动模型开关（switch）{1=基准模型（原始模型），2=Gonzalez变型（改变Cn、Cc、Cm），3=Minemma/Pierce变型（改变Cc和Cm）} [仅在AFAeroMod=2时使用]。<br/>
        /// <br/> - 这段英文描述了一个关于非定常气动学模型的开关选项，其中"switch"指的是选择哪种非定常气动学模型。共有三种模型可供选择：基准模型、Gonzalez变型和Minemma/Pierce变型。其中，Gonzalez变型和Minemma/Pierce变型都是基于基准模型进行的改进，分别改变了不同的气动系数（如升力系数Cn、阻力系数Cc、弯矩系数Cm等）。在实际应用中，这个开关选项通常与AFAeroMod参数一起使用，用于指定非定常气动学计算所采用的模型和算法。
        /// </remarks>
        public int UAMod;

        /// <remarks>
        /// 是否计算流场参数f'的查找表（TRUE），还是使用最佳拟合指数方程（FALSE）的标志。<br/>
        /// <br/> - 这段英文描述了一个关于流场参数f'的计算方法的选择。其中，f'是一个描述流场来流状态的无量纲参数，通常用于非定常气动学计算中。在计算过程中，可以选择计算f'的查找表，也可以使用最佳拟合指数方程。如果选择计算查找表，则需要预先计算和存储一系列f'的数值和相应的计算结果，以便在实际计算中进行查找和插值。而如果选择使用最佳拟合指数方程，则可以通过对已知的f'和相应的计算结果进行拟合，得到一个数学模型，从而在实际计算中直接使用该模型进行计算。该标志用于指示所选择的计算方法。
        /// </remarks>
        public bool FLookup;
        #endregion Beddoes Leishman不稳定翼型空气动力学选项


        #region 自由涡尾迹模型选项

        /// <remarks>
        /// 自由涡尾迹模型设置文件 [used only when WakeMod=3]
        /// </remarks>
        public string OLAFInputFileName;


        public FreeWake FreeWake;
        #endregion  自由涡尾迹模型选项


        #region AerofoiLs 信息
        /// <remarks>
        /// Number of airfoil files
        /// </remarks>
        public int NumAFfiles;

        /// <remarks>
        /// 翼型路径，厚度从高到低
        /// </remarks>
        public string[] airfoilpath;

        /// <remarks>
        /// AeroL读取到的翼型信息
        /// </remarks>
        public Airfoil1 Airfoil { get; set; }

        #endregion AerofoiLs 信息


        #region 叶片和塔架Geometry

        /// <remarks>
        /// 包含叶片分空气动力学特性的文件名称
        /// </remarks>
        public string[] ADBlFile_1;

        /// <remarks>
        /// 塔基开始的基础高度在水面上的位置（陆上风力机设置为0，海上按照ElastoDyn.TowerBsHt的高度进行设置）
        /// </remarks>
        public double TowerBsHt;

        /// <remarks>
        /// 分析中使用的塔节点数 [used only when TwrPotent/=0, TwrShadow=True, or TwrAero=True]
        /// </remarks>
        public int NumTwrNds;


        /// <summary>
        /// 叶片截面参数
        /// </summary>
        public int NumBldNds;
        /// <remarks>
        /// AeroL读取的叶片和塔架几何信息
        /// </remarks>
        //public Geometry1 Geometry;


        /// <summary>
        /// Openhast 当中增加的新的参数，OpenWECd当中没有，这个函数支持不同叶片的定义
        /// </summary>
        public Geometry1[] Geometry;
        #endregion 叶片和塔架Geometry



        #region  叶片信息  [used only when ApOfMb=1 and 2]
        /// <remarks>
        /// - 轮毂半径
        /// </remarks>
        public double HubRad;
        /// <remarks>
        ///  - 叶片数量
        /// </remarks>
        public int Bldnum;

        #endregion  叶片信息  [used only when ApOfMb=1 and 2]


        #region 计算Cp曲线 [used only when ApOfMb=1]

        /// <remarks>
        ///  -最小的叶尖速比
        /// </remarks>
        public double Minlamda;

        /// <remarks>
        /// -最大的叶尖速比
        /// </remarks>
        public double Maxlamda;

        /// <remarks>
        ///   -叶尖速比的间隔
        /// </remarks>
        public double lamdaStep;

        /// <remarks>
        /// -最小的叶尖速比
        /// </remarks>
        public double MinPitch;

        /// <remarks>
        /// -最大的叶尖速比
        /// </remarks>
        public double MaxPitch;

        /// <remarks>
        /// -叶尖速比的间隔
        /// </remarks>
        public double PitchStep;

        /// <remarks>
        /// -计算Cp曲线的结果文件
        /// </remarks>
        public string CpResultFilePath;

        #endregion 计算Cp曲线 [used only when ApOfMb=1]


        #region 计算功率曲线 [used only when ApOfMb=2]

        /// <remarks>
        ///  -最小风速
        /// </remarks>
        public double MinWindSpeed;

        /// <remarks>
        /// -最大风速
        /// </remarks>
        public double MaxWindSpeed;

        /// <remarks>
        ///   -风速间隔
        /// </remarks>
        public double WindSpeedStep;



        /// <remarks>
        /// - 初始桨距角[rad]
        /// </remarks>
        public double orig_pit;

        /// <remarks>
        /// - 切入转速[rpm / min]
        /// </remarks>
        public double ωmin;

        /// <remarks>
        /// - 额定功率[kw]
        /// </remarks>
        public double opt_KW;

        /// <remarks>
        /// - 额定转速[rpm / min]
        /// </remarks>
        public double opt_rpm_rad;

        /// <remarks>
        /// - 发电机效率%
        /// </remarks>
        public double η;

        /// <remarks>
        ///  - 最大桨距角[rad]
        /// </remarks>
        public double pitch_up;

        /// <remarks>
        ///  - 最小桨距角[rad]
        /// </remarks>
        public double pitch_down;

        /// <remarks>
        ///  - 是否变桨
        /// </remarks>
        public bool ifpitch;

        /// <remarks>
        /// - 固定桨距角[rad][used only when ifpitch = false]
        /// </remarks>
        public double Fixed_pitch;

        /// <remarks>
        ///  - 固定转速[rpm / min][used only when ifpitch = false]
        /// </remarks>
        public double Fixed_rotationalspeed;

        /// <remarks>
        /// -计算功率曲线的结果文件目录
        /// </remarks>
        public string PowerCurveResultFilePath;
        #endregion 计算功率曲线 [used only when ApOfMb=2]

        #region 其他参数
        public Vector<double>[] PitchAxis;

        public Vector<double>[] AeroCentJ1;

        public Vector<double>[] AeroCentJ2;
        #endregion 其他参数


        #region output

        /// <summary>
        /// 是否生成输出文件
        /// </summary>
        public bool SumPrint;

        /// <summary>
        /// 输出模式选择是节点·还是变量。如果是节点将生成下面节点数量的文件，否则生成输出变量个数的文件。
        /// </summary>
        public bool AfSpanput;
        /// <summary>
        /// 输出的叶片编号，默认只有0
        /// </summary>
        public int[] BldOutSig;
        /// <summary>
        /// 生成的文件夹名称，注意是文件夹！
        /// </summary>
        public string SumPath;

        /// <remarks>
        /// 叶片的输出截面个数
        /// </remarks>
        public int NBlOuts;

        /// <remarks>
        /// 输出的叶片节点编号
        /// </remarks>
        public int[] BlOutNd;

        /// <remarks>
        /// 塔架的输出截面个数
        /// </remarks>
        public int NTwOuts;

        /// <remarks>
        /// 输出的塔架节点编号
        /// </remarks>
        public int[] TwOutNd;

        public string[] Outputs_OutList;

        #endregion  output
    }


    public struct FreeWake
    {

    }
    public struct Geometry1
    {
        /// <remarks>
        /// 第一列BlSpn[m],第二列BlCrvAC[m](预弯),第三列BlSwpAC[m](后掠)，第四列BlCrvAng(预弯角，默认都是0),第五列BlTwist扭角[deg],第6列BlChord 弦长[m],第七列BlAFID是插值翼型编号[.],第八列pitch)
        /// </remarks>
        public Matrix<double> Blade;

        /// <remarks>
        /// 叶片的截面个数
        /// </remarks>
        public int NumBladeSection;

        /// <remarks>
        /// 第一列BlSpn[m]
        /// </remarks>
        public Vector<double> BlSpn;

        /// <remarks>
        /// 第二列BlCrvAC[m](预弯)
        /// </remarks>
        public Vector<double> BlCrvAC;

        /// <remarks>
        /// 第三列BlSwpAC[m](后掠)
        /// </remarks>
        public Vector<double> BlSwpAC;

        /// <remarks>
        /// 第四列BlCrvAng(预弯角，默认都是0)
        /// </remarks>
        public Vector<double> BlCrvAng;

        /// <remarks>
        /// 第五列BlTwist扭角[deg]
        /// </remarks>
        public Vector<double> BlTwist;

        /// <remarks>
        /// 第6列BlChord 弦长[m]
        /// </remarks>
        public Vector<double> BlChord;

        /// <remarks>
        /// 第七列BlAFID是插值翼型编号[.]
        /// </remarks>
        public int[] BlAFID;

        /// <remarks>
        /// 变桨轴线
        /// </remarks>
        public Vector<double> PitchAxis;
        /// <remarks>
        /// 塔架的几何外形,第一列是高度，第二列是直径，第三列是空气阻力系数Cd.,可以访问TowerH,TowerD,TowerCd来获取
        /// </remarks>
        public Matrix<double> Tower;

        /// <remarks>
        /// 塔架的截面个数
        /// </remarks>
        public int NumTowerSection;

        /// <remarks>
        /// 塔架第一列的高度[m]
        /// </remarks>
        public Vector<double> TowerH;

        /// <remarks>
        /// 塔架第二列的直径[m]
        /// </remarks>
        public Vector<double> TowerD;

        /// <remarks>
        /// 塔架第二列的Cd[.]
        /// </remarks>
        public Vector<double> TowerCd;


    }

    public struct airfoil__temp
    {
        #region===========  OYG modal  ============
        public double T_f_OYG;
        #endregion ===========  OYG modal  ============

        #region===========  IAG modal modal  ============
        /// <summary>
        /// Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.3]
        /// </summary>
        public double A1;
        /// <summary>
        /// Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]
        /// </summary>
        public double A2;
        /// <summary>
        /// //  Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]
        /// </summary>
        public double b1;
        /// <summary>
        ///  Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.53]
        /// </summary>
        public double b2;
        /// <summary>
        /// 
        /// </summary>
        public double ka;
        /// <summary>
        ///  Boundary-layer, leading edge pressure gradient time constant in the expression of Dp. It should be tuned based on airfoil experimental data. [default = 1.7]
        /// </summary>
        public double T_p;
        /// <summary>
        /// Initial value of the time constant associated with Df in the expression of Df and f''. [default = 3]
        /// </summary>
        public double T_f;
        /// <summary>
        /// //  Initial value of the time constant associated with the vortex lift decay process; it is used in the expression of Cvn.It depends on Re, M, and airfoil class. [default = 6]
        /// </summary>
        public double T_V;
        ///<summary>
        ///Initial value of the time constant associated with the vortex advection process; it represents the non-dimensional time in semi-chords, needed for a vortex to travel from LE to trailing edge(TE); it is used in the expression of Cvn.It depends on Re, M (weakly), and airfoil. [valid range = 6 - 13, default = 6]
        /// </summary>
        public double T_VL;
        /// <summary>
        /// 
        /// </summary>
        public double K_v;// 
        /// <summary>
        /// 
        /// </summary>
        public double K_Cf;// 
        /// <summary>
        /// 
        /// </summary>
        public double T_Um;// 
        /// <summary>
        /// 
        /// </summary>
        public double T_Dm;// 
        /// <summary>
        /// 
        /// </summary>
        public double M_IAG;// 
        #endregion ===========  IAG modal modal  ============

        #region===========  OYG modal  ============
        public double A_M;
        #endregion ===========  OYG modal  ============

        #region===========  BL modal  ============

        #endregion ===========  Bl modal  ============

        #region ===========   ATEF modal  ============
        /// <summary>
        /// 
        /// </summary>
        public double Tf_ATEF;
        /// <summary>
        /// 
        /// </summary>
        public double Tp_ATEF;

        #endregion ===========   ATEF modal  ============

        #region  ===========  Airfoil data  ===========

        /// <remarks>
        /// 在准稳态表格查找中要使用的插值次序 {1=线性；2=三次样条插值；"默认"} [默认=1]。
        /// </remarks>
        public int InterpOrd;
        /// <summary>
        /// 数据的列数
        /// </summary>
        public int Numcolumn;
        /// <remarks>
        /// 翼型行数
        /// </remarks>
        public int NumAlf;
        /// <remarks>
        /// 表示该翼型在不同攻角下的升力系数、阻力系数等数据。每一列分别是：[攻角 Cl Cd CM ......]
        /// </remarks>
        public Matrix<double> DataSet;


        #endregion  ===========  Airfoil data  ===========
    }
    public struct Airfoil1
    {
        public Airfoil1()
        {
            list = new List<airfoil__temp>();
        }
        /// <remarks>
        /// 表示空气动力学数据文件中所包含的翼型数量；
        /// </remarks>
        public int Nfoil;

        /// <remarks>
        /// 表示每个翼型的名称，是一个长度为 `nfoil` 的一维 `String` 数组；
        /// </remarks>
        public string[] StringFoil;


        public List<airfoil__temp> list;

        public airfoil__temp this[int a]
        {
            get
            {
                return list[a];
            }
            set
            {
                list[a] = value;
            }
        }

    }
}

