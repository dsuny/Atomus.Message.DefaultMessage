using Atomus.Diagnostics;
using Atomus.Message.Controllers;
using Atomus.Service;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Atomus.Message
{
    public class DefaultMessage : IMessageSource
    {
        private static DataTable dataTable;

        public DefaultMessage()
        {
            IResponse response;

            if (dataTable == null)
            {
                try
                {
                    response = this.Search();

                    if (response.Status == Status.OK)
                    {
                        dataTable = response.DataSet.Tables[0];
                    }
                }
                catch (Exception ex)
                {
                    DiagnosticsTool.MyTrace(ex);
                }
            }
        }

        MessageResult IMessageSource.GetMessage(string module, string code, string message)
        {
            MessageResult messageResult;

            messageResult = new MessageResult
            {
                Message = message
            };

            if (dataTable == null)
                return messageResult;

            var a = (from sel in dataTable.AsEnumerable()
                     where sel.Field<string>("MODULE") == module
                     && sel.Field<string>("CODE") == code
                     select new
                     {
                         Title = sel.Field<string>("TITLE"),
                         Text = sel.Field<string>("LANGUAGE_TEXT"),
                         Icon = sel.Field<string>("ICON"),
                         Button = sel.Field<string>("BUTTONS")
                     });

            if (a.Count() >= 1)
            {
                messageResult.Title = a.ToList()[0].Title;
                messageResult.Message = a.ToList()[0].Text;
                messageResult.MessageBoxIcon = this.GetMessageBoxIcon(a.ToList()[0].Icon);
                messageResult.MessageBoxButtons = this.GetMessageBoxButtons(a.ToList()[0].Button);
                messageResult.Result = true;
            }

            return messageResult;
        }

        private MessageBoxIcon GetMessageBoxIcon(string messageBoxIcon)
        {
            switch (messageBoxIcon)
            {
                case "Hand":
                    return MessageBoxIcon.Hand;
                case "Stop":
                    return MessageBoxIcon.Stop;
                case "Error":
                    return MessageBoxIcon.Error;
                case "Question":
                    return MessageBoxIcon.Question;
                case "Exclamation":
                    return MessageBoxIcon.Exclamation;
                case "Warning":
                    return MessageBoxIcon.Warning;
                case "Asterisk":
                    return MessageBoxIcon.Asterisk;
                case "Information":
                    return MessageBoxIcon.Information;
                default:// "None"
                    return MessageBoxIcon.None;
            }
        }
        private MessageBoxButtons GetMessageBoxButtons(string messageBoxButtons)
        {
            switch (messageBoxButtons)
            {
                case "OKCancel":
                    return MessageBoxButtons.OKCancel;
                case "AbortRetryIgnore":
                    return MessageBoxButtons.AbortRetryIgnore;
                case "YesNoCancel":
                    return MessageBoxButtons.YesNoCancel;
                case "YesNo":
                    return MessageBoxButtons.YesNo;
                case "RetryCancel":
                    return MessageBoxButtons.RetryCancel;
                default:// "OK"
                    return MessageBoxButtons.OK;
            }
        }
    }
}