// --------------------------------------------------------------------------------------------------
// <copyright file = "TwilioEvent.cs" company="Nino Crudele">
//   Copyright (c) 2013 - 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog: http://ninocrudele.me
//    
//    By accessing GrabCaster code here, you are agreeing to the following licensing terms.
//    If you do not agree to these terms, do not access the GrabCaster code.
//    Your license to the GrabCaster source and/or binaries is governed by the 
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//    
//    This work is registered with the UK Copyright Service.
//    Registration No:284695248  
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.TwilioEvent
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    using Twilio;

    /// <summary>
    /// The twilio event.
    /// </summary>
    [EventContract("{A5765B22-4003-4463-AB93-EEB5C0C477FE}", "Twilio Event", "Twilio send text message", true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class TwilioEvent : IEventType
    {
        /// <summary>
        /// Gets or sets the account sid.
        /// </summary>
        [EventPropertyContract("AccountSid", "AccountSid")]
        public string AccountSid { get; set; }

        /// <summary>
        /// Gets or sets the auth token.
        /// </summary>
        [EventPropertyContract("AuthToken", "AuthToken")]
        public string AuthToken { get; set; }

        /// <summary>
        /// Gets or sets the from.
        /// </summary>
        [EventPropertyContract("From", "From")]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the to.
        /// </summary>
        [EventPropertyContract("To", "To")]
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public EventActionContext Context { get; set; }

        /// <summary>
        /// Gets or sets the set event action event.
        /// </summary>
        public SetEventActionEvent SetEventActionEvent { get; set; }

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="setEventActionEvent">
        /// The set event action event.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <exception cref="Exception">
        /// </exception>
        [EventActionContract("{5ABB263A-8B69-49F7-BC9E-802A0A81AA0B}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                var content = Encoding.UTF8.GetString(this.DataContext);
                var twilio = new TwilioRestClient(this.AccountSid, this.AuthToken);
                var text = content.Replace("\"", string.Empty).Replace("\\", string.Empty);
                twilio.SendMessage(this.From, this.To, text);

                // SetEventActionEvent(this, context);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}