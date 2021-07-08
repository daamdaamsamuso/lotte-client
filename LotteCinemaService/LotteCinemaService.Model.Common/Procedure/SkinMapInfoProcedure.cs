using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Procedure
{
    public class SkinMapInfoProcedure
    {
        /// <summary>
        /// 스킨 코드
        /// </summary>
        public string SkinID;

        /// <summary>
        /// 미디어/광고 코드
        /// </summary>
        public string GroupID;

        /// <summary>
        /// 컨텐츠 코드
        /// </summary>
        public int ContentsID;

        /// <summary>
        /// 미디어/광고 구분
        /// </summary>
        public ContentsType ContentsType;

        /// <summary>
        /// 등록일
        /// </summary>
        public DateTime RegDate;

        /// <summary>
        /// 등록 아이디
        /// </summary>
        public string RegID;

        /// <summary>
        /// 갱신 일
        /// </summary>
        public DateTime UpdateDate;

        /// <summary>
        /// 갱신 아이디
        /// </summary>
        public string UpdateRegID;

        /// <summary>
        /// 컨텐츠 이름
        /// </summary>
        public string ContentsName;

        /// <summary>
        /// 파일 이름
        /// </summary>
        public string FileName;

        /// <summary>
        /// 파일 타입
        /// </summary>
        public string FileType;

        /// <summary>
        /// 파일 크기
        /// </summary>
        public long FileSize;

        /// <summary>
        /// 아이템 위치
        /// </summary>
        public int ItemPositionID;

    }
}