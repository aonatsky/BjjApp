using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TRNMNT.Core.Services.Interface;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.impl
{
    class EmailService : IEmailService
    {
        #region dependencies
        private readonly ILogger<EmailService> logger;
        #endregion


        #region ctor
        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }
        #endregion

        #region consts

        public const string DateFormat = "dd.MM.yyyy";

        #endregion


        #region public methods

        public Task SendForgotPasswordEmail(User user)
        {
            throw new NotImplementedException();
        }

        public Task SendEventParticipationEmail(User user, Participant participant, Event _event)
        {
            throw new NotImplementedException();
        }

        public Task SendEventParticipationApprovalEmail(User user, Event _event)
        {
            throw new NotImplementedException();
        }

        public Task SendTeamRegistrationEmail(User user, Team team)
        {
            throw new NotImplementedException();
        }

        public Task SendMultipleParticipationEmail(User user, IEnumerable<Participant> participants, Team team)
        {
            throw new NotImplementedException();
        }



        #endregion

        #region private methods

        private string ReplaceForgotPasswordSuccessMessageTokens(string token, string rawText, string email)
        {
            var specialTokens = new Dictionary<string, object>
                                    {
                                        {token, email},
                                    };
            return ReplaceTokens(rawText, specialTokens);
        }


        public string ReplaceTokens(string rawText, Dictionary<string, object> tokens)
        {
            var procesedText = new StringBuilder(rawText);
            foreach (var token in tokens)
            {
                procesedText = ReplaceToken(procesedText, token.Key, token.Value);
            }
            return procesedText.ToString();
        }

        public string ReplaceTokens(string rawText, Dictionary<string, string> tokens)
        {
            var procesedText = new StringBuilder(rawText);
            foreach (var token in tokens)
            {
                procesedText = ReplaceToken(procesedText, token.Key, token.Value);
            }
            return procesedText.ToString();
        }

        private StringBuilder ReplaceToken(StringBuilder rawText, string tokenKey, object tokenValue)
        {
            if (tokenValue is DateTime || tokenValue == null)
            {
                var resultText = rawText;
                foreach (Match match in Regex.Matches(rawText.ToString(), tokenKey))
                {
                    var index = resultText.ToString().IndexOf(match.ToString());
                    resultText = ReplaceTokenDateTime(resultText, match.ToString(), tokenValue, index);
                }
                return resultText;
            }
            else
            {
                return ReplaceTokenString(rawText, tokenKey, tokenValue.ToString());
            }
        }

        private StringBuilder ReplaceTokenString(StringBuilder rawText, string tokenKey, string tokenValue)
        {
            return rawText != null ? rawText.Replace(tokenKey, tokenValue) : null;
        }

        private StringBuilder ReplaceTokenDateTime(StringBuilder rawText, string tokenKey, object tokenValue, int tokenIndex)
        {
            string formattedDateTimeValue = null;
            var token = tokenKey;
            var format = GetFormat(rawText.ToString(), tokenKey);
            if (!string.IsNullOrEmpty(format))
            {
                token = token + format;
            }
            if (tokenValue != null)
            {
                var dateValue = (DateTime)tokenValue;
                if (!string.IsNullOrEmpty(format) && !string.IsNullOrEmpty(format.Trim('{', '}')))
                {
                    formattedDateTimeValue = GetFormatedDateTimeValue(dateValue, format);
                }
                if (string.IsNullOrEmpty(formattedDateTimeValue))
                {
                    formattedDateTimeValue = dateValue.ToString(DateFormat);
                }
            }

            return rawText != null ? rawText.Replace(token, formattedDateTimeValue, tokenIndex, token.Length) : null;
        }

        private string GetFormat(string rawText, string token)
        {
            string format;
            var startCharIndex = rawText.IndexOf(token) + token.Length;
            if (rawText.Length > startCharIndex && rawText[startCharIndex] == '{')
            {
                var endIndex = rawText.IndexOf('}', startCharIndex) + 1;
                format = rawText.Substring(startCharIndex, endIndex - startCharIndex);
            }
            else
            {
                format = "";
            }
            return format;
        }

        private string GetFormatedDateTimeValue(DateTime dateTime, string format)
        {
            try
            {
                IFormatProvider provider = CultureInfo.InvariantCulture;
                return dateTime.ToString(format.Replace("{", string.Empty).Replace("}", string.Empty), provider);
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion
    }
}
