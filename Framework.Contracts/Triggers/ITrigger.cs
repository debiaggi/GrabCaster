// --------------------------------------------------------------------------------------------------
// <copyright file = "ITrigger.cs" company="Nino Crudele">
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
namespace GrabCaster.Framework.Contracts.Triggers
{
    using GrabCaster.Framework.Contracts.Globals;

    /// <summary>
    /// The TriggerType interface.
    /// </summary>
    public interface ITriggerType
    {
        /// <summary>
        /// Internal Trigger context.
        /// </summary>
        EventActionContext Context { get; set; }

        /// <summary>
        ///     internal delegate to use in delegates events
        /// </summary>
        SetEventActionTrigger SetEventActionTrigger { get; set; }

        /// <summary>
        ///     Main default data property
        /// </summary>
        byte[] DataContext { get; set; }

        /// <summary>
        /// Main default method
        /// </summary>
        /// <param name="setEventActionTrigger">
        /// The set Event Action Trigger.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        void Execute(SetEventActionTrigger setEventActionTrigger, EventActionContext context);
    }
}