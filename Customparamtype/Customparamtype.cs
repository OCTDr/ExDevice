using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace Customparamtype
{ 

    public enum zone { 三度带 = 3, 六度带 = 6 };
    public enum JoinDirection { 半方向 = 0, 全方向 = 1 };
    public enum DESTYPE { DesWarning, DesError, DesNotError };
    public enum DATASIZEUNIT { kb, mb, gb };
    public enum NationalBasicScale { B, C, D, E, F, G, H };
    public enum ErrType { A, B, C, D, Undefined };
    public enum ThrDstype { DLG, DEM, DOM, DSM, JYYB };
    public enum Starttype { DLG, DEM, DOM, DSM, GEOPDF, EPSDLG, CZ, 解译样本, 地表覆盖, 遥感影像, 国情元数据, 国情要素, PIPE };
    public enum Missingtype { 不准多余, 不准缺失, 不准多余缺失 };
    public enum Coordsys { cgcs2000, cgcs2000prj_3d, cgcs2000prj_6d, xian80_3d, xian80_6d, beijing54_3d, beijing54_6d }
    public enum LengthUnits { Meter, Degree }
    public enum bordertype { 来自图号, 来自图廓线, 来自边界面, 来自对象边界 }
    public enum LxAChecktype { 必须相交, 必须不相交, 必须包含, 必须不包含, 必须边界相交, 必须部分包含,端点必须在边界 }//相交用参数指定
    public enum AxAChecktype { 必须相交, 必须不相交, 必须包含, 必须不包含, 必须被包含, 必须不被包含, 必须相邻, 必须部分被包含 }//相交用参数指定
    public enum PxLChecktype { 必须相邻, 必须不相邻,必须节点对应 }
    public enum LxLChecktype { 必须相交, 必须不相交, 必须包含, 必须不包含, 必须被包含, 必须不被包含, 必须相邻, 必须重叠 }//相交包括包含和相邻和重叠
    public enum PxAChecktype { 必须包含, 必须不包含, 必须相邻, 必须不相邻, 必须相交, 必须不相交, }//相交包括包含和相邻
    public enum PxPChecktype { 必须重叠, 必须不重叠 }
    public enum DataOrgType { 分幅, 整景, 分幅1, 整景1 }
    public enum RoundOffType { 舍位, 进位, 四舍五入 }
    public enum ContourType { 首曲线, 计曲线, 间曲线, 助曲线 } //登高线类型
    public enum ControlType { 精确, 模糊 }
    public enum ExtentCheckType { 位置缺失, 位置超出, 缺失与超出 }

    public enum LxARetype { 相交, 包含, 相邻 }//相交只限于cross
    public enum AxARetype { 相交, 包含, 相邻 }//相交只限于cross
    public enum PxLRetype { 相邻, 端点相邻 }
    public enum LxLRetype { 重叠, 相交 }//相交只限于cross
    public enum PxARetype { 包含, 相邻 }
    public enum PxPRetype { 重叠 }
    public enum Hittype { 可在任意点, 必须在节点, 必须在端点,必须节点对应 }
    public enum DivisionType { 行政村, 乡镇, 市, 县, 省级 }
    public enum RoadGradeType { 高速, 一级, 二级, 三级, 四级, 等外 }


    public enum DeviceType { 水准仪,全站仪,光电测距仪,GNSS}



    public class checkdata
    {
        string pdataid;
        string pdataremotepath;
        string pdatalocalpath;
        string pdatamxdfile;
        bool   pdataissample;
        string pdataformattype;
        string pdatadataname;
        object ptag;
        public object Tag
        {
            get
            {
                return ptag;
            }
            set
            {
                ptag = value;
            }
        }
        public string Dataid
        {
            get
            { return pdataid; }

            set
            {
                pdataid = value;
            }
        }
        public string RemotePath
        {
            get
            {
                return pdataremotepath;
            }
            set
            {
                pdataremotepath = value;
            }
        }
        public string LocalPath
        {
            get
            {
                return pdatalocalpath;
            }
            set
            {
                pdatalocalpath = value;
            }
        }
        public string MxdFile
        {
            get
            {
                return pdatamxdfile;
            }
            set
            {
                pdatamxdfile = value;
            }
        }
        public bool IsSample
        {
            get
            {
                return pdataissample;
            }
            set
            {
                pdataissample = value;
            }
        }

        public string FormatType
        {
            get
            {
                return pdataformattype;
            }
            set
            {
                pdataformattype = value;
            }
        }
        public string DataName
        {
            get
            {
                return pdatadataname;
            }
            set
            {
                pdatadataname = value;
            }
        }
    }


}


