#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS.WebForms.WebControls {
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Net.Mail;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Kaliko;
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
                    ((Button)c).Click += SubmitHandler;
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


        private void SubmitHandler(object sender, EventArgs e) {
            bool showThankyou = true;

            if(UseCustomFormHandler) {
                var ea = new SendFormEventArgs(_formContainer.Controls);
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
            string mailSubject = MailSubject;
            string mailTemplate = MailTemplate;

            // Fill out mail template
            foreach (Control c in _formContainer.Controls) {
                Type type = c.GetType();
                if (type == typeof(TextBox)) {
                    var textBox = ((TextBox)c);
                    mailSubject = mailSubject.Replace("@@" + c.ID + "@@", textBox.Text);
                    mailTemplate = mailTemplate.Replace("@@" + c.ID + "@@", textBox.Text);
                }
                else if (type == typeof(HiddenField)) {
                    var hiddenField = ((HiddenField)c);
                    mailSubject = mailSubject.Replace("@@" + c.ID + "@@", hiddenField.Value);
                    mailTemplate = mailTemplate.Replace("@@" + c.ID + "@@", hiddenField.Value);
                }
                else if (type == typeof(DropDownList)) {
                    var dropDownList = ((DropDownList)c);
                    mailSubject = mailSubject.Replace("@@" + c.ID + "@@", dropDownList.SelectedValue);
                    mailTemplate = mailTemplate.Replace("@@" + c.ID + "@@", dropDownList.SelectedValue);
                }
                else if (type == typeof(RadioButtonList)) {
                    var radioButtonList = ((RadioButtonList)c);
                    mailSubject = mailSubject.Replace("@@" + c.ID + "@@", radioButtonList.SelectedValue);
                    mailTemplate = mailTemplate.Replace("@@" + c.ID + "@@", radioButtonList.SelectedValue);
                }
                Logger.Write(" Email field: " + c.ID);
            }

            // Fill out predefined tags
            mailTemplate = mailTemplate.Replace("@@timestamp@@", DateTime.Now.ToString(CultureInfo.InvariantCulture));

            // Setup mail objects
            // TODO: Bryt ut till egen klass för mailhantering
            var smtp = new SmtpClient();
            var mail = new MailMessage(MailFrom, MailTo) { 
                Subject = mailSubject, 
                Body = mailTemplate, 
                IsBodyHtml = true 
            };

            smtp.Send(mail);
        }

        private bool UseCustomFormHandler {
            get {
                return SendForm != null;
            }
        }

        private void ShowThankYouMessage() {
            AddThankYouMessage();

            if (HideFormAfterSend) {
                RemoveForm();
            }
        }

        private void AddThankYouMessage() {
            Control placeHolder = new PlaceHolder();
            ThankYouTemplate.InstantiateIn(placeHolder);
            Controls.AddAt(0, placeHolder);
        }

        private void RemoveForm() {
            Controls.Remove(_formContainer);
        }
    }
}