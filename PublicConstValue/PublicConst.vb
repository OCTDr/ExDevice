Public Class PublicConst
#Region "全局常量表名称"
    Public Const cQC_PRO_productlevelTable As String = "QC_PRO_PRODUCTLEVEL"
    Public Const cQC_PRO_producttypeTable As String = "QC_PRO_PRODUCTTYPE"
    Public Const cQC_EVA_quaelementTable As String = "QC_EVA_QUAELEMENT"
    Public Const cQC_EVA_subquaelementTable As String = "QC_EVA_SUBQUAELEMENT"
    Public Const cQC_EVA_checkentryTable As String = "QC_EVA_CHECKENTRY"
    Public Const cQC_INS_checkoperatorTable As String = "QC_INS_CHECKOPERATOR"
    Public Const cQC_INS_checkruleTable As String = "QC_INS_CHECKRULE"
    Public Const cQC_INS_checkprojectTable As String = "QC_INS_CHECKPROJECT"
    Public Const cQC_INS_productoperatorTable As String = "QC_INS_PRODUCTOPERATOR"
    Public Const cQC_PRO_checkdataTable As String = "QC_PRO_CHECKDATA"
    Public Const cQC_INS_erorecodeTable As String = "QC_INS_ERORECODE"
    Public Const cQC_PRO_jobTable As String = "QC_PRO_JOB"
    Public Const cQC_DAT_datascheme As String = "QC_DAT_DATASCHEME"
    Public Const cQC_EVA_checkrecodeTable As String = "QC_EVA_CHECKRECODE"
    Public Const cQC_INS_userruleTable As String = "QC_INS_USERRULE"
    Public Const cQC_INS_RuleStatusTable As String = "QC_INS_CHECKRULESTATE"
#End Region
#Region "全局常量表字段名称"

    Public Const cWarningScore As Double = 60

    Public Const cFieldTaskNumber As String = "任务通知单号"
    Public Const cFieldProductLevelName As String = "产品级别名称"
    Public Const cFieldProductLevel As String = "产品级别"
    Public Const cFieldProductLevelCode As String = "产品级别编码"
    Public Const cFieldProductTypeCode As String = "产品类别编码"
    Public Const cFieldProductTypeName As String = "产品类别名称"
    Public Const cFieldProductType As String = "产品类别"
    Public Const cFieldProjectID As String = "方案ID"
    Public Const cFieldProjectName As String = "方案名称"
    Public Const cFieldDescription As String = "描述"
    Public Const cFieldIsDefaut As String = "是否默认"
    Public Const cFieldModifyRights As String = "修改权限"
    Public Const cFieldRemark As String = "备注"
    Public Const cFieldName As String = "名称"
    Public Const cFieldQuaelementCode As String = "质量元素编码"
    Public Const cFieldSubQuaelementCode As String = "质量子元素编码"
    Public Const cFieldCheckentryCode As String = "检查项编码"
    Public Const cFieldStandardCheckentryCode As String = "标准检查项编码"
    Public Const cFieldCheckoperatorCode As String = "算子编码"
    Public Const cFieldCheckoperatorName As String = "算子名称"
    Public Const cFieldCheckoperatorShortName As String = "短名称"
    Public Const cFieldCheckoperatorProduct As String = "适用产品类型"
    Public Const cFieldCheckMethod As String = "检查方式" '自动，人机
    Public Const cFieldCheckRuleID As String = "规则ID"
    Public Const cFieldUserName As String = "用户名"
    Public Const cFieldUserCheckRuleID As String = "用户规则ID"
    Public Const cFieldCheckRuleParam As String = "规则参数"
    Public Const cFieldCheckRuleName As String = "规则名称"
    Public Const cFieldOwner As String = "创建人"
    Public Const cFieldIsPrimary As String = "是否概查"
    Public Const cFieldTaskCode As String = "任务编号"
    Public Const cFieldJobCode As String = "作业编号"
    Public Const cFieldJobOperator As String = "作业员"
    Public Const cFieldCheckWay As String = "检查方式" '自动，人机
    Public Const cFieldUpdater As String = "修改人"
    Public Const cFieldModifyTime As String = "修改日期"
    Public Const cFieldDataId As String = "数据ID"
    Public Const cFieldDataPath As String = "数据路径"
    Public Const cFieldDataName As String = "数据名称"
    Public Const cFieldDataFormat As String = "数据格式"
    Public Const cFieldDataSize As String = "数据大小"
    Public Const cFieldCheckObject As String = "检查对象"
    Public Const cFieldErrDescription As String = "错误描述"
    Public Const cFieldPicture As String = "截图"
    Public Const cFieldGeometry As String = "实体"
    Public Const cFieldCoordX As String = "坐标X"
    Public Const cFieldCoordY As String = "坐标Y"
    Public Const cFieldCoordH As String = "坐标H"
    Public Const cFieldCoordMINX As String = "MINX"
    Public Const cFieldCoordMINY As String = "MINY"
    Public Const cFieldCoordMAXX As String = "MAXX"
    Public Const cFieldCoordMAXY As String = "MAXY"
    Public Const cFieldIsConfirmed As String = "是否确认"
    Public Const cFieldErrType As String = "错漏类别"
    Public Const cFieldErrID As String = "错误ID"
    Public Const cFieldIsSample As String = "是否样本"
    Public Const cFieldFileName As String = "文件名"
    Public Const cFieldEnable As String = "是否可用"
    Public Const cFieldschemeID As String = "数据规定ID"
    Public Const cFieldschemeName As String = "数据规定名称"
    Public Const cFieldschemeType As String = "数据规定类型"
    Public Const cFieldModelFile As String = "模板文件名"
    Public Const cFieldModelFileContext As String = "模板文件内容"
    Public Const cFieldDesType As String = "描述类型"
    Public Const cFieldErrCount As String = "错漏数量"
    Public Const cFieldJobName As String = "作业名称"
    Public Const cFieldJobStatus As String = "作业状态"
    Public Const cFieldJobCount As String = "数据数量"
    Public Const cFieldJobPlanEndTime As String = "作业计划结束时间"
    Public Const cFieldJobPlanStartTime As String = "作业计划开始时间"
    Public Const cFieldIsCompleted As String = "是否完成"
    Public Const cFieldAutoCheckState As String = "自动检查进度"
    Public Const cFieldChecker As String = "检查员"
    Public Const cFieldManuFinishTime As String = "人机检查完成时间"
    Public Const cFieldRuleStatus As String = "规则状态"
    Public Const cFieldDiskID As String = "磁盘ID"
    Public Const cFieldJobStartType As String = "启动类型"
    Public Const cFieldParameter As String = "参数"
    Public Const cFieldCheckTimes As String = "检查次数"
#End Region
#Region "通用常量名称"
    Public Const C_DATANAME As String = "DATANAME"
    Public Const C_DATAID As String = "DATAID"
    Public Const C_REMOTEDATAPATH As String = "REMOTEDATAPATH"
    Public Const C_DATAFORMAT As String = "DATAFORMAT"
    Public Const C_LOCALDATAPATH As String = "LOCALDATAPATH"
    Public Const C_LOCALMXDPATH As String = "LOCALMXDPATH"
#End Region
#Region "算子常量字典表"
    Public Class OperatorCodeDictionary
        Public Shared ReadOnly Property OperatorCode As Dictionary(Of String, OperatorCodeClass)
            Get
                Return Add()
            End Get
        End Property
        Private Shared Function Add() As Dictionary(Of String, OperatorCodeClass)
            Dim P As New Dictionary(Of String, OperatorCodeClass)
            P.Add(cOP_人机交互, New OperatorCodeClass(cOP_人机交互, "人机交互检查"))
            P.Add(cOP_DLG_属性_数据集, New OperatorCodeClass(cOP_DLG_属性_数据集, "DLG数据集的正确性"))
            P.Add(cOP_DLG_属性_要素集, New OperatorCodeClass(cOP_DLG_属性_要素集, "DLG要素集的正确性"))
            P.Add(cOP_DLG_属性_属性项, New OperatorCodeClass(cOP_DLG_属性_属性项, "DL属性项的正确性"))
            P.Add(cOP_DLG_属性_非法代码, New OperatorCodeClass(cOP_DLG_属性_非法代码, "DLG是否存在非法代码"))
            P.Add(cOP_DLG_数学基础_坐标系统, New OperatorCodeClass(cOP_DLG_数学基础_坐标系统, "能够被AE识别的坐标系统数据格式检查"))
            P.Add(cOP_DLG_数学基础_投影参数, New OperatorCodeClass(cOP_DLG_数学基础_投影参数, "能够被AE识别的投影参数数据格式检查"))
            P.Add(cOP_DLG_数学基础_数学基础, New OperatorCodeClass(cOP_DLG_数学基础_数学基础, "能够被AE识别的整个数学基础数据格式检查"))
            P.Add(cOP_DLG_要素关系_线面关系, New OperatorCodeClass(cOP_DLG_要素关系_线面关系, "线面相交检测"))
            P.Add(cOP_DLG_拓扑关系_面重叠, New OperatorCodeClass(cOP_DLG_拓扑关系_面重叠, "DLG面重叠"))
            P.Add(cOP_DLG_拓扑关系_线重叠, New OperatorCodeClass(cOP_DLG_拓扑关系_线重叠, "DLG线重叠"))
            P.Add(cOP_DLG_拓扑关系_点重叠, New OperatorCodeClass(cOP_DLG_拓扑关系_点重叠, "DLG点重叠"))
            P.Add(cOP_DLG_拓扑关系_面裂隙, New OperatorCodeClass(cOP_DLG_拓扑关系_面裂隙, "DLG面裂隙"))
            P.Add(cOP_DLG_拓扑关系_相邻面属性, New OperatorCodeClass(cOP_DLG_拓扑关系_相邻面属性, "检查相邻面属性是否相同"))
            P.Add(cOP_DLG_拓扑关系_极小面, New OperatorCodeClass(cOP_DLG_拓扑关系_极小面, "DLG极小面"))
            P.Add(cOP_DLG_拓扑关系_伪节点, New OperatorCodeClass(cOP_DLG_拓扑关系_伪节点, "DLG伪节点"))
            P.Add(cOP_DLG_拓扑关系_极短线, New OperatorCodeClass(cOP_DLG_拓扑关系_极短线, "DLG极短线"))
            P.Add(cOP_DLG_拓扑关系_悬挂点, New OperatorCodeClass(cOP_DLG_拓扑关系_悬挂点, "DLG悬挂点"))
            P.Add(cOP_DLG_数学基础_图幅分幅, New OperatorCodeClass(cOP_DLG_数学基础_图幅分幅, "检查内图廓线是否正确"))
            P.Add(cOP_NC_属性精度_CC与GB一致性, New OperatorCodeClass(cOP_NC_属性精度_CC与GB一致性, "检查地理国情CC码与GB码一致性"))
            P.Add(cOP_DLG_属性精度_必填字段, New OperatorCodeClass(cOP_DLG_属性精度_必填字段, "DLG必填字段"))
            P.Add(cOP_DLG_属性精度_选填字段, New OperatorCodeClass(cOP_DLG_属性精度_选填字段, "DLG选填字段"))
            P.Add(cOP_DLG_属性精度_枚举字段, New OperatorCodeClass(cOP_DLG_属性精度_枚举字段, "DLG枚举字段"))
            P.Add(cOP_DLG_属性精度_缺省值, New OperatorCodeClass(cOP_DLG_属性精度_缺省值, "DLG默认值"))
            P.Add(cOP_DLG_要素关系_面面关系, New OperatorCodeClass(cOP_DLG_要素关系_面面关系, "DLG默认值"))
            P.Add(cOP_DLG_要素关系_点线关系, New OperatorCodeClass(cOP_DLG_要素关系_点线关系, "DLG默认值"))
            P.Add(cOP_DLG_要素关系_线线关系, New OperatorCodeClass(cOP_DLG_要素关系_线线关系, "DLG默认值"))
            P.Add(cOP_DLG_要素关系_点面关系, New OperatorCodeClass(cOP_DLG_要素关系_点面关系, "DLG默认值"))
            P.Add(cOP_DLG_要素关系_点点关系, New OperatorCodeClass(cOP_DLG_要素关系_点点关系, "DLG默认值"))
            P.Add(cOP_DLG_属性_要素类型, New OperatorCodeClass(cOP_DLG_属性_要素类型, "DLG要素类型"))
            P.Add(cOP_DLG_属性_几何类型, New OperatorCodeClass(cOP_DLG_属性_几何类型, "DLG几何类型"))
            P.Add(cOP_DLG_属性_空数据集, New OperatorCodeClass(cOP_DLG_属性_空数据集, "DLG空数据集"))
            P.Add(cOP_DLG_属性_非法记录, New OperatorCodeClass(cOP_DLG_属性_非法记录, "DLG空数据集"))
            P.Add(cOP_通用_概念一致_文件命名, New OperatorCodeClass(cOP_通用_概念一致_文件命名, "典型项目文件结构"))
            P.Add(cOP_通用_概念一致_文件结构, New OperatorCodeClass(cOP_通用_概念一致_文件结构, "典型项目文件结构"))

            P.Add(cOP_GRD_格网范围, New OperatorCodeClass(cOP_GRD_格网范围, "格网范围"))
            P.Add(cOP_GRD_格网接边, New OperatorCodeClass(cOP_GRD_格网接边, "格网接边"))
            P.Add(cOP_NC_摄影点位置, New OperatorCodeClass(cOP_NC_摄影点位置, "地理国情摄影点位置"))
            P.Add(cOP_NC_裁切范围, New OperatorCodeClass(cOP_NC_裁切范围, "地理国情解译样本裁切范围"))
            P.Add(cOP_NC_样本数据库, New OperatorCodeClass(cOP_NC_样本数据库, "地理国情解译样本数据库"))
            P.Add(cOP_NC_样本数据表, New OperatorCodeClass(cOP_NC_样本数据表, "地理国情解译样本数据表"))
            P.Add(cOP_NC_样本属性项, New OperatorCodeClass(cOP_NC_样本属性项, "地理国情解译样本属性项"))
            P.Add(cOP_NC_样本文件名, New OperatorCodeClass(cOP_NC_样本文件名, "地理国情解译样本文件命名"))
            P.Add(cOP_NC_数学基础, New OperatorCodeClass(cOP_NC_数学基础, "地理国情解译样本数学基础"))
            P.Add(cOP_DLG_连续_连通性, New OperatorCodeClass(cOP_DLG_连续_连通性, "DLG连通性"))
            P.Add(cOP_NC_解译样本缺失, New OperatorCodeClass(cOP_NC_解译样本缺失, "解译样本缺失"))
            P.Add(cOP_DLG_属性精度_PAC一致性, New OperatorCodeClass(cOP_DLG_属性精度_PAC一致性, "PAC一致性"))
            P.Add(cOP_NC_专业资料值对比, New OperatorCodeClass(cOP_NC_专业资料值对比, "专业资料值对比"))
            P.Add(cOP_NC_专业资料位置对比, New OperatorCodeClass(cOP_NC_专业资料位置对比, "专业资料位置对比"))
            P.Add(cOP_DLG_数学精度_矢量接边, New OperatorCodeClass(cOP_DLG_数学精度_矢量接边, "分幅矢量接边"))
            P.Add(cOP_DLG_属性精度_属性接边, New OperatorCodeClass(cOP_DLG_属性精度_属性接边, "分幅属性接边"))
            P.Add(cOP_DLG_数学精度_不规则边界矢量接边, New OperatorCodeClass(cOP_DLG_数学精度_不规则边界矢量接边, "不规则矢量接边"))
            P.Add(cOP_DLG_属性精度_不规则边界属性接边, New OperatorCodeClass(cOP_DLG_属性精度_不规则边界属性接边, "不规则属性接边"))
            P.Add(cOP_NC_DOM元数据内容错漏, New OperatorCodeClass(cOP_NC_DOM元数据内容错漏, "元数据文件检查"))
            P.Add(cOP_DLG_属性值异常, New OperatorCodeClass(cOP_DLG_属性值异常, "属性值异常检查"))
            P.Add(cOP_NC_解译样本分布正确性, New OperatorCodeClass(cOP_NC_解译样本分布正确性, "解译样本是否分布在数据范围内"))
            P.Add(cOP_NC_解译样本属性值一致性, New OperatorCodeClass(cOP_NC_解译样本属性值一致性, "解译样本表中的记录值是否一致"))
            P.Add(cOP_NC_解译样本数量正确性, New OperatorCodeClass(cOP_NC_解译样本数量正确性, "解译样本数量是否符合要求"))
            P.Add(cOP_NC_解译样本记录缺失, New OperatorCodeClass(cOP_NC_解译样本记录缺失, "解译样本文件是否在样本表中有记录"))
            P.Add(cOP_NC_解译样本文件缺失, New OperatorCodeClass(cOP_NC_解译样本文件缺失, "解译样本表记录的文件是否存在与文件夹中"))
            P.Add(cOP_NC_解译样本文件一致, New OperatorCodeClass(cOP_NC_解译样本文件一致, "PHOTO文件夹和IMG文件夹中的文件是否一致"))
            P.Add(cOP_NC_解译样本影像类型正确性, New OperatorCodeClass(cOP_NC_解译样本影像类型正确性, "解译样本文件类型是否填写正确"))
            P.Add(cOP_NC_节点数检查, New OperatorCodeClass(cOP_NC_节点数检查, "数据节点数是否正确"))
            P.Add(cOP_NC_精度检查, New OperatorCodeClass(cOP_NC_精度检查, "数据坐标精度及拓扑容差精度是否与模版一致"))
            P.Add(cOP_NC_解译样本角点, New OperatorCodeClass(cOP_NC_解译样本角点, "解译样本数据角点坐标一致"))

            P.Add(cOP_NC_调绘数据对比检查, New OperatorCodeClass(cOP_NC_调绘数据对比检查, "调绘数据检查"))
            P.Add(cOP_NC_数据表专题资料检查, New OperatorCodeClass(cOP_NC_数据表专题资料检查, "表格型专题资料检查"))
            P.Add(cOP_DLG_属性_高程值, New OperatorCodeClass(cOP_DLG_属性_高程值, "高程值正确性检查"))
            P.Add(cOP_NC_解译样本文件可读性, New OperatorCodeClass(cOP_NC_解译样本文件可读性, "文件格式检查"))
            P.Add(cOP_DLG_属性精度_点线矛盾, New OperatorCodeClass(cOP_DLG_属性精度_点线矛盾, "DLG点线矛盾检查"))
            P.Add(cOP_DLG_拓扑关系_复合要素, New OperatorCodeClass(cOP_DLG_拓扑关系_复合要素, "DLG复合要素检查"))
            P.Add(cOP_DLG_几何异常_异常折线, New OperatorCodeClass(cOP_DLG_几何异常_异常折线, "异常折线检查"))
            P.Add(cOP_DLG_几何异常_自相交, New OperatorCodeClass(cOP_DLG_几何异常_自相交, "线要素自相交检查"))
            P.Add(cOP_DLG_属性精度_PAC完整性, New OperatorCodeClass(cOP_DLG_属性精度_PAC完整性, "PAC完整性"))
            P.Add(cOP_DLG_属性精度_属性值约束, New OperatorCodeClass(cOP_DLG_属性精度_属性值约束, "属性值约束"))
            P.Add(cOP_NC_目录结构, New OperatorCodeClass(cOP_NC_目录结构, "目录结构检查"))
            P.Add(cOP_NC_解译样本像素个数, New OperatorCodeClass(cOP_NC_解译样本像素个数, "解译样本相片的像素数量"))
            P.Add(cOP_NC_元数据范围, New OperatorCodeClass(cOP_NC_元数据范围, "国情元数据范围"))

            P.Add(cOP_专用_数据组织_文件命名, New OperatorCodeClass(cOP_专用_数据组织_文件命名, "数据组织正确性"))
            P.Add(cOP_专用_数据组织_文件结构, New OperatorCodeClass(cOP_专用_数据组织_文件结构, "数据组织正确性"))
            P.Add(cOP_竞赛_自动化检查算子, New OperatorCodeClass(cOP_竞赛_自动化检查算子, "自动化检查算子"))
            '国情项目临时算子
            P.Add(cOP_NC_水文要素关系, New OperatorCodeClass(cOP_NC_水文要素关系, "水文要素关系"))
            'GeoPDF
            P.Add(cOP_PDF_图层组织, New OperatorCodeClass(cOP_PDF_图层组织, "GeoPDF数据组织检查"))

            Return P
        End Function
    End Class
    '10001
    Public Const cOP_人机交互 As String = "99999999"
    Public Const cOP_DLG_属性_数据集 As String = "10001006"
    Public Const cOP_DLG_属性_要素集 As String = "10001007"
    Public Const cOP_DLG_属性_属性项 As String = "10001008"
    Public Const cOP_DLG_属性_非法代码 As String = "10001009"
    Public Const cOP_DLG_属性_要素类型 As String = "10001010"
    Public Const cOP_DLG_属性_几何类型 As String = "10001011"
    Public Const cOP_DLG_属性_空数据集 As String = "10001012"
    Public Const cOP_DLG_属性_非法记录 As String = "10001047"

    Public Const cOP_DLG_数学基础_坐标系统 As String = "10001003"
    Public Const cOP_DLG_数学基础_投影参数 As String = "10001004"
    Public Const cOP_DLG_数学基础_数学基础 As String = "10001005"


    Public Const cOP_DLG_拓扑关系_面重叠 As String = "10001001"
    Public Const cOP_DLG_拓扑关系_面裂隙 As String = "10001018"
    Public Const cOP_DLG_拓扑关系_相邻面属性 As String = "10001002"
    Public Const cOP_DLG_拓扑关系_极小面 As String = "10001029"
    Public Const cOP_DLG_拓扑关系_伪节点 As String = "10001017"
    Public Const cOP_DLG_拓扑关系_极短线 As String = "10001030"
    Public Const cOP_DLG_拓扑关系_悬挂点 As String = "10001013"
    Public Const cOP_DLG_数学基础_图幅分幅 As String = "10001031"
    Public Const cOP_NC_属性精度_CC与GB一致性 As String = "50002001"
    Public Const cOP_DLG_属性精度_必填字段 As String = "10001028"
    Public Const cOP_DLG_属性精度_选填字段 As String = "10001014"
    Public Const cOP_DLG_属性精度_枚举字段 As String = "10001015"
    Public Const cOP_DLG_属性精度_缺省值 As String = "10001016"
    Public Const cOP_DLG_要素关系_面面关系 As String = "10001019"
    Public Const cOP_DLG_要素关系_点线关系 As String = "10001023"
    Public Const cOP_DLG_要素关系_线线关系 As String = "10001021"
    Public Const cOP_DLG_要素关系_点面关系 As String = "10001022"
    Public Const cOP_DLG_要素关系_点点关系 As String = "10001024"
    Public Const cOP_DLG_要素关系_线面关系 As String = "10001020"
    Public Const cOP_DLG_属性精度_PAC一致性 As String = "10001032"
    Public Const cOP_DLG_数学精度_矢量接边 As String = "10001033"
    Public Const cOP_DLG_属性精度_属性接边 As String = "10001034"
    Public Const cOP_DLG_数学精度_不规则边界矢量接边 As String = "10001035"
    Public Const cOP_DLG_属性精度_不规则边界属性接边 As String = "10001036"
    Public Const cOP_DLG_属性值异常 As String = "10001037"
    Public Const cOP_DLG_属性_高程值 As String = "10001038"
    Public Const cOP_DLG_属性精度_点线矛盾 As String = "10001039"
    Public Const cOP_DLG_属性精度_属性值对比 As String = "10001040"
    Public Const cOP_DLG_拓扑关系_复合要素 As String = "10001041"
    Public Const cOP_DLG_几何异常_异常折线 As String = "10001042"
    Public Const cOP_DLG_几何异常_自相交 As String = "10001043"
    Public Const cOP_DLG_拓扑关系_线重叠 As String = "10001044"
    Public Const cOP_DLG_拓扑关系_点重叠 As String = "10001045"
    Public Const cOP_DLG_属性精度_PAC完整性 As String = "10001046"
    Public Const cOP_DLG_属性精度_属性值约束 As String = "10001048"

    Public Const cOP_通用_概念一致_文件命名 As String = "10001026"
    Public Const cOP_通用_概念一致_文件结构 As String = "10001025"




    Public Const cOP_GRD_格网范围 As String = "20001001"
    Public Const cOP_GRD_格网接边 As String = "20001002"

    Public Const cOP_NC_摄影点位置 As String = "50001001"
    Public Const cOP_NC_裁切范围 As String = "50001002"
    Public Const cOP_NC_样本数据库 As String = "50001003"
    Public Const cOP_NC_样本数据表 As String = "50001004"
    Public Const cOP_NC_样本属性项 As String = "50001005"
    Public Const cOP_NC_样本文件名 As String = "50001006"
    Public Const cOP_NC_数学基础 As String = "50001007"
    Public Const cOP_DLG_连续_连通性 As String = "10001027"
    Public Const cOP_NC_解译样本缺失 As String = "50001008"
    Public Const cOP_NC_专业资料值对比 As String = "50001009"
    Public Const cOP_NC_DOM元数据内容错漏 As String = "50001010"
    Public Const cOP_NC_调绘数据对比检查 As String = "50001011"

    Public Const cOP_NC_解译样本分布正确性 As String = "50001012"
    Public Const cOP_NC_解译样本属性值一致性 As String = "50001013"
    Public Const cOP_NC_解译样本数量正确性 As String = "50001014"
    Public Const cOP_NC_解译样本记录缺失 As String = "50001015"
    Public Const cOP_NC_解译样本文件可读性 As String = "50001016"
    Public Const cOP_NC_专业资料位置对比 As String = "50001017"
    Public Const cOP_NC_解译样本文件缺失 As String = "50001018"
    Public Const cOP_NC_解译样本文件一致 As String = "50001019"
    Public Const cOP_NC_解译样本影像类型正确性 As String = "50001020"
    Public Const cOP_NC_数据表专题资料检查 As String = "50001021"
    Public Const cOP_NC_节点数检查 As String = "50001022"
    Public Const cOP_NC_精度检查 As String = "50001023"
    Public Const cOP_NC_解译样本角点 As String = "50001024"
    Public Const cOP_NC_目录结构 As String = "50001025"
    Public Const cOP_NC_解译样本像素个数 As String = "50001026"
    Public Const cOP_NC_时点核准元数据 As String = "50001027"
    Public Const cOP_NC_元数据范围 As String = "50001028"
    '10002001-10002999

    Public Const cOP_RS_数据大小 As String = "10002001"
    Public Const cOP_RS_数据格式 As String = "10002002"
    Public Const cOP_RS_文件命名 As String = "10002003"
    Public Const cOP_RS_航片编号 As String = "10002004"
    Public Const cOP_RS_数据组织 As String = "10002005"

    Public Const cOP_RS_测区覆盖 As String = "10002101"
    Public Const cOP_RS_分区覆盖 As String = "10002102"
    Public Const cOP_RS_实设航高差 As String = "10002103"
    Public Const cOP_RS_航向重叠度 As String = "10002104"
    Public Const cOP_RS_航线弯曲度 As String = "10002105"
    Public Const cOP_RS_旁向重叠度 As String = "10002106"
    Public Const cOP_RS_倾斜角 As String = "10002107"
    Public Const cOP_RS_像片旋角 As String = "10002108"
    Public Const cOP_RS_像片旋偏角 As String = "10002109"
    Public Const cOP_RS_相邻航高差 As String = "10002110"
    Public Const cOP_RS_大小航高差 As String = "10002111"

    Public Const cOP_RS_地面分辨率 As String = "10002301"

    Public Const cOP_专用_数据组织_文件命名 As String = "90001001"
    Public Const cOP_专用_数据组织_文件结构 As String = "90001002"
    Public Const cOP_竞赛_自动化检查算子 As String = "90001003"

    'GeoPdf检查算子
    Public Const cOP_PDF_图层组织 As String = "10004001"

    '国情项目临时算子
    Public Const cOP_NC_水文要素关系 As String = "50009999"

    '竞赛专用是算子
    Public Const cOP_JS_分层设色 As String = "70000001"
    Public Const cOP_JS_要素缺失 As String = "70000002"
    Public Const cOP_JS_高程精度 As String = "70000003"
    Public Const cOP_JS_平面精度 As String = "70000004"


    Public Class OperatorCodeClass
        Public Property Code As String
        Public Property Name As String
        Public Property PctlName As String
        Public Sub New(ByVal code As String, ByVal name As String)
            Me.Code = code
            Me.Name = name
        End Sub
        Public Sub New(ByVal code As String, ByVal name As String, ByVal pctlname As String)
            Me.Code = code
            Me.Name = name
            Me.PctlName = pctlname
        End Sub
        Public Sub New()
        End Sub
    End Class
#End Region
End Class
