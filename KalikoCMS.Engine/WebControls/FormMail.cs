#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz and Contributors
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */
#endregion

namespace KalikoCMS.WebControls {
    using System;
    using System.ComponentModel;
    using System.Net.Mail;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using KalikoCMS.Events;

    public class FormMail : CustomWebControl {

        private Control _formContainer;


        public delegate void SendFormEventHandler(Object sender, SendFormEventArgs e);

        public event SendFormEventHandler SendForm;

        
        protected override void CreateChildControls() {
            _formContainer = new PlaceHolder();
            FormTemplate.InstantiateIn(_formContainer);
            Controls.Add(_formContainer);

            HookupEventOnSubmitButton();

            base.CreateChildControls();
        }

        private void HookupEventOnSubmitButton() {
            foreach (Control c in _formContainer.Controls) {
                if (c.ID == SendButton) {
                    ((Button)c).Click += FormMail_Click;
                }
            }
        }


        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate FormTemplate { get; set; }

        [Browsable(false),
        DefaultValue(null),
        PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate ThankYouTemplate { get; set; }

        [Browsable(false),
         DefaultValue(null),
         PersistenceMode(PersistenceMode.InnerProperty)]
        public string MailTemplate { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string SendButton { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public bool HideFormAfterSend { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public Uri ThanksUrl { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string ExtraEmailId { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string MailFrom { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string MailTo { get; set; }

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public string MailSubject { get; set; }


        protected virtual void OnSendForm(SendFormEventArgs e) {
            SendForm(this, e);
        }


        private void FormMail_Click(object sender, EventArgs e) {
            bool showThankyou = true;

            // Check if a custom form handler has been assigned..
            if(UseCustomFormHandler) {
                SendFormEventArgs ea = new SendFormEventArgs(_formContainer.Controls);
                OnSendForm(ea);
                showThankyou = ea.FormWasSentCorrectly;
            }
            else {
                SendFormAsEmail();
            }

            if(ThanksUrl != null) {
                HttpContext.Current.Response.Redirect(ThanksUrl.ToString());
            }
            else if(showThankyou) {
                ShowThankYouMessage();
            }
        }

        private void SendFormAsEmail() {
            // No custom form handler was found, do the default mailing..
            string extraemail = string.Empty;
            string mailSubject = MailSubject;
            string mailTemplate = MailTemplate;

            // Fill out mail template
            foreach (Control c in _formContainer.Controls) {
                Type type = c.GetType();
                if (type == typeof(TextBox)) {
                    TextBox textBox = ((TextBox)c);
                    mailSubject = mailSubject.Replace("@@" + c.ID + "@@", textBox.Text);
                    mailTemplate = mailTemplate.Replace("@@" + c.ID + "@@", textBox.Text);

                    if (c.ID == ExtraEmailId) {
                        extraemail = textBox.Text;
                    }
                }
                else if (type == typeof(HiddenField)) {
                    HiddenField hiddenField = ((HiddenField)c);
                    mailSubject = mailSubject.Replace("@@" + c.ID + "@@", hiddenField.Value);
                    mailTemplate = mailTemplate.Replace("@@" + c.ID + "@@", hiddenField.Value);
                }
                else if (type == typeof(DropDownList)) {
                    DropDownList dropDownList = ((DropDownList)c);
                    mailSubject = mailSubject.Replace("@@" + c.ID + "@@", dropDownList.SelectedValue);
                    mailTemplate = mailTemplate.Replace("@@" + c.ID + "@@", dropDownList.SelectedValue);
                }
                else if (type == typeof(RadioButtonList)) {
                    RadioButtonList radioButtonList = ((RadioButtonList)c);
                    mailSubject = mailSubject.Replace("@@" + c.ID + "@@", radioButtonList.SelectedValue);
                    mailTemplate = mailTemplate.Replace("@@" + c.ID + "@@", radioButtonList.SelectedValue);
                }
            }

            // Fill out predefined tags
            mailTemplate = MailTemplate.Replace("@@timestamp@@", DateTime.Now.ToString());

            // Setup mail objects
            // TODO: Bryt ut till egen klass för mailhantering
            SmtpClient smtp = new SmtpClient();
            MailMessage mail = new MailMessage(MailFrom, MailTo)
            { Subject = mailSubject, Body = mailTemplate, IsBodyHtml = true };
            smtp.Send(mail);

            //if (string.IsNullOrEmpty(extraemail)) {
            //    try {
            //        mail = new MailMessage(MailFrom, extraemail)
            //        { Subject = mailSubject, Body = mailTemplate, IsBodyHtml = true };
            //        smtp.Send(mail);
            //    }
            //    catch {
            //        throw;
            //    }
            //}
        }

        private bool UseCustomFormHandler {
            get {
                return SendForm != null;
            }
        }

        private void ShowThankYouMessage() {
            // Add thank you message
            Control placeHolder = new PlaceHolder();
            ThankYouTemplate.InstantiateIn(placeHolder);
            Controls.AddAt(0, placeHolder);

            // Remove the posted form if property HideFormAfterSend is set to true
            if (HideFormAfterSend) {
                Controls.Remove(_formContainer);
            }
        }
    }
}