﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Model.Enum
{
    public enum CMSMenuType
    {
        DID_Schedule = 1,
        DID_Schedule_WF = 1020100,
        DID_Schedule_TBA = 1010100,
        DID_Schedule_TBA_Transparent = 1010101,
        DID_Schedule_TBA_Poster = 1010200,
        DID_Schedule_MC = 1020300,
        DID_Schedule_TMBMain = 1020200,
        DID_Schedule_TMBSub = 1020201,
        DID_Schedule__DG = 1020400,
        DID_Schedule_DC = 1020500,
        DID_Schedule_DW = 1020600,
        DID_Schedule_NS = 1010300,
        DID_Schedule_CS = 1010400,
        DID_Schedule_ES = 1010500,
        DID_Schedule__SI = 1010600,
        DID_Schedule__SIBackground = 1010601,
        DID_Management = 2,
        DID_Management_ADReg = 21,
        DID_Management_ADReg_WF = 2120100,
        DID_Management_ADReg_TBA = 2110100,
        DID_Management_ADReg_TBATransparent = 2110101,
        DID_Management_ADReg_TBAPoster = 2110200,
        DID_Management_ADReg_MC = 2120300,
        DID_Management_ADReg_TMBMain = 2120200,
        DID_Management_ADReg_TMBSub = 2121201,
        DID_Management_ADReg_DG = 2120400,
        DID_Management_ADReg_DC = 2120500,
        DID_Management_ADReg_DW = 2120600,
        DID_Management_ADReg_NS = 2110300,
        DID_Management_ADReg_CS = 2110400,
        DID_Management_ADReg_ES = 2110500,
        DID_Management_ADReg_ESBackground = 2110501,
        DID_Management_ADReg_SI = 2110600,
        DID_Management_ADReg_SIBackground = 2110601,
        DID_Management_ContentsReg = 22,
        DID_Management_ContentsReg_TBAComeSoon = 2210200,
        DID_Management_ContentsReg_TMB = 2220200,
        DID_Management_ContentsReg_DG = 2220400,
        DID_Management_ContentsReg_DC = 2220500,
        DID_Management_ContentsReg_DW = 2220600,
        DID_Management_ContentsReg_DS = 2220700,
        DID_Management_ContentsMenuBoard = 1234567,
        DID_Management_ContentsMiniNotice = 1234568,
        DID_Management_Event = 23,
        DID_Management_Event_TMB = 2320200,
        DID_Management_Event_DC = 2320500,
        DID_Management_Event_IS = 2330000,
        DID_Management_Skin = 24,
        DID_Management_Skin_WF = 2420100,
        DID_Management_Skin_TMB = 2410100,
        DID_Management_Skin_DG = 2420400,
        DID_Management_Skin_DC = 2420500,
        DID_Management_Skin_DS = 2420700,
        DID_Management_Skin_ES = 2410500,
        DID_Management_PopupNotice = 2490099,
        DID_Management_ADStatus = 25,
        DID_Management_ADStatus_WF = 2520100,
        DID_Management_ADStatus_TBA = 2510100,
        DID_Management_ADStatus_TBATransparent = 2510101,
        DID_Management_ADStatus_TBAPoster = 2510200,
        DID_Management_ADStatus_MC = 2520300,
        DID_Management_ADStatus_TMBMain = 2520200,
        DID_Management_ADStatus_TMBSub = 2521201,
        DID_Management_ADStatus_DG = 2520400,
        DID_Management_ADStatus_DC = 2520500,
        DID_Management_ADStatus_DW = 2520600,
        DID_Management_ADStatus_NS = 2510300,
        DID_Management_ADStatus_CS = 2510400,
        DID_Management_ADStatus_ES = 2510500,
        DID_Management_ADStatus_SI = 2510600,
        DID_Operation = 3,
        DID_Operation_OneNotice = 3090009,
        DID_Operation_PopupNotice = 3090099,
        DID_Operation_Event = 3010510,
        DID_Operation_TBA_Inspection = 3010118,
        DID_Operation_NS_MovieRegstration = 3010306,
        DID_Operation_CS_MovieRegstration = 3010406,
        DID_Operation_IS_HomeMain = 3030019,
        DID_Operation_IS_SpecialImage = 3030006,
        DID_Operation_DS_Notice = 3020700,
        DID_Operation_DW_EmergencyNotice = 3020600,
        
        DID_FileUploader = 4,
        DID_FileUploader_Register = 4000000,
        DID_FileUploader_List = 4000001
    }
}
