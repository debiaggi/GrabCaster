﻿// --------------------------------------------------------------------------------------------------
// <copyright file = "DialogBoxEvent.cs" company="Nino Crudele">
//   Copyright (c) 2015 Nino Crudele. All Rights Reserved.
// </copyright>
// <summary>
//    Author: Nino Crudele
//    Blog:   http://ninocrudele.me
//    Email:  nino.crudele@live.com
//    
//    Reciprocal Public License 1.5 (RPL1.5) license as described here: 
//    http://www.opensource.org/licenses/rpl1.5.txt
//    
//    Unless explicitly acquired and licensed from Licensor under another
//    license, the contents of this file are subject to the Reciprocal Public
//    License ("RPL") Version 1.5, or subsequent versions as allowed by the RPL,
//    and You may not copy or use this file in either source code or executable
//    form, except in compliance with the terms and conditions of the RPL.
//    
//    All software distributed under the RPL is provided strictly on an "AS
//    IS" basis, WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND
//    LICENSOR HEREBY DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT
//    LIMITATION, ANY WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
//    PURPOSE, QUIET ENJOYMENT, OR NON-INFRINGEMENT. See the RPL for specific
//    language governing rights and limitations under the RPL. 
//  </summary>
// --------------------------------------------------------------------------------------------------
namespace GrabCaster.Framework.DialogBoxEvent
{
    using System.Text;
    using System.Windows.Forms;

    using GrabCaster.Framework.Contracts.Attributes;
    using GrabCaster.Framework.Contracts.Events;
    using GrabCaster.Framework.Contracts.Globals;

    [EventContract("{39AD14F3-009E-45EE-83B6-CECD51E6A242}", "DialogBox Event", "Show a DialogBox", true)]
    public class DialogBoxEvent : IEventType
    {
        public EventActionContext Context { get; set; }

        public SetEventActionEvent SetEventActionEvent { get; set; }

        [EventPropertyContract("DataContext", "Event Default Main Data")]
        public byte[] DataContext { get; set; }

        [EventActionContract("{6908E16A-6763-435C-B7C9-8FDD9F333FB9}", "Main action", "Main action description")]
        public void Execute(SetEventActionEvent setEventActionEvent, EventActionContext context)
        {
            try
            {
                var rfidtag = Encoding.UTF8.GetString(this.DataContext);
                var dialogResult = MessageBox.Show(
                    $"Authorization for TAG code {rfidtag}.",
                    "Authorization TAG",
                    MessageBoxButtons.YesNo);
                this.DataContext = Encoding.UTF8.GetBytes(dialogResult == DialogResult.Yes ? true.ToString() : false.ToString());

                setEventActionEvent(this, context);
            }

            catch
            {
                // ignored
            }
        }
    }
}