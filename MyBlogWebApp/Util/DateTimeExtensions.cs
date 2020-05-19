using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlogWebApp.Util
{
    public static class DateTimeExtensions
    {
        public enum Format
        {
            YearToMonth = 0,
            YearToSec = 1,
        }

        public static string ToISOString(this DateTime dateTime, Format format = Format.YearToSec)
        {
            string resultStr = string.Empty;

            switch (format)
            {
                case Format.YearToMonth:
                    resultStr = dateTime.ToString("yyyy-MM-dd");
                    break;
                case Format.YearToSec:
                    resultStr = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                    break;
            }

            return resultStr;
        }

        public static string ToISOStringWithUtcTimezoneStr(this DateTime dateTime, Format format = Format.YearToSec)
        {
            string resultStr = string.Empty;

            switch (format)
            {
                case Format.YearToMonth:
                    resultStr = string.Format("{0}Z", dateTime.ToString("yyyy-MM-dd"));
                    break;
                case Format.YearToSec:
                    resultStr = string.Format("{0}Z", dateTime.ToString("yyyy-MM-ddTHH:mm:ss"));
                    break;
            }

            return resultStr;
        }

        public static string ToISOStringWithInstanceTimezoneStr(this DateTime dateTime, Format format = Format.YearToSec)
        {
            // DateTime.ToStringで"zzz"書式指定子を使っても、実行OSのローカルタイムゾーンの、UTCを基準とした符号付きオフセット(時間および分単位)を表すのであって、
            // DateTimeインスタンスのDateTime.Kindプロパティの値は反映されないため、DateTime値に対して"zzz"書式指定子を使用することは目的と異なる。
            // DateTimeOffset.ToStringで"zzz"書式指定子を使えば、DateTimeOffset値の、UTCを基準とするオフセット(時間および分単位)を表してくれるので、
            // DateTime値をDateTimeOffsetに変換して、DateTimeOffset.ToStringで"zzz"書式指定子を用いる。
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
            string resultStr = string.Empty;

            switch (format)
            {
                case Format.YearToMonth:
                    resultStr = dateTimeOffset.ToString("yyyy-MM-ddzzz");
                    break;
                case Format.YearToSec:
                    resultStr = dateTimeOffset.ToString("yyyy-MM-ddTHH:mm:sszzz");
                    break;
            }

            return resultStr;
        }
    }
}