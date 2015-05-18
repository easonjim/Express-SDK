/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ComLib;
using ComLib.ValidationSupport;


namespace ComLib.Types
{
 
    /// <summary>
    /// Time parse result.
    /// </summary>
    public class DateParseResult
    {
        public readonly bool IsValid;
        public readonly string Error;
        public readonly DateTime Start;
        public readonly DateTime End;


        /// <summary>
        /// Constructor to initialize the results
        /// </summary>
        /// <param name="valid"></param>
        /// <param name="error"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public DateParseResult(bool valid, string error, DateTime start, DateTime end)
        {
            IsValid = valid;
            Error = error;
            Start = start;
            End = end;
        }
    }



    /// <summary>
    /// Parses the dates.
    /// </summary>
    public class DateParser
    {
        /// <summary>
        /// Error for confirming start date <= end date.
        /// </summary>
        public const string ErrorStartDateGreaterThanEnd = "End date must be greater or equal to start date.";


        /// <summary>
        /// Parses a string representing 2 dates.
        /// The dates must be separated by the word "to".
        /// </summary>
        /// <param name="val"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static DateParseResult ParseDateRange(string val, IList<string> errors, string delimiter)
        {
            int ndxTo = val.IndexOf(delimiter);

            // start and end date specified.
            string strStarts = val.Substring(0, ndxTo);
            string strEnds = val.Substring(ndxTo + delimiter.Length);
            DateTime ends = DateTime.Today;
            DateTime starts = DateTime.Today;
            int initialErrorCount = errors.Count;

            // Validate that the start and end date are supplied.
            ValidationUtils.Validate(string.IsNullOrEmpty(strStarts), errors, "Start date not supplied.");
            ValidationUtils.Validate(string.IsNullOrEmpty(strEnds), errors, "End date not supplied.");

            if (errors.Count > initialErrorCount)
                return new DateParseResult(false, errors[0], TimeParserConstants.MinDate, TimeParserConstants.MaxDate);

            // Validate that format of the start and end dates.
            ValidationUtils.Validate(!DateTime.TryParse(strStarts, out starts), errors, "Start date '" + strStarts + "' is not valid.");
            ValidationUtils.Validate(!DateTime.TryParse(strEnds, out ends), errors, "End date '" + strEnds + "' is not valid.");

            if (errors.Count > initialErrorCount)
                return new DateParseResult(false, errors[0], TimeParserConstants.MinDate, TimeParserConstants.MaxDate);

            // Validate ends date greater equal to start.
            if (starts.Date > ends.Date)
            {
                errors.Add(ErrorStartDateGreaterThanEnd);
                return new DateParseResult(false, errors[0], TimeParserConstants.MinDate, TimeParserConstants.MaxDate);
            }

            // Everything is good.
            return new DateParseResult(true, string.Empty, starts, ends);
        }


        /// <summary>
        /// Handle parsing of dates with T-1, T+2 etc.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime ParseTPlusMinusX(string dateStr)
        {
            return ParseTPlusMinusX(dateStr, DateTime.MinValue);
        }


        /// <summary>
        /// Handle parsing of dates with T-1, T+2 etc.
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static DateTime ParseTPlusMinusX(string dateStr, DateTime defaultVal)
        {
            //(?<datepart>[0-9a-zA-Z\\/]+)\s*((?<addop>[\+\-]{1})\s*(?<addval>[0-9]+))? 
            string pattern = @"(?<datepart>[0-9a-zA-Z\\/]+)\s*((?<addop>[\+\-]{1})\s*(?<addval>[0-9]+))?";
            Match match = Regex.Match(dateStr, pattern);
            DateTime date = defaultVal;
            if (match.Success)
            {
                string datepart = match.Groups["datepart"].Value;
                if (datepart.ToLower().Trim() == "t")
                    date = DateTime.Now;
                else
                    date = DateTime.Parse(datepart);

                // Now check for +- days
                if (match.Groups["addop"].Success && match.Groups["addval"].Success)
                {
                    string addOp = match.Groups["addop"].Value;
                    int addVal = Convert.ToInt32(match.Groups["addval"].Value);
                    if (addOp == "-") addVal *= -1;
                    date = date.AddDays(addVal);
                }
            }
            return date;
        }
    }
}
