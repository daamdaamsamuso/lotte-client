using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaLibraries.Common.LCRenewal
{
    /// <summary>
    /// 상영관구분코드
    /// </summary>
    public enum LCScreenDivCode
    {
        None,
        Normal,
        CineCouple,
        CharLotte,
        CharLottePrivate,
        ArteClassic,
        ArteAni,
        WineCinemaTrain,
        KTXCinema,
        SuperSound,
        SuperVibe,
        /// <summary>Cine Couple 좌석관</summary>
        CineCoupleSeat,
        Super4D,
        SuperPlex,
        SuperPlexG,
        CineBiz,
        CineFamily,
        JangAnUniv,
        Wow,
        SuperS,
        //2020.12.01
        COLORIUM
    }

    /// <summary>
    /// 필름코드
    /// </summary>
    public enum LCFilmCode
    {
        None,
        Film,
        _2D,
        _3D,
        _4D
    }

    /// <summary>
    /// 4D구분코드 
    /// </summary>
    public enum LC4DTypeCode
    {
        None,
        Normal,
        _4D
        //Super4D,
        //SuperVibe,
        //Normal_SuperVibe
    }

    /// <summary>
    /// 관람등급
    /// </summary>
    public enum LCPermissionLevel
    {
        None,
        Universal,
        _12,
        _15,
        _18,
        Limit
    }

    /// <summary>
    /// 자막코드
    /// </summary>
    public enum LCCaptionCode
    {
        None,
        Dubbing,
        Subtitle,
        /// <summary>수화</summary>
        SignLanguage,
    }
}
