using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class EmailService : IEmailService
    {
        #region Dependencies

        private readonly ILogger<EmailService> _logger;

        #endregion

        #region .ctor

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Consts

        public const string DateFormat = "dd.MM.yyyy";

        #endregion

        #region Public Methods

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

        #endregion

        #region Private Methods

        private string ReplaceForgotPasswordSuccessMessageTokens(string token, string rawText, string email)
        {
            var specialTokens = new Dictionary<string, object>
            {
                { token, email}
            };
            return ReplaceTokens(rawText, specialTokens);
        }


        private StringBuilder ReplaceToken(StringBuilder rawText, string tokenKey, object tokenValue)
        {
            if (tokenValue is DateTime || tokenValue == null)
            {
                var resultText = rawText;
                foreach (System.Text.RegularExpressions.Match match in Regex.Matches(rawText.ToString(), tokenKey))
                {
                    var index = resultText.ToString().IndexOf(match.ToString(), StringComparison.Ordinal);
                    resultText = ReplaceTokenDateTime(resultText, match.ToString(), tokenValue, index);
                }
                return resultText;
            }

            return ReplaceTokenString(rawText, tokenKey, tokenValue.ToString());
        }

        private StringBuilder ReplaceTokenString(StringBuilder rawText, string tokenKey, string tokenValue)
        {
            return rawText?.Replace(tokenKey, tokenValue);
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

            return rawText.Replace(token, formattedDateTimeValue, tokenIndex, token.Length);
        }

        private string GetFormat(string rawText, string token)
        {
            var startCharIndex = rawText.IndexOf(token, StringComparison.Ordinal) + token.Length;
            if (rawText.Length > startCharIndex && rawText[startCharIndex] == '{')
            {
                var endIndex = rawText.IndexOf('}', startCharIndex) + 1;
                return rawText.Substring(startCharIndex, endIndex - startCharIndex);
            }

            return string.Empty;
        }

        private string GetFormatedDateTimeValue(DateTime dateTime, string format)
        {
            try
            {
                return dateTime.ToString(format.Replace("{", string.Empty).Replace("}", string.Empty), CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
